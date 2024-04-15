using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private RepositoryBoxBox repo;

        public DriversController(RepositoryBoxBox repo)
        {
            this.repo = repo;
        }

        // GET: api/drivers
        /// <summary>
        /// Obtiene el conjunto de Drivers, tabla Drivers.
        /// </summary>
        /// <remarks>
        /// Método para devolver todos los Drivers de la BBDD
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Driver>>> Get()
        {
            return await this.repo.GetDriversAsync();
        }

        // GET: api/drivers/{id}
        /// <summary>
        /// Obtiene un Driver por su Id, tabla Drivers.
        /// </summary>
        /// <remarks>
        /// Permite buscar un objeto Driver por su ID
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto Driver.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Driver>> Get(int id)
        {
            var driver = await this.repo.FindDriverAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return driver;
        }

        // POST: api/drivers
        /// <summary>
        /// Crea un nuevo Driver en la BBDD, tabla Drivers
        /// </summary>
        /// <remarks>
        /// Este método inserta un nuevo Driver enviando el Objeto JSON
        /// El ID del driver se genera automáticamente dentro del método
        /// </remarks>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Driver>> Post(Driver driver)
        {
            await this.repo.CreateDriverAsync(driver);

            return Ok();
        }
        // PUT api/drivers/5
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(Driver driver)
        {
            var id = await this.repo.FindDriverAsync(driver.DriverID);

            if (id == null)
            {
                return NotFound();
            }
            await this.repo.UpdateDriverAsync(driver);
            return Ok();
        }

        // DELETE api/<DriversController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
        }
    }
}
