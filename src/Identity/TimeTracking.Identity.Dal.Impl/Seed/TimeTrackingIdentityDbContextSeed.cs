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
        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<Role> roleManager,
            IPublishEndpoint publish)
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
            var result = rolesNames.Zip(UsersIds(), 
                    (x, y) => new Tuple<string, Guid>(x, y))
                .ToList();
            foreach (var (role, id) in result)
            {
                await EnsureUser(userManager, role,id, publish);
            }

            foreach (var user in UsersData.Users)
            {
                await AddUser(userManager, user, rolesNames[0]);
            }
        }

        public static async Task AddUser(UserManager<User> userManager, User userToAdd, string role)
        {
            if (await userManager.FindByNameAsync(userToAdd.UserName) == null)
            {
                userToAdd.PhoneNumberConfirmed = true;
                userToAdd.EmailConfirmed = true;

                await userManager.CreateAsync(userToAdd, AuthorizationData.DefaultPassword + role);
                await userManager.AddToRoleAsync(userToAdd, role);
            }
        }


        public static List<Guid> UsersIds ()
        {
            return new List<Guid>()
            {
                new Guid("45eb7068-bcfe-4b62-9b4a-474b453f96d2"),
                new Guid("b0c7a3c8-33c6-4d0f-816d-318c3e49c664"),
                new Guid("3245fb7a-7d89-4eac-a564-c87fa40d85ad"),
            };
        }

        private static async Task EnsureUser(UserManager<User> userManager, string role,Guid id, IPublishEndpoint publish)
        {
            var user = await userManager.FindByNameAsync(AuthorizationData.DefaultUsername + role);

            if (user == null)
            {
                user = new User()
                {
                    Id = id,
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