using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Naseejk.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naseejk.Infrastructure.Data
{
    public class NaseejkDataSeeder
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private readonly NaseejkDbContext context;

        public NaseejkDataSeeder(RoleManager<Role> roleManager, UserManager<User> userManager, NaseejkDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.context = context;
        }

        public async Task SeedAllAsync()
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
        }

        private async Task SeedAdminUserAsync()
        {
            string adminEmail = "abdalrhman@naseejkdev.onmicrosoft.com";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin == null)
            {
                var defaultTenant = await context.Tenants.FirstOrDefaultAsync(t => t.Name == "Naseejk Tenant");
                var adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    TenantId = defaultTenant!.Id,
                };
                var result = await userManager.CreateAsync(adminUser, "Adminin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
                    Console.WriteLine("Admin user seeded successfully.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error seeding admin user: {error.Description}");
                    }
                }
            }
        }

        private async Task SeedRolesAsync()
        {
            var roles = new List<(int Id, string Name)>
            {
                (1, "SuperAdmin"),
                (2, "TenantAdmin"),
                (3, "User")
            };

            var defaultApp = await context.Applications.FirstOrDefaultAsync(a => a.Code == "Naseejk_Hub_Admin");
            if (defaultApp == null)
            {
                Console.WriteLine("[ERROR] Cannot seed roles because no Application exists in DB!");
                return;
            }

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    var identityRole = new Role
                    {
                        Id = role.Id,
                        Name = role.Name,
                        NormalizedName = role.Name.ToUpper(),
                        ApplicationId = defaultApp.Id 
                    };

                    var result = await roleManager.CreateAsync(identityRole);

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"[SUCCESS] Role '{role.Name}' seeded successfully with Id {role.Id}.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"[FAILED TO SAVE ROLE] {role.Name} -> Error: {error.Description}");
                        }
                    }
                }
            }
        }
    }
}