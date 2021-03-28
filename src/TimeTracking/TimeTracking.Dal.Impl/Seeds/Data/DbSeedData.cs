using System;
using System.Collections.Generic;
using System.Linq;
using TimeTracking.Entities;
using ListExtensions = FluentEmail.Core.ListExtensions;

namespace TimeTracking.Dal.Impl.Seeds.Data
{
    public static class DbSeedData
    {
        #region TimeTrackingUsers

        public static List<TimeTrackingUser> TimeTrackingUserData()
        {
            return GeneratePredefinedUsers();
        }

        private static List<TimeTrackingUser> GeneratePredefinedUsers()
        {

            var list = new List<TimeTrackingUser>();
            var rolesNames = Enum.GetNames(typeof(AuthorizationData.Roles)).ToList();
            rolesNames?.ForEach(role =>
            {
                list.Add(new TimeTrackingUser()
                {
                    FirstName = AuthorizationData.DefaultUsername + role,
                    LastName = AuthorizationData.DefaultUsername + role,
                    Email = AuthorizationData.DefaultEmail + role,
                });
            });

            return list;
        }

        public static class AuthorizationData
        {
            public enum Roles
            {
                Engineer,
                ProjectManager,
                TeamLead,
            }
            public const string DefaultUsername = "user";
            public const string DefaultEmail = "user@secureapi.com";
            public const string DefaultPassword = "Pa$$w0rd_";
            public const Roles DefaultRole = Roles.Engineer;
        }

        #endregion
    }
}