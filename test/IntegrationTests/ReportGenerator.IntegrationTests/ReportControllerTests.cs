using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using TimeTracking.IntegrationTests;
using TimeTracking.ReportGenerator.Models.Requests;
using TimeTracking.ReportGenerator.WebApi;
using TimeTracking.Tests.Common.Data;
using TimeTracking.Tests.Common.Extensions;

namespace ReportGenerator.IntegrationTests
{
    public class ReportControllerTests : RequestBase<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            Factory = new ReportGeneratorWebAppFactory();
            base.SetUp();
            var token = GetJwtToken();
            AddAuth(token);
        }

        [Test]
        public async Task GenerateReport_WhenValidParameters_GeneratesReportAsFile()
        {
            var request = new ReportGeneratorRequest()
            {
                ProjectId = ProjectsDbSet.Get().First().Id,
                UserId = UsersDbSet.Get().First().Id
            };

            var httpResponse = await GetAsync(ReportControllerRoutes.BaseRoute + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();

            httpResponse.Content.Should().NotBeNull();
        }
    }
}