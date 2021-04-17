using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Dsl;
using TimeTracking.Entities;

namespace TimeTracking.Tests.Common.Data
{
    public class UsersDbSet
    {
        private static Fixture Fixture = new Fixture();

        private static IQueryable<TimeTrackingUser> _userDbSet;

        public static IQueryable<TimeTrackingUser> Get()
        {
            return _userDbSet ??= new List<TimeTrackingUser>()
            {
                TimeTrackingUserBuilder()
                    .With(i => i.Id, new Guid("29C2F600-4F76-4753-A54D-A422DEF8EB9E"))
                    .With(e=>e.TeamId,new Guid("40B40CF5-46E1-4F03-8968-FA1F5DA459B3"))
                    .Create(),
                TimeTrackingUserBuilder()
                    .With(i => i.Id, new Guid("57C10EE7-4108-4FD9-BCE9-5477FE8BFF9B"))
                    .With(e=>e.TeamId,new Guid("40B40CF5-46E1-4F03-8968-FA1F5DA459B3"))
                    .Create(),
                TimeTrackingUserBuilder()
                    .With(i => i.Id, new Guid("403FD7F2-027B-42B6-875A-72555E999D61"))
                    .With(e=>e.TeamId,new Guid("40B40CF5-46E1-4F03-8968-FA1F5DA459B3"))
                    .Create(),
                TimeTrackingUserBuilder()
                    .With(i => i.Id, new Guid("44A345F7-DD98-4CA3-B193-CEB4D4904864"))
                    .With(e=>e.TeamId,new Guid("9245950B-0F82-4B9E-9AF7-DEC9ACF171FA"))
                    .Create(),
            }.AsQueryable();
        }

        public static IPostprocessComposer<TimeTrackingUser> TimeTrackingUserBuilder()
        {
            return Fixture.Build<TimeTrackingUser>()
                .Without(x => x.Team)
                .Without(x => x.CreatedMilestones)
                .Without(x => x.ReportedIssues)
                .Without(x => x.WorkLogs)
                .Without(x => x.AssignedIssues);
        }
    }
}