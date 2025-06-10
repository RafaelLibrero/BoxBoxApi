using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly RepositoryBoxBox repo;

        public TopicsController(RepositoryBoxBox repo)
        {
            this.repo = repo;
        }

        // GET api/topics
        /// <summary>
        /// Obtiene el conjunto de VTopics, view V_Topics.
        /// </summary>
        /// <remarks>
        /// Método para devolver todos los VTopics de la BBDD.
        /// </remarks>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<VTopic>>> Get()
        {
            return await this.repo.GetVTopicsAsync();
        }

        // GET api/topics/{id}
        /// <summary>
        /// Obtiene un Topic por su Id, tabla Topics.
        /// </summary>
        /// <remarks>
        /// Permite buscar un objeto Topic por su ID.
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto Topic.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Topic>> Get(int id)
        {
            Topic topic = await this.repo.FindTopicAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return topic;
        }

        // POST api/topics
        /// <summary>
        /// Crea un nuevo Topic en la BBDD, tabla Topics.
        /// </summary>
        /// <remarks>
        /// Este método inserta un nuevo Topic enviando el Objeto JSON.
        /// El ID del topic se genera automáticamente dentro del método.
        /// </remarks>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="403">Forbidden. El usuario no tiene permisos para modificar este topic.</response> 
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>
        [HttpPost]
        [Authorize(Roles = "1")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post(Topic topic)
        {
            await this.repo.CreateTopicAsync(topic);
            return CreatedAtAction(nameof(Get), new { id = topic.TopicId }, topic);
        }

        // PUT api/topics
        /// <summary>
        /// Modifica un Topic en la BBDD mediante su ID, tabla Topic.
        /// </summary>
        /// <response code="200">OK. El objeto ha sido actualizado correctamente.</response>
        /// <response code="403">Forbidden. El usuario no tiene permisos para crear un topic.</response> 
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>
        [HttpPut]
        [Authorize(Roles = "1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(Topic topic)
        {
            var id = await this.repo.FindTopicAsync(topic.TopicId);

            if (id == null)
            {
                return NotFound();
            }
            await this.repo.UpdateTopicAsync(topic);
            return Ok();
        }

        // DELETE api/topics/{id}
        /// <summary>
        /// Elimina un Topic en la BBDD mediante su ID. Tabla Topics.
        /// </summary>
        /// <remarks>
        /// Enviaremos el ID mediante la URL.
        /// </remarks>
        /// <param name="id">ID del Topic a eliminar.</param>
        /// <response code="200">OK. Objeto eliminado en la BBDD.</response> 
        /// <response code="403">Forbidden. El usuario no tiene permisos para eliminar este topic.</response>
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
            Topic topic = await this.repo.FindTopicAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            await this.repo.DeleteTopicAsync(id);
            return Ok();
        }
    }
}
