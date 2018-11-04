using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

//Good practice to specify the startup explicitly
//even if class and method name follows convention
[assembly: OwinStartup(typeof(HelloWorldV2.Startup))]

namespace HelloWorldV2
{
    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //multiple middleware and components
            //call next.Invoke to call the next component in line
            //these middleware are active middleware; will always run no matter what URI path it is.
            app.Use(
                async (IOwinContext context, Func<Task> next) =>
                {
                    await context.Response.WriteAsync("<h1>Hello World</h1>");

                    await next.Invoke();

                    await context.Response.WriteAsync("<h1>Hello World on return</h1>");
                });

            app.Use(
                async (IOwinContext context, Func<Task> next) =>
                {
                    await context.Response.WriteAsync("<h1>Hello Universe</h1>");

                    await next.Invoke();

                    await context.Response.WriteAsync("<h1>Hello Universe on return</h1>");
                });

            app.Use(
                async (IOwinContext context, Func<Task> next) =>
                {
                    await context.Response.WriteAsync("<h1>Hello,Simplified World!</h1>");
                });


            //Output:
            //Hello World
            //Hello Universe
            //Hello, Simplified World!
            //Hello Universe on return
            //Hello World on return
        }

        private static void LessComplicatedConfig(IAppBuilder app)
        {
            //Use Run() method which takes in Func delegate
            //as opposed to Use method taking in object.
            //Run() takes in IOwinContext and returns Task.
            app.Run((IOwinContext context) =>
            {
                byte[] bytes = Encoding.UTF8.GetBytes("<h1>Hello,Simplified World!</h1>");

                //from IOwinContext we get response object
                //of type IOwinResponse via Response property
                var response = context.Response;
                //From IOwinResponse get typed properties to access headers
                //easier to write and read
                response.ContentType = "text/html";
                response.ContentLength = bytes.Length;

                return response.WriteAsync(bytes);
            });
        }
    }
}
