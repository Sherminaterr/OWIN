using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleAppHost2
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class MachineNamingMiddleware
    {
        private readonly AppFunc next;

        public MachineNamingMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string,object> env)
        {
            //Comment this out, we want the X-Box header

            ////do nothing on inbound processing.
            ////do just pass on to next middleware in the pipeline
            //await this.next(env);

            IOwinContext context = new OwinContext(env);

            //on outbound processing, check status code
            //and appends machine name if status code > 400

            //this middleware needs to run first in pipeline,
            //so it can be last to inspect outbound message.

            //on sending headers callback is called when response headers are sent
            //the response headers get sent whenever someone writes to response stream
            //for streaming hosts. For buffering hosts, someone has to flush the response.
            context.Response.OnSendingHeaders(state =>
            {
                var response = (OwinResponse)state;

                if (response.StatusCode >= 400)
                {
                    response.Headers.Add("X-Box", new[] { System.Environment.MachineName });
                }
            }, context.Response);

            await this.next(env);

            //if(context.Response.StatusCode >= 400)
            //{
            //    context.Response.Headers.Add("X-Box",
            //        new[]{
            //            System.Environment.MachineName
            //        });
            //}
        }
    }
}
