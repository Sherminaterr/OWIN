using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Web.Http;

namespace SecuredWebApi.Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            ////sample code to generate key
            //using (var provider = new System.Security.Cryptography.RNGCryptoServiceProvider())
            //{
            //    byte[] secretKeyBytes = new byte[32];
            //    provider.GetBytes(secretKeyBytes);
            //    var key = Convert.ToBase64(secretKeyBytes);
            //}

            //jwt code

            //extension method to plugin the options
            var jwtOptions = new JwtBearerAuthenticationOptions
            {
                //check if this token is supposed to be for this server
                //minted by authorization server for us
                AllowedAudiences = new[] { "http://localhost:5000/api" },

                //we are faking the auth server... assume it is this issuer
                //accept only when issuer is this value
                IssuerSecurityKeyProviders = new[]
                {
                    new SymmetricKeyIssuerSecurityKeyProvider(
                        issuer: "http://authzserver.demo",
                        base64Key:"80ssGCKB931r2r8Lm3om1SIt5YY05yBZ34pna3+dYi8=")
                }
            };

            app.UseJwtBearerAuthentication(jwtOptions);

            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "default", "api/{controller}/{id}");

            app.UseWebApi(config);
        }
    }
}
