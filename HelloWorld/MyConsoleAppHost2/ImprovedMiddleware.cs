using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleAppHost2
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    //ImprovedMiddleware: Invoke method uses IOwinContext, passing env dictionary
    //This allows referencing of strongly typed accessor, Response.
    //Also, pass in GreetingOptions. This is standard way of passing in data.

    //NOT RECOMMENDED to derive middleware class from OwinMiddleware abstract class
    //as can cause inter-operability issues by exposing types specific to katana implementation.
    //other middleware may not be following katana.

    //In here, we did use the Microsoft.Owin package but usage is within class methods, not spilled over
    //to outside (e.g. other middleware or startup class)
    public class ImprovedMiddleware
    {
        private readonly AppFunc next;
        private readonly GreetingOptions options;

        public ImprovedMiddleware(AppFunc next, GreetingOptions options)
        {
            this.next = next;
            this.options = options;
        }

        public async Task Invoke(IDictionary<string,object> env)
        {
            IOwinContext context = new OwinContext(env);

            string message = this.options.Message;
            if (this.options.isHtml)
                message = String.Format("<h1>{0}</h1>", message);

            byte[] bytes = Encoding.UTF8.GetBytes(message);

            await context.Response.WriteAsync(bytes);

            await this.next(env);
        }
    }

    public class GreetingOptions
    {
        public string Message { get; set; }
        public bool isHtml { get; set; }
    }
}
