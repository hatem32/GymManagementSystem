using GymManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();

                if (HasUsers && HasRoles) return;
                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new IdentityRole(){Name = "SuperAdmin"},
                        new IdentityRole(){Name = "Admin"}
                    };

                    foreach (var roleName in Roles.Select(R => R.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(roleName!))
                        {
                            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName!));
                            if (!roleResult.Succeeded)
                                logger.LogError("Failed to create role {Role}: {Errors}", roleName,
                                    string.Join("; ", roleResult.Errors.Select(e => e.Description)));
                        }
                    }
                }
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Hatem",
                        LastName = "Hussein",
                        UserName = "HatemHussein",
                        Email = "HatemHussein@gmail.com",
                        PhoneNumber = "01152840092"
                    };

                    await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");

                    var Admin01 = new ApplicationUser()
                    {
                        FirstName = "Omar",
                        LastName = "Mohamed",
                        UserName = "OmarMohamed",
                        Email = "OmarMohamed@gmail.com",
                        PhoneNumber = "01232589652"
                    };

                    var createResult = await userManager.CreateAsync(Admin01, "P@ssw0rd");
                    await userManager.AddToRoleAsync(Admin01, "Admin");
                    if (!createResult.Succeeded)
                    {
                        logger.LogError("Failed to create seed Admin: {Errors}", string.Join("; ", createResult.Errors.Select(e => e.Description)));
                        return;
                    }
                    logger.LogInformation($"Seeded Admin {Admin01.Email}");

                }
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Identity seeding failed.");
                throw;
            }
        }

    }
}
