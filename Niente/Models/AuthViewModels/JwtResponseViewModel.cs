using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Niente.Models.AuthViewModels
{
    public class JwtResponseViewModel
    {
        public string Token { get; set; }
        public string User { get; set; }
        public string Role { get; set; }
        public DateTime Expires { get; set; }
    }
}
