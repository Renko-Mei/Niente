using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace neu.Controllers
{
    [Route("api/")]
    public class WelcomeController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public string Get()
        {
            return "Welcome to neu API";
        }
    }
}
