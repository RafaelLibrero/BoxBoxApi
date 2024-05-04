using Azure.Security.KeyVault.Secrets;
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
        private SecretClient secretClient;
        private KeyVaultSecret imagesContainer;

        public DriversController(RepositoryBoxBox repo, SecretClient secretClient)
        {
            this.repo = repo;
            this.secretClient = secretClient;
            this.imagesContainer =
                this.secretClient.GetSecret("ImagesContainer");
        }

        // GET api/drivers
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
            List<Driver> drivers = await this.repo.GetDriversAsync();
            foreach(Driver driver in drivers)
            {
                driver.Flag = this.imagesContainer.Value + "/" + driver.Flag;
                driver.Imagen = this.imagesContainer.Value + "/" + driver.Imagen;
            }
            return drivers;
        }

        // GET api/drivers/{id}
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
            Driver driver = await this.repo.FindDriverAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            driver.Flag = this.imagesContainer.Value + "/" + driver.Flag;
            driver.Imagen = this.imagesContainer.Value + "/" + driver.Imagen;
            return driver;
        }

        // POST api/drivers
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
        public async Task<ActionResult> Post(Driver driver)
        {
            await this.repo.CreateDriverAsync(driver);

            return Ok();
        }
        // PUT api/drivers
        /// <summary>
        /// Modifica un Driver en la BBDD mediante su ID, tabla Drivers
        /// </summary>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPut]
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

        // DELETE api/drivers/{id}
        /// <summary>
        /// Elimina un Driver en la BBDD mediante su ID. Tabla Drivers
        /// </summary>
        /// <remarks>
        /// Enviaremos el ID mediante la URL
        /// </remarks>
        /// <param name="id">ID del Driver a eliminar</param>
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
            Driver driver = await this.repo.FindDriverAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            await this.repo.DeleteDriverAsync(id);
            return Ok();
        }
    }
}
