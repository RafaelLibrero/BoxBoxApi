using Azure.Security.KeyVault.Secrets;
using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private RepositoryBoxBox repo;
        private SecretClient secretClient;
        private KeyVaultSecret imagesContainer;

        public UsersController(RepositoryBoxBox repo, SecretClient secretClient)
        {
            this.repo = repo;
            this.secretClient = secretClient;
            this.imagesContainer =
                this.secretClient.GetSecret("ImagesContainer");
        }

        // GET: api/users
        /// <summary>
        /// Obtiene el conjunto de Users, tabla Users.
        /// </summary>
        /// <remarks>
        /// Método para devolver todos las Users de la BBDD
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<User>>> Get()
        {
            List<User> users = await this.repo.GetUsersAsync();
            foreach(User user in users) 
            {
                user.ProfilePicture = this.imagesContainer.Value + "/" + user.ProfilePicture;
            }
            return await this.repo.GetUsersAsync();
        }

        // GET api/users/{id}
        /// <summary>
        /// Obtiene un User por su Id, tabla User.
        /// </summary>
        /// <remarks>
        /// Permite buscar un objeto User por su ID
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto User.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Get(int id)
        {
            User user = await this.repo.FindUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.ProfilePicture = this.imagesContainer.Value + "/" + user.ProfilePicture;
            return user;
        }

        // POST api/users
        /// <summary>
        /// Crea un nuevo User en la BBDD, tabla Users
        /// </summary>
        /// <remarks>
        /// Este método inserta un nuevo User enviando el Objeto JSON
        /// El ID del user se genera automáticamente dentro del método
        /// </remarks>
        /// <param name="username">Nombre de usuario del User</param>
        /// <param name="email">Email del User</param>
        /// <param name="password">Contraseña del User</param>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> Post(string username, string email, string password)
        {
            User user = await this.repo.Register(username, email, password);
            return user;
        }

        // PUT api/users
        /// <summary>
        /// Modifica un Users en la BBDD mediante su ID, tabla Users
        /// </summary>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(User user)
        {
            var id = await this.repo.FindUserAsync(user.UserId);

            if (id == null)
            {
                return NotFound();
            }
            await this.repo.UpdateUserAsync(user);
            return Ok();
        }

        // DELETE api/users/{id}
        /// <summary>
        /// Elimina un User en la BBDD mediante su ID. Tabla Users
        /// </summary>
        /// <remarks>
        /// Enviaremos el ID mediante la URL
        /// </remarks>
        /// <param name="id">ID del User a eliminar</param>
        /// <response code="201">Deleted. Objeto eliminado en la BBDD.</response> 
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>    
        /// <response code="500">BBDD. No se ha eliminado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            User user = await this.repo.FindUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await this.repo.DeleteUserAsync(id);
            return Ok();
        }

        // GET: api/usuarios/perfilusuario
        /// <summary>
        /// Obtiene un User a partir de su TOKEN, tabla Users.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>        
        /// <response code="401">NotAuthorized. No autorizado, sin Token válido.</response>         
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<User> Profile()
        {
            Claim claimUser = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonUser = claimUser.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonUser);
            int idUser = user.UserId;
            User userValid = await this.repo.FindUserAsync(idUser);
            userValid.ProfilePicture = this.imagesContainer.Value + "/" + userValid.ProfilePicture;
            return userValid;
        }

    }
}
