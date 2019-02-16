using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace neu.Models
{
    public class ApplicationUser : IdentityUser
    {
        public enum UserRole
        {
            SuperAdministrator,
            User
        }
    }
}
