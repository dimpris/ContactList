using ContactList.DataContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add DB connection
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
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
                if (context.Request.Cookies.ContainsKey("access_token") && context.Request.Cookies.TryGetValue("access_token", out token))
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
   corsPolicyBuilder.WithOrigins("http://localhost:81")
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

app.MapControllers();

app.Run();
