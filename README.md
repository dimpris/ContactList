
Application that helps managing user contacts. Solution includes ASP.NET Core (both, MVC and API) and legacy Classic ASP. 
Implemented custom authentication using own DB table through the Entity Framework. 
Authorization based on JWT tokens, with shared Cookies session between Classic ASP and ASP.NET Core components.

App can work with the tokens transfered either in through auth headers or through cookies. 
App includes migrations to manage th db.
Bussines logic described in service classes to have a thin controllers.
Common folder includes some contract files and utilities for hashing and auth process.
App includec MVC and API Controllers, API covering Authentication and some data management enpoints.
ClassicASP folder includes legacy pages which can access the token and user data stored in cookies, and than request the API or query DB.

Tests project includes some unit test to check Sign Up, Sign In and Contact creation

App deployed on [https://h.dimpris.pp.ua/](https://h.dimpris.pp.ua/)

Classic ASP page can be navigated either from the navigation bar or by URL [https://h.dimpris.pp.ua/asp/Contacts.asp](https://h.dimpris.pp.ua/asp/Contacts.asp)