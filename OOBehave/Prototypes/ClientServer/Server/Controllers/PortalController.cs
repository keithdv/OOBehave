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
using OOBehave.Portal.Core;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortalController : ControllerBase
    {

        public PortalController(IServer server)
        {
            Server = server;
        }

        public IServiceScope Scope { get; }
        public IServer Server { get; }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public async Task<PortalResponse> Post(PortalRequest request)
        {

            var response = await Server.Handle(request).ConfigureAwait(false);

            return response;
        }

    }
}