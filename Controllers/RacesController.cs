using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacesController : ControllerBase
    {
        private RepositoryBoxBox repo;

        public RacesController(RepositoryBoxBox repo)
        {
            this.repo = repo;
        }

        // GET api/races
        /// <summary>
        /// Obtiene el conjunto de Races, tabla Races.
        /// </summary>
        /// <remarks>
        /// Método para devolver todos las Races de la BBDD
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Race>>> Get()
        {
            return await this.repo.GetRacesAsync();
        }

        // GET api/races/{id}
        /// <summary>
        /// Obtiene una Race por su Id, tabla Race.
        /// </summary>
        /// <remarks>
        /// Permite buscar un objeto Race por su ID
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto Race.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Race>> Get(int id)
        {
            Race race = await this.repo.FindRaceAsync(id);
            if (race == null)
            {
                return NotFound();
            }
            return race;
        }

        // POST api/races
        /// <summary>
        /// Crea una nueva Race en la BBDD, tabla Races
        /// </summary>
        /// <remarks>
        /// Este método inserta una nueva Race enviando el Objeto JSON
        /// El ID de la Race se genera automáticamente dentro del método
        /// </remarks>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post(Race race)
        {
            await this.repo.CreateRaceAsync(race);
            return Ok();
        }

        // PUT api/races
        /// <summary>
        /// Modifica una Race en la BBDD mediante su ID, tabla Races
        /// </summary>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(Race race)
        {
            var id = await this.repo.FindRaceAsync(race.RaceId);

            if (id == null)
            {
                return NotFound();
            }
            await this.repo.UpdateRaceAsync(race);
            return Ok();
        }

        // DELETE api/races/{id}
        /// <summary>
        /// Elimina una Race en la BBDD mediante su ID. Tabla Races
        /// </summary>
        /// <remarks>
        /// Enviaremos el ID mediante la URL
        /// </remarks>
        /// <param name="id">ID de la Race a eliminar</param>
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
            Race race = await this.repo.FindRaceAsync(id);
            if (race == null)
            {
                return NotFound();
            }
            await this.repo.DeleteRaceAsync(id);
            return Ok();
        }
    }
}
