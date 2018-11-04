using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleAppHost2
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class RawMiddleware
    {
        private readonly AppFunc next;

        //receives param AppFunc to be saves as readonlt variable next for later use
        public RawMiddleware(AppFunc next)
        {
            this.next = next;
        }

        //Convention: Must be this name, Katana identifies method through this name using reflection
        //invokes AppFunc saved in field next, and passing environment dictionary, to pass control to next middleware
        public async Task Invoke(IDictionary<string,object> env)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("<h1>Hello Raw</h1>");

            var response = (Stream)env["owin.ResponseBody"];
            await response.WriteAsync(bytes, 0, bytes.Length);

            await this.next(env);
        }
    }
}
