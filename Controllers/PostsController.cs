using BoxBoxApi.DTOs;
using BoxBoxApi.Repositories;
using BoxBoxModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private RepositoryBoxBox repo;

        public PostsController (RepositoryBoxBox repo)
        {
            this.repo = repo;
        }

        // GET api/posts/get/{posicion}/{conversationId}
        /// <summary>
        /// Obtiene el conjunto de Posts por su posicion y Conversation, Tabla Posts.
        /// </summary>
        /// <remarks>
        /// Método para devolver los Posts paginados de una Conversation
        /// </remarks>
        /// <param name="posicion">Página de los posts</param>
        /// <param name="conversationId">Id del objeto Conversations para buscar sus posts</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        [HttpGet]
        [Route("[action]/{posicion}/{conversationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PostsPaginado>> Get(int posicion, int conversationId)
        {
            return await this.repo.GetPostsConversationAsync(posicion, conversationId);
        }

        // GET api/posts/{id}
        /// <summary>
        /// Obtiene un Post por su Id, tabla Posts.
        /// </summary>
        /// <remarks>
        /// Permite buscar un objeto Post por su ID
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto Post.</param>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Post>> Get(int id)
        {
            Post post = await this.repo.FindPostAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return post;
        }

        // POST api/posts
        /// <summary>
        /// Crea un nuevo Post en la BBDD, tabla Posts
        /// </summary>
        /// <remarks>
        /// Este método inserta un nuevo Post enviando el Objeto JSON
        /// El ID del post se genera automáticamente dentro del método
        /// </remarks>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post(CreatePostDto post)
        {
            await this.repo.CreatePostAsync(post);

            return Ok();
        }
        // PUT api/posts
        /// <summary>
        /// Modifica un Post en la BBDD mediante su ID, tabla Posts
        /// </summary>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(Post post)
        {
            var id = await this.repo.FindPostAsync(post.PostId);

            if (id == null)
            {
                return NotFound();
            }
            await this.repo.UpdatePostAsync(post);
            return Ok();
        }

        // DELETE api/posts/{id}
        /// <summary>
        /// Elimina un Post en la BBDD mediante su ID. Tabla Posts
        /// </summary>
        /// <remarks>
        /// Enviaremos el ID mediante la URL
        /// </remarks>
        /// <param name="id">ID del Post a eliminar</param>
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
            Post post = await this.repo.FindPostAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            await this.repo.DeletePostAsync(id);
            return Ok();
        }

        //GET api/posts/reported
        /// <summary>
        /// Obtiene el conjunto de posts reportados, tabla Posts.
        /// </summary>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        [HttpGet]
        [Route("[action]")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Post>>> GetReported()
        {
            return await this.repo.GetReportedPosts();
        }

        //PUT api/posts/report
        /// <summary>
        /// Cambia el estado del post a reportado por su ID, tabla Posts
        /// </summary>
        /// <param name="id">Id del Post a reportar</param>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPut]
        [Route("[action]/{idpost}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Report(int id)
        {
            Post post = await this.repo.FindPostAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            await this.repo.ReportPostAsync(id);
            return Ok();
        }

        //PUT api/posts/report
        /// <summary>
        /// Cambia el estado del post de reportado a normal por su ID, tabla Posts
        /// </summary>
        /// <param name="id">Id del Post</param>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">BBDD. No se ha creado el objeto en la BD. Error en la BBDD.</response>/// 
        [HttpPut]
        [Route("[action]/{idpost}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Unreport(int id)
        {
            Post post = await this.repo.FindPostAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            await this.repo.UnreportPostAsync(id);
            return Ok();
        }
    }
}
