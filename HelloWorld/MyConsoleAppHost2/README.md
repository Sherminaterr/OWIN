This project is to test conditionally running middlewares
1) Install-Package Microsoft.Owin.SelfHost
2) Add the Startup class
3) Set this project as startup project when testing

Comparing RawMiddleware and ImprovedMiddleware:<br/>
 ImprovedMiddleware: Invoke method uses IOwinContext, passing env dictionary
 This allows referencing of strongly typed accessor, Response.
 Also, pass in GreetingOptions. This is standard way of passing in data.

 Caution<br/>
 NOT RECOMMENDED to derive middleware class from OwinMiddleware abstract class
 as can cause inter-operability issues by exposing types specific to katana implementation.
 other middleware may not be following katana.

 In here, we did use the Microsoft.Owin package but usage is within class methods, not spilled over
 to outside (e.g. other middleware or startup class)