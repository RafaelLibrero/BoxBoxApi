using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private RepositoryBoxBox repo;

        public TeamsController(RepositoryBoxBox repo)
        {
            this.repo = repo;
        }

        // GET api/teams
        /// <summary>
        /// Obtiene el conjunto de Teams, tabla Teams.
        /// </summary>
        /// <remarks>
        /// Método para devolver todos los Teams de la BBDD
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Team>>> Get()
        {
            return await this.repo.GetTeamsAsync();
        }

        // GET api/teams/{id}
        /// <summary>
        /// Obtiene un Team por su Id, tabla Teams.
        /// </summary>
        /// <remarks>
        /// Permite buscar un objeto Team por su ID
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto Team.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Team>> Get(int id)
        {
            Team team = await this.repo.FindTeamAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return team;
        }

        // POST api/teams
        /// <summary>
        /// Crea un nuevo Team en la BBDD, tabla Teams
        /// </summary>
        /// <remarks>
        /// Este método inserta un nuevo Team enviando el Objeto JSON
        /// El ID del team se genera automáticamente dentro del método
        /// </remarks>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post(Team team)
        {
            await this.repo.CreateTeamAsync(team);

            return Ok();
        }
        // PUT api/teams
        /// <summary>
        /// Modifica un Team en la BBDD mediante su ID, tabla Teams
        /// </summary>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(Team team)
        {
            var id = await this.repo.FindTeamAsync(team.TeamId);

            if (id == null)
            {
                return NotFound();
            }
            await this.repo.UpdateTeamAsync(team);
            return Ok();
        }

        // DELETE api/teams/{id}
        /// <summary>
        /// Elimina un Team en la BBDD mediante su ID. Tabla Teams
        /// </summary>
        /// <remarks>
        /// Enviaremos el ID mediante la URL
        /// </remarks>
        /// <param name="id">ID del Team a eliminar</param>
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
            Team team = await this.repo.FindTeamAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            await this.repo.DeleteTeamAsync(id);
            return Ok();
        }
    }
}
