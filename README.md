
Application that helps managing user contacts. Solution includes ASP.NET Core (both, MVC and API) and legacy Classic ASP. 
Implemented custom authentication using own DB table through the Entity Framework. 
Authorization based on JWT tokens, with shared Cookies session between Classic ASP and ASP.NET Core components.

App can work with the tokens transfered either in through auth headers or through cookies. 
App includes migrations to manage th db.
Bussines logic described in service classes to have a thin controllers.
Common folder includes some contract files and utilities for hashing and auth process.
App includec MVC and API Controllers, API covering Authentication and some data management enpoints.
ClassicASP folder includes legacy pages which can access the token in cookies and request the API.

Tests project includes some unit test to check Sign Up, Sign In and Contact creation

App deployed on [https://contactlist20230804150242.azurewebsites.net/](https://contactlist20230804150242.azurewebsites.net/)

Classic ASP page deployed on [https://aspcontactlist.azurewebsites.net/](https://aspcontactlist.azurewebsites.net/)