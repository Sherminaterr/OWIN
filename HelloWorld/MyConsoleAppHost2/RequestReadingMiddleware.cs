using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleAppHost2
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// Buffers and read request body, then output content to console.
    /// 
    /// </summary>
    public class RequestReadingMiddleware
    {
        private readonly AppFunc next;

        public RequestReadingMiddleware(AppFunc next)
        {
            this.next = next;
        }

        /// <summary>
        /// Copies request stream into memory stream,
        /// then sets memory stream as Request.Body for downstream
        /// middleware to read.
        /// It reads the stream, then reset the current pointer
        /// to beginning of stream
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public async Task Invoke(IDictionary<string,object> env)
        {
            IOwinContext context = new OwinContext(env);

            //buffer the request body
            //copies stream to memoery stream
            var requestBuffer = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBuffer);
            requestBuffer.Seek(0, SeekOrigin.Begin);

            context.Request.Body = requestBuffer;

            //Read the body
            var reader = new StreamReader(context.Request.Body);
            string content = await reader.ReadToEndAsync();

            //seek to beginning of stream 
            //for the other components to correctly
            //read the request body
            ((MemoryStream)context.Request.Body).Seek(0, SeekOrigin.Begin);

            Console.WriteLine(content);

            await this.next(env);
        }
    }
}
