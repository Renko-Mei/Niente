using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Niente.Controllers
{
    [Route("api/")]
    public class WelcomeController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IHostingEnvironment HostingEnvironment;

        public WelcomeController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        // GET: api/<controller>
        [HttpGet]
        public string Get()
        {
            string welcome = "Welcome to Niente API\n" +
                             $"The environment is {HostingEnvironment.EnvironmentName}\n" +
                             $"The secret is {Configuration["Bangumi:ClientSecret"]}";
            return welcome;
        }
    }
}
