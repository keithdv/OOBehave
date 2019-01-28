using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave
{
    public class OOBehaveConfiguration : IOOBehaveConfiguration
    {

        public OOBehaveConfiguration(IConfiguration configuration)
        {
            var section = configuration.GetSection("OOBehave");
            PortalURL = section["PortalURL"];
        }

        public string PortalURL { get; set; }

    }
}
