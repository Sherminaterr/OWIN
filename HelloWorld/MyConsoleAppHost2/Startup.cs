using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyConsoleAppHost2.Startup))]

namespace MyConsoleAppHost2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //runs only in this sub-directory
            app.Map("/planets", helloApp =>
            {
                helloApp.Use(async (
                    IOwinContext context, Func<Task> next) =>
                {
                    await context.Response.WriteAsync("<h1>Hello Mercury</h1>");

                    await next.Invoke();

                    await context.Response.WriteAsync("<h1>Hello Mercury on return</h1>");
                });

                helloApp.Run(async (IOwinContext context) =>
                {
                    await context.Response.WriteAsync("<h1>Hello Neptune</h1>");
                });
            });

            //runs in entire app
            app.Run(async (IOwinContext context) =>
            {
                await context.Response.WriteAsync("<h1>Hello Universe</h1>");
            });
        }
    }
}
