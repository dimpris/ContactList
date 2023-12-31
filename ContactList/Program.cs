using ContactList.DataContexts;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add DB connection
string connection = builder.Configuration.GetConnectionString("DefaultConnectionAzure");
builder.Services.AddDbContext<ContactListAppContext>(options => options.UseSqlServer(connection));
builder.Services.AddMvc(opt => opt.EnableEndpointRouting = false);

// Added Authorization. Configure Authentication with JWT tokens
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("OAuth")
    .AddJwtBearer("OAuth", config => 
    {
        //config.SaveToken = true;
        config.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string token;
                if (context.Request.Cookies.ContainsKey("accessToken") && context.Request.Cookies.TryGetValue("accessToken", out token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };

        string secret = builder.Configuration["AuthOptions:SecretKey"] ?? "";
        byte[] secretBytes = Encoding.UTF8.GetBytes(secret);
        var key = new SymmetricSecurityKey(secretBytes);

        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["AuthOptions:Issuer"],
            ValidAudience = builder.Configuration["AuthOptions:Audience"],
            IssuerSigningKey = key
        };
    });

builder.Services.AddAntiforgery(options =>
{
    // Set Cookie properties using CookieBuilder properties†.
    options.FormFieldName = "AntiforgeryFieldname";
    options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
    options.SuppressXFrameOptionsHeader = false;
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use((context, next) =>
{
    var requestPath = context.Request.Path.Value;

    if (string.Equals(requestPath, "/", StringComparison.OrdinalIgnoreCase)
        || string.Equals(requestPath, "/index.html", StringComparison.OrdinalIgnoreCase))
    {
        var tokenSet = antiforgery.GetAndStoreTokens(context);
        context.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!,
            new CookieOptions { HttpOnly = false });
    }

    return next(context);
});

// On Jwt Bearer Authentication response = 401, redirect to SignOut and then to Login
app.UseStatusCodePages(async context => 
{
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized 
        && !request.Path.Value.StartsWith("/Auth")) // Prevent redirect from AuthController actions
    {
        response.Redirect("/Auth/SignOut");
    }
});

app.UseCors(corsPolicyBuilder =>
   corsPolicyBuilder
      .AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader()
);


app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Contacts}/{action=Index}/{id?}");
});

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"ClassicASP")
    ),
    RequestPath = new PathString("/asp"), 
    ServeUnknownFileTypes = true,
    DefaultContentType = "text/html"
});

app.MapControllers();

app.Run();
