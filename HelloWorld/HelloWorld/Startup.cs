using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    /*
     We deploy as class library, and changed deploy location to /bin instead of /bin/Debug.
     With OwinHost.exe installed, go command prompt:
     cd <path of this program>\bin
     OwinHost

    An error will occur for middleware that is defined as Func<IDictionary<string,object>,Task>

    Reason: Use method does not accept Func<IDictionary<string,object>,Task>
    A middleware is actually Func<AppFunc,AppFunc>
    Application delegate of Func<IDictionary<string,object>,Task> takes in environment dictionary and returns Task.
    - Context of request is described by environment dictionary
    - OWIN component gets chance to review request and act on it.
    - To be able to chain components, there needs to be a reference to another middleware.
    - Func<AppFunc,AppFunc> -> takes into account the reference to the next component, with incoming AppFunc being that reference.
     */
    //using AppFunc = Func<IDictionary<string, object>, Task>;
    //public class Startup
    //{
    //    public void Configuration(IAppBuilder app)
    //    {

    //        //we can define a simple middleware in form of Func<IDictionary<string,object>,Task>
    //        AppFunc middleware = (IDictionary<string, object> env) =>
    //        {
    //            var response = (Stream)env["owin.ResponseBody"];

    //            byte[] bytes = Encoding.UTF8.GetBytes("Hello World");

    //            return response.WriteAsync(bytes, 0, bytes.Length);

    //        };

    //        //plugin the middleware into our pipeline
    //        app.Use(middleware);
    //    }

    //}

    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var middleware = new Func<AppFunc, AppFunc>(HelloWorldMiddleware);

            app.Use(middleware);
        }
        
        //called during pipeline building
        public AppFunc HelloWorldMiddleware(AppFunc nextMiddleware)
        {
            AppFunc appFunc = (IDictionary<string, object> env) =>
            {
                //Called per request
                //env is the environment dictionary

                //The content we want to send
                byte[] bytes = Encoding.UTF8.GetBytes("<h1>Hello World</h1>");

                //Response headers requires to specify what is the content type and length
                var headers = (IDictionary<string, string[]>)env["owin.ResponseHeaders"];
                headers["Content-Length"] = new[] { bytes.Length.ToString() };
                headers["Content-Type"] = new[] { "text/html" };

                //get stream that represents the response body
                var response = (Stream)env["owin.ResponseBody"];
                //write hello world to stream
                response.WriteAsync(bytes, 0, bytes.Length);

                //pass the control to the next middleware in pipeline
                return nextMiddleware(env);
            };

            return appFunc;
        }
    }
}
