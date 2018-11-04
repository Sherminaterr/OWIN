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
            //MapAndMapWhen(app);

            //this middleware needs to run first in pipeline,
            //so it can be last to inspect outbound message.
            app.Use<MachineNamingMiddleware>();

            app.Use(
                async (IOwinContext context, Func<Task> next) =>
                {
                    //Simulate file not found
                    context.Response.StatusCode = 404;
                    await next.Invoke();
                });

            app.Run(async (IOwinContext context) =>
            {
                ////wont see X-Box response header.
                ////await context.Response.WriteAsync("<h1>Hello Universe</h1>");

                //var bytes = System.Text.Encoding.UTF8.GetBytes("<h1>Hello Universe</h1>");
                ////to see the X-Box response header, need the content length?
                ////not really, as headers get sent out on first write to response body stream
                ////any changes to HTTP status code or HTTP response headers after a middleware component starts
                ////writing to response body will not have any effect on the response message parts that have
                ////already been received by client

                ////if change order of these 2 statements, content length no longer will be sent.
                //context.Response.ContentLength = bytes.Length;

                //await context.Response.WriteAsync(bytes);

                //2nd attempt
                var bytes = System.Text.Encoding.UTF8.GetBytes("<h1>Hello World</h1>");
                var moreBytes = System.Text.Encoding.UTF8.GetBytes("<h1>Hello Universe</h1>");

                context.Response.ContentLength = bytes.Length + moreBytes.Length;

                //add breakpoint on line below
                await context.Response.WriteAsync(bytes);

                await context.Response.WriteAsync(moreBytes);
            });
        }

        private static void MapAndMapWhen(IAppBuilder app)
        {
            //runs only in this sub-directory
            app.Map("/planets", helloApp =>
            {
                //map to subdirectory /planets/3
                helloApp.Map("/3", helloEarth =>
                {
                    helloEarth.Run(async (IOwinContext context) =>
                    {
                        await context.Response.WriteAsync("<h1>Hello Earth</h1>");
                    });
                });

                //inspect URI path, based on what it sees, run a pipeline
                //takes in a Func delegate which accept IOwinContext and returns a bool
                //if true, pipeline is defined by second parameter, which Action<IAppbuilder> runs
                //the pipeline only runs when condition is true.
                //can inspect anything exposed by content to decide if pipeline should run or not, at run time.
                helloApp.MapWhen(context =>
                {
                    if (context.Request.Path.HasValue)
                    {
                        int position;

                        //if /planets/<number>, number>8
                        if (int.TryParse(context.Request.Path.Value.Trim('/'), out position))
                        {
                            if (position > 8)
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                },
                helloPluto =>
                {
                    helloPluto.Run(async (IOwinContext context) =>
                    {
                        await context.Response.WriteAsync("<h1>Opps! We are out of Solar System</h1>");
                    });
                }
                );

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
