using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OOBehave;
using OOBehave.Portal;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortalController : ControllerBase
    {

        public PortalController(IServiceScope scope)
        {
            Scope = scope;
        }

        public IServiceScope Scope { get; }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public async Task<PortalResponse> Post(PortalRequest request)
        {

            var server = (IServer) Scope.Resolve(typeof(Server<>).MakeGenericType(request.ObjectType));

            var response = await server.Handle(request).ConfigureAwait(false);

            return response;
        }

    }
}