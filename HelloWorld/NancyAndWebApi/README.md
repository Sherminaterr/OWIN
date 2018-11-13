Composing your app from middlewares - NancyAndWebApi projects
1. Install the following using Nuget package manager
Install-Package Nancy.Owin
Install-Package Microsoft.AspNet.WebApi.Owin
Install-Package Microsoft.Owin.SelfHost
Install-Package Microsoft.Owin.StaticFiles -Pre (may or may not need the Pre)
2. Create the following classes:
EmployeesController
MeteringMiddleware
HomeModule
Startup
3. Modify the program class
4. You can try navigating to the pages, and you will get meter response on your console window, for example:
Response body to GET http://localhost:5000/ is 19 bytes
Response body to GET http://localhost:5000/files is 30058 bytes
Response body to GET http://localhost:5000/files/photo1 is 30058 bytes
Response body to GET http://localhost:5000/files/photo1.png is 507133 bytes
Response body to GET http://localhost:5000/api/employees/1 is 91 bytes