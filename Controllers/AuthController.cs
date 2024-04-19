using BoxBoxApi.Helpers;
using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryBoxBox repo;
        private HelperToken helper;

        public AuthController
            (RepositoryBoxBox repo, HelperToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        // POST api/auth/login
        /// <summary>
        /// Obtiene un TOKEN con Email y Password de un Usuario
        /// </summary>
        /// <remarks>
        /// Incluir los siguientes datos: 
        /// Admin = Email: admin@gmail.com, Password: admin123
        /// User = Email: user@gmail.com, Password: user123
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>        
        /// <response code="401">NotAuthorized. No autorizado, sin Token válido.</response>         
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(string email, string password)
        {
            User user = await
                this.repo.LoginUserAsync(email, password);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken()
                    , SecurityAlgorithms.HmacSha256);
                string jsonUser = JsonConvert.SerializeObject(user);
                Claim[] infoUser = new[]
                {
                    new Claim("UserData", jsonUser)
                };
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: infoUser,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler().WriteToken(token)
                    });
            }
        }
    }
}
