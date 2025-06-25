using AutoMapper;
using Azure.Security.KeyVault.Secrets;
using BoxBoxApi.DTOs;
using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RepositoryBoxBox repo;
        private readonly SecretClient secretClient;
        private readonly KeyVaultSecret imagesContainer;
        private readonly IMapper _mapper;

        public UsersController(RepositoryBoxBox repo, SecretClient secretClient, IMapper mapper)
        {
            this.repo = repo;
            this.secretClient = secretClient;
            this.imagesContainer = this.secretClient.GetSecret("ImagesContainer");
            _mapper = mapper;
        }

        // GET: api/users
        /// <summary>
        /// Obtiene el conjunto de Users, tabla Users.
        /// </summary>
        /// <remarks>
        /// Método para devolver todos los Users de la BBDD.
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="403">Forbidden. El usuario no tiene permisos para acceder a esta lista.</response>
        [HttpGet]
        [Authorize(Roles = "1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<UserProfileDto>>> Get()
        {
            var users = await this.repo.GetUsersAsync();

            var userDtos = _mapper.Map<List<UserProfileDto>>(users);

            foreach (var dto in userDtos)
            {
                var user = users.First(u => u.UserId == dto.UserId);
                dto.ProfilePicture = this.imagesContainer.Value + "/" + user.ProfilePicture;
            }

            return Ok(userDtos);
        }

        // GET api/users/{id}
        /// <summary>
        /// Obtiene un User por su Id, tabla User.
        /// </summary>
        /// <remarks>
        /// Permite buscar un objeto User por su ID.
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto User.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDto>> Get(int id)
        {
            User user = await this.repo.FindUserAsync(id);
            if (user == null) return NotFound();

            int? currentUserId = null;

            if (User.Identity.IsAuthenticated)
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    currentUserId = int.Parse(claim.Value);
                }
            }

            var userProfile = _mapper.Map<UserProfileDto>(user);

            if (user.UserId != currentUserId)
                userProfile.Email = null;

            userProfile.ProfilePicture = this.imagesContainer.Value + "/" + user.ProfilePicture;

            return Ok(userProfile);
        }

        // POST api/users
        /// <summary>
        /// Crea un nuevo User en la BBDD, tabla Users.
        /// </summary>
        /// <remarks>
        /// Este método inserta un nuevo User enviando el Objeto JSON.
        /// El ID del user se genera automáticamente dentro del método.
        /// </remarks>
        /// <param name="username">Nombre de usuario del User.</param>
        /// <param name="email">Email del User.</param>
        /// <param name="password">Contraseña del User.</param>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response> 
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
        /// Modifica un User en la BBDD mediante su ID, tabla Users.
        /// </summary>
        /// <response code="200">OK. El objeto ha sido actualizado correctamente.</response>       
        /// <response code="403">Forbidden. El usuario no tiene permiso para modificar este usuario.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put([FromBody] UserRequestDto dto)
        {
            Claim claimUser = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonUser = claimUser.Value;
            User authenticatedUser = JsonConvert.DeserializeObject<User>(jsonUser);
            int authenticatedUserId = authenticatedUser.UserId;

            if (authenticatedUserId != dto.UserId)
            {
                return Forbid();
            }

            var user = await this.repo.FindUserAsync(dto.UserId);
            if (user == null) return NotFound();

            var originalProfilePicture = user.ProfilePicture;
            _mapper.Map(dto, user);

            if (dto.ProfilePicture == null)
            {
                user.ProfilePicture = originalProfilePicture;
            }

            await this.repo.UpdateUserAsync(user);

            return Ok();
        }

        // DELETE api/users/{id}
        /// <summary>
        /// Elimina un User en la BBDD mediante su ID. Tabla Users.
        /// </summary>
        /// <remarks>
        /// Enviaremos el ID mediante la URL.
        /// </remarks>
        /// <param name="id">ID del User a eliminar.</param>
        /// <response code="200">OK. Objeto eliminado en la BBDD.</response> 
        /// <response code="403">Forbidden. El usuario no tiene permisos suficientes para eliminar este usuario.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>    
        /// <response code="500">BBDD. No se ha eliminado el objeto en la BD. Error en la BBDD.</response> 
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        [ProducesResponseType(StatusCodes.Status200OK)]  
        [ProducesResponseType(StatusCodes.Status403Forbidden)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <response code="401">Unauthorized. No autorizado, sin Token válido.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>         
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDto>> Profile()
        {
            Claim claimUser = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonUser = claimUser.Value;
            User user = JsonConvert.DeserializeObject<User>(jsonUser);
            int idUser = user.UserId;

            User userValid = await this.repo.FindUserAsync(idUser);
            if (userValid == null) return NotFound();

            var userProfile = _mapper.Map<UserProfileDto>(userValid);
            userProfile.ProfilePicture = this.imagesContainer.Value + "/" + userValid.ProfilePicture;

            return Ok(userProfile);
        }
    }
}
