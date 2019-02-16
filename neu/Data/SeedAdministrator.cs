using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using neu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace neu.Data
{
    public static class SeedAdministrator
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // User Info
            string userName = "RenkoMei";
            string email = "superadmin@renkomei.com";
            string password = "osu_Player_94";
            string role = "SuperAdministrator";

            if (await userManager.FindByNameAsync(userName) == null)
            {
                // Create SuperAdmins role if it doesn't exist
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Create user account if it doesn't exist
                ApplicationUser user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, password);

                // Assign role to the user
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
