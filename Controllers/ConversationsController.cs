using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private RepositoryBoxBox repo;

        public ConversationsController(RepositoryBoxBox repo)
        {
            this.repo = repo;
        }

        // GET api/conversations/get/{posicion}/{topicId}
        /// <summary>
        /// Obtiene el conjunto de VConversations por su posicion y Topic, view V_Conversations.
        /// </summary>
        /// <remarks>
        /// Método para devolver los VConversations paginados del Topic
        /// </remarks>
        /// <param name="posicion">Página de las conversations</param>
        /// <param name="topicId">Id del objeto Topic para buscar sus conversations</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        [HttpGet]
        [Route("[action]/{posicion}/{topicId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ConversationsPaginado>> Get(int posicion, int topicId)
        {
            return await this.repo.GetVConversationsTopicAsync(posicion, topicId);
        }

        // GET api/conversations/{id}
        /// <summary>
        /// Obtiene una Conversation por su Id, tabla Conversations.
        /// </summary>
        /// <remarks>
        /// Permite buscar un objeto Conversation por su ID
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto Conversation.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Conversation>> Get(int id)
        {
            Conversation conversation = await this.repo.FindConversationAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }
            return conversation;
        }

        // POST api/conversations
        /// <summary>
        /// Crea una nueva Conversation en la BBDD, tabla Conversations
        /// </summary>
        /// <remarks>
        /// Este método inserta una nueva Conversation enviando el Objeto JSON
        /// El ID de la conversation se genera automáticamente dentro del método
        /// </remarks>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Conversation>> Post(Conversation conversation)
        {
            Conversation conver = await this.repo.CreateConversationAsync(conversation);

            return conver;
        }
        // PUT api/conversations
        /// <summary>
        /// Modifica una Conversation en la BBDD mediante su ID, tabla Conversations
        /// </summary>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(Conversation conversation)
        {
            var id = await this.repo.FindConversationAsync(conversation.ConversationId);

            if (id == null)
            {
                return NotFound();
            }
            await this.repo.UpdateConversationAsync(conversation);
            return Ok();
        }

        // DELETE api/conversations/{id}
        /// <summary>
        /// Elimina una Conversation en la BBDD mediante su ID. Tabla Conversations
        /// </summary>
        /// <remarks>
        /// Enviaremos el ID mediante la URL
        /// </remarks>
        /// <param name="id">ID de la Conversation a eliminar</param>
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
            Conversation conversation = await this.repo.FindConversationAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }
            await this.repo.DeleteConversationAsync(id);
            return Ok();
        }

        // PUT api/conversations/updateEntryCount
        /// <summary>
        /// Actualiza el contador de visualizaciones de una Conversation (EntryCount) mediante su ID, tabla Conversations
        /// </summary>
        /// <remarks>
        /// Enviaremos el ID mediante la URL
        /// </remarks>
        /// <param name="id">ID de la Conversation</param>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>///
        [HttpPut]
        [Route("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateEntryCount(int id)
        {
            Conversation conversation = await this.repo.FindConversationAsync(id);

            if (conversation == null)
            {
                return NotFound();
            }
            await this.repo.UpdateEntryCount(id);
            return Ok();
        }
    }
}
