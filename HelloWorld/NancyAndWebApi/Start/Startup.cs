using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.FileSystems;
using NancyAndWebApi.Middleware;
using Owin;
using System.Web.Http;
using Microsoft.Owin;

namespace NancyAndWebApi.Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Output by metering app
            /*
             Server ready ... Press Enter to quit.
            Response body to GET http://localhost:5000/ is 19 bytes
            Response body to GET http://localhost:5000/files is 30058 bytes
            Response body to GET http://localhost:5000/files/photo1 is 30058 bytes
            Response body to GET http://localhost:5000/files/photo1.png is 507133 bytes
            Response body to GET http://localhost:5000/api/employees/1 is 91 bytes
             */

            app.Use<MeteringMiddleware>();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileSystem = new PhysicalFileSystem(@"D:\ASP\OwinCode\NancyDrive"),
                RequestPath = new PathString("/files"),
            });

            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("default", "api/{controller}/{id}");

            app.UseWebApi(config);

            app.UseNancy();
        }
    }
}
