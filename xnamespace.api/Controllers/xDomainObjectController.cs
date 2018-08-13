using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace xnamespace.Controllers
{
    [Route("/api/[controller]")]
    public class xDomainObject : Controller
    {
        [HttpPost]
        [Route("/api/xDomainObject")]
        [SwaggerOperation("xDomainObjectPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(xDomainObject), description: "Success")]
        [SwaggerResponse(statusCode: 500, type: typeof(void), description: "InternalServerError")]
        public virtual async Task<IActionResult> xDomainObjectPost()
        {
            return null;
        }
    }
}