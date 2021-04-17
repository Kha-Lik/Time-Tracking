using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TimeTracking.Dal.Impl.Seeds.Data;
using TimeTracking.Identity.Dal.Impl.Seed.Data;
using TimeTracking.Identity.Entities;

namespace TimeTrackingIdentity.IntegrationTests
{
    public static class DbSeedHelper
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                //Seed Roles
                await roleManager.CreateAsync(new Role(DbSeedData.AuthorizationData.Roles.Engineer.ToString()));
                await roleManager.CreateAsync(new Role(DbSeedData.AuthorizationData.Roles.TeamLead.ToString()));
                await roleManager.CreateAsync(new Role(AuthorizationData.Roles.ProjectManager.ToString()));
            }

            //Seed Default User
            var rolesNames = Enum.GetNames(typeof(AuthorizationData.Roles));
            foreach (var stringRole in rolesNames) await EnsureUser(userManager, stringRole);

            foreach (var user in Users) await userManager.CreateAsync(user);
        }


        private static List<User> Users =>
            new List<User>()
            {
                new User()
                {
                    UserName = "notConfirmedEmail@email.com",
                    Email = "notConfirmedEmail@email.com",
                    EmailConfirmed = false
                },
                new User()
                {
                    UserName = "lockoutEmail@email.com",
                    Email = "lockoutEmail@email.com",
                    EmailConfirmed = true,
                    LockoutEnd = DateTimeOffset.MaxValue
                }
            };

        private static async Task EnsureUser(UserManager<User> userManager, string role)
        {
            var user = await userManager.FindByNameAsync(AuthorizationData.DefaultUsername + role);

            if (user == null)
            {
                user = new User()
                {
                    UserName = AuthorizationData.DefaultUsername + role,
                    FirstName = AuthorizationData.DefaultUsername + role,
                    LastName = AuthorizationData.DefaultUsername + role,
                    Email = AuthorizationData.DefaultEmail + role,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    RefreshTokens = new List<RefreshToken>()
                    {
                        new RefreshToken()
                        {
                            Created = DateTimeOffset.Now,
                            Expires = DateTimeOffset.MaxValue,
                            Revoked = null,
                            Token = "testToken"
                        }
                    }
                };
                await userManager.CreateAsync(user, AuthorizationData.DefaultPassword + role);
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}