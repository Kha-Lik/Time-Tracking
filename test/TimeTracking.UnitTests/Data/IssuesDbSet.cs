using System;
using System.Collections.Generic;
using AutoFixture;
using TimeTracking.Entities;

namespace TimeTracking.UnitTests.Data
{
    public static class IssuesDbSet
    {
        private static Fixture Fixture = new Fixture();

        private static List<Issue> _issuesDbSet;

        public static ICollection<Issue> Get()
        {
            if (_issuesDbSet == null)
            {
                _issuesDbSet = new List<Issue>()
                {
                    Fixture.Build<Issue>()
                        .With(i => i.Id, new Guid("EC3CB528-45E3-4ABA-8E6E-DB40D0C0A400"))
                        .Without(x => x.Milestone)
                        .Without(x => x.Project)
                        .Without(x => x.WorkLogs)
                        .Without(e => e.TimeTrackingUserAssigned)
                        .Without(w => w.TimeTrackingUserReporter)
                        .Create(),
                    Fixture.Build<Issue>()
                        .With(i => i.Id, new Guid("BB25D21B-2CD9-4EA3-A82F-1E9EB669E6FA"))
                        .Without(x => x.Milestone)
                        .Without(x => x.Project)
                        .Without(x => x.WorkLogs)
                        .Without(e => e.TimeTrackingUserAssigned)
                        .Without(w => w.TimeTrackingUserReporter)
                        .Create(),
                };
            }

            return _issuesDbSet;
        }
    }
}