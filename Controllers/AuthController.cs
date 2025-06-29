using BoxBoxApi.Helpers;
using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
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
        private readonly RepositoryBoxBox _repo;
        private readonly HelperToken _helper;

        public AuthController
            (RepositoryBoxBox repo, HelperToken helper)
        {
            _repo = repo;
            _helper = helper;
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
        public async Task<ActionResult> Login(LoginModel loginUser)
        {
            User user = await
                _repo.LoginUserAsync(loginUser);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials =
                    new SigningCredentials(_helper.GetKeyToken()
                    , SecurityAlgorithms.HmacSha256);
                string jsonUser = JsonConvert.SerializeObject(user);
                Claim[] infoUser = new[]
                {
                    new Claim("UserData", jsonUser),
                    new Claim(ClaimTypes.Role, user.RolId.ToString())
                };
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: infoUser,
                        issuer: _helper.Issuer.Value,
                        audience: _helper.Audience.Value,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler().WriteToken(token),
                    });
            }
        }

        // POST api/auth/refresh
        /// <summary>
        /// Refresca el token JWT aunque esté expirado, validando sólo firma, issuer y audience.
        /// </summary>
        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult> Refresh()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized();

            var tokenString = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();

            var validationParameters = _helper.GetTokenValidationParameters();
            validationParameters.ValidateLifetime = false; // Ignora expiración para refrescar

            ClaimsPrincipal principal;
            try
            {
                principal = handler.ValidateToken(tokenString, validationParameters, out SecurityToken validatedToken);
            }
            catch
            {
                return Unauthorized();
            }

            var credentials = new SigningCredentials(_helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

            var newToken = new JwtSecurityToken(
                issuer: _helper.Issuer.Value,
                audience: _helper.Audience.Value,
                claims: principal.Claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            var newTokenString = handler.WriteToken(newToken);

            await Task.Yield(); // Simula async, opcional

            return Ok(new
            {
                response = newTokenString
            });
        }
    }
}