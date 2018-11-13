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
    /// Buffers response and adds X-Response-Reader-Header to response header
    /// </summary>
    public class ResponseReadingMiddleware
    {
        private readonly AppFunc next;

        public ResponseReadingMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string,object> env)
        {
            IOwinContext context = new OwinContext(env);

            //To avoid exception of "Stream is not readable" (CanRead = 0), use buffer
            //Switch the response body stream to a memory stream
            var originalStream = context.Response.Body;
            var responseBuffer = new MemoryStream();
            context.Response.Body = responseBuffer;

            await this.next(env);

            //Seek to beginning and read the stream
            responseBuffer.Seek(0, SeekOrigin.Begin);
            //string responseBody = await this.ReadAllAsync(context.Response.Body);
            string responseBody = await this.ReadAllAsync(responseBuffer);

            Console.WriteLine(responseBody);

            //write extra header
            //This header is getting added after second middleware has written into
            //the response body stream, yet the header goes out
            context.Response.Headers.Add("X-Response-Reader-Header", new[] { "Hello: Response is read" });

            //Seek to beginning again and copy contents into the original steram
            responseBuffer.Seek(0, SeekOrigin.Begin);
            await responseBuffer.CopyToAsync(originalStream);
        }

        private async Task<string> ReadAllAsync(Stream stream)
        {
            string content = null;

            try
            {
                var reader = new StreamReader(stream);
                content = await reader.ReadToEndAsync();
            }//add breakpoint here
            catch(Exception ex)
            {
                //exception of "Stream is not readable"(CanRead = 0), occurs
                //when buffer is not used
                string exception = ex.Message;
            }//add breakpoint here

            return content;
        }
    }
}
