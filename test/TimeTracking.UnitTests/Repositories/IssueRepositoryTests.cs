using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMockHelper.Core;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using TimeTracking.Dal.Impl;
using TimeTracking.Dal.Impl.Repositories;
using TimeTracking.Entities;
using TimeTracking.UnitTests.Data;

namespace TimeTracking.UnitTests.Repositories
{
    [TestFixture]
    public class IssueRepositoryTests
    {
        private DbContextOptions<TimeTrackingDbContext> dbOptions;

        [SetUp]
        public void SetUp()
        {
            this.dbOptions = DbOptionsFactory.GetTimeTrackingDbOptions();
        }
        
        //https://github.com/markjsc/AutoMockHelper/blob/bed7c63ecbd22989f25e165548758ac48e97f37a/AutoMockHelper.Samples.NUnit/OrderProcessorTests.cs
        [TestCase("EC3CB528-45E3-4ABA-8E6E-DB40D0C0A400")]
        [TestCase("BB25D21B-2CD9-4EA3-A82F-1E9EB669E6FA")]
        public async Task HistoryRepository_GetById(string id)
        {
            var guidId = Guid.Parse(id);
            await using var context = new TimeTrackingDbContext(dbOptions);
            var expected =  IssuesDbSet.Get().First(x => x.Id == guidId);
            var issueRepository = new IssueRepository(context);

            var issue = await issueRepository.GetByIdAsync(guidId);

            issue.Should().BeEquivalentTo(expected);
        }
        
    }
}