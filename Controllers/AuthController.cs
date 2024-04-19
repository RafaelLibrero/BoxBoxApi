using BoxBoxApi.Helpers;
using BoxBoxApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxBoxApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryBoxBox repo;
        private HelperToken helper;

        public AuthController(RepositoryBoxBox repo)
        {
            this.repo = repo;
        }


    }
}
