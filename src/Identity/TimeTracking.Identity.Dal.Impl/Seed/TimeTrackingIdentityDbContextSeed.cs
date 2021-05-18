using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using TimeTracking.Contracts.Events;
using TimeTracking.Identity.Dal.Impl.Seed.Data;
using TimeTracking.Identity.Entities;

namespace TimeTracking.Identity.Dal.Impl.Seed
{
    public static class TimeTrackingIdentityDbContextSeed
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<Role> roleManager, IPublishEndpoint publish)
        {
            if (!roleManager.Roles.Any())
            {
                //Seed Roles
                await roleManager.CreateAsync(new Role(AuthorizationData.Roles.Engineer.ToString()));
                await roleManager.CreateAsync(new Role(AuthorizationData.Roles.TeamLead.ToString()));
                await roleManager.CreateAsync(new Role(AuthorizationData.Roles.ProjectManager.ToString()));
            }
            //Seed Default User
            var rolesNames = Enum.GetNames(typeof(AuthorizationData.Roles));
            foreach (var stringRole in rolesNames)
            {
                await EnsureUser(userManager, stringRole,publish);
           
            }
        }

        private static async Task EnsureUser(UserManager<User> userManager, string role,IPublishEndpoint publish)
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
                };
                await userManager.CreateAsync(user, AuthorizationData.DefaultPassword + role);
                await userManager.AddToRoleAsync(user, role);
                await publish.Publish<UserSignedUp>(new
                {
                    UserId =user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    __CorrelationId = InVar.Id,
                });
            }

        }

    }
}