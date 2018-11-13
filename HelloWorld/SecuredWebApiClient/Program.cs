using System;
using System.Net.Http;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SecuredWebApiClient
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            //string key = "";
            //using (var provider = new System.Security.Cryptography.RNGCryptoServiceProvider())
            //{
            //    byte[] secretKeyBytes = new byte[32];
            //    provider.GetBytes(secretKeyBytes);
            //    key = Convert.ToBase64String(secretKeyBytes);
            //}

            //Console.WriteLine(key);
            //Console.ReadLine();

            string jwt = GetJwtFromTokenIssuer();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            var result = client.GetStringAsync("http://localhost:5000/api/employees/123").Result;

            Console.WriteLine(result);
            Console.ReadLine();
        }

        static string GetJwtFromTokenIssuer()
        {
            byte[] key = Convert.FromBase64String("80ssGCKB931r2r8Lm3om1SIt5YY05yBZ34pna3+dYi8=");

            var symmetricKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key);
            //var symmetricKey = new InMemorySymmetricSecurityKey(key);

            var signingCredentials =
                new Microsoft.IdentityModel.Tokens.SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var claimsIdentity = new System.Security.Claims.ClaimsIdentity(new System.Security.Claims.Claim[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "Johnny")
                });

            var descriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Audience = "http://localhost:5000/api",
                Issuer = "http://authzserver.demo",
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = signingCredentials
            };
               
            //var descriptor = new SecurityTokenDescriptor()
            //{
            //    TokenIssuerName = "http://authzserver.demo",
            //    AppliesToAddress = "http://localhost:5000/api",
            //    Lifetime = new System.IdentityModel.Protocols.WSTrust.Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(1)),
            //    SigningCredentials = signingCredentials,
            //    Subject = new System.Security.Claims.ClaimsIdentity(new System.Security.Claims.Claim[]
            //    {
            //        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "Johnny")
            //    })
            //};

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
