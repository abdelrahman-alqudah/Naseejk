using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Naseejk.core.Entities;
using System;
using System.Threading.Tasks;

namespace Naseejk.Infrastructure.Data
{
    public static class DatabaseSeederExtension
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            try
            {
                var userManager = scopedServices.GetRequiredService<UserManager<User>>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<Role>>();
                var context = scopedServices.GetRequiredService<NaseejkDbContext>();

                var seeder = new NaseejkDataSeeder(roleManager, userManager, context);

                await seeder.SeedAllAsync();
            }
            catch (Exception ex)
            {
                var logger = scopedServices.GetRequiredService<ILogger<NaseejkDataSeeder>>();
                logger.LogError(ex, "Error: An error occurred while seeding the database.");
            }
        }
    }
}