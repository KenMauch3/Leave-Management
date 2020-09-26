using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leave_Management
{
    public static class SeedData
    {
        public static void Seed(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if(userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new IdentityUser("admin@localhost.com")
                {
                    Email = "admin@localhost.com"
                };
                var result = userManager.CreateAsync(user,"P@ssword1").Result;
                if (result.Succeeded)
                {
                    var result2 = userManager.AddToRoleAsync(user, "Administrator").Result;
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            List<string> roles = new List<string> { "Administrator", "Employee" };
            foreach (var roleName in roles)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    var role = new IdentityRole(roleName);
                    var result = roleManager.CreateAsync(role).Result;
                }
            }
        }
    }
}
