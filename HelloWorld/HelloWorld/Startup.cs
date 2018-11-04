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
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            //we can define a simple middleware in form of Func<IDictionary<string,object>,Task>
            AppFunc middleware = (IDictionary<string, object> env) =>
            {
                var response = (Stream)env["owin.ResponseBody"];

                byte[] bytes = Encoding.UTF8.GetBytes("Hello World");

                return response.WriteAsync(bytes, 0, bytes.Length);

            };

            //plugin the middleware into our pipeline
            app.Use(middleware);
        }

    }
}
