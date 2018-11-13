MyWebApi
1. This is a class library project
2. Install-Package Microsoft.AspNet.WebApi.Core here.
3. This is going to be hosted on MyConsoleAppHost2 project, OWIN hosted
4. Select MyConsoleAppHost2 project, Install-Package Microsoft.AspNet.WebApi.Owin
5. Startup.cs class implementation is in HostOWINWebApi(app) method.
6. Add reference to this project from MyConsoleAppHost2 project