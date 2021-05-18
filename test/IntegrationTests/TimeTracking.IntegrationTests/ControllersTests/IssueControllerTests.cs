using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TimeTracking.Common.Mappers;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Dal.Impl;
using TimeTracking.Entities;
using TimeTracking.Models;
using TimeTracking.Models.Requests;
using TimeTracking.Tests.Common;
using TimeTracking.Tests.Common.Data;
using TimeTracking.Tests.Common.Extensions;
using TimeTracking.WebApi;

namespace TimeTracking.IntegrationTests.ControllersTests
{
    [TestFixture]
    public class IssueControllerTests : RequestBase<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            Factory = new TimeTrackingWebApplicationFactory();
            base.SetUp();
            var token = GetJwtToken();
            AddAuth(token);
        }

        #region AssignIssueToUser

        [Test]
        public async Task AssignIssueToUser_WhenRequestIsValid_ReturnsSuccessStatus()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new AssignIssueToUserRequest()
            {
                IssueId = IssuesDbSet.Get().First().Id,
                UserId = UsersDbSet.Get().Last().Id
            };

            var httpResponse = await PostAsync(IssueControllerRoutes.AssignToUser, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();

            var issue = await GetIssueFromDatabase(request.IssueId);
            issue.AssignedToUserId.Should().Be(request.UserId);
            response.StatusCode.Should().Be(200);
            await ReSeedDatabase();
        }

        [Test]
        public async Task AssignIssueToUser_WhenIssueNotFound_ReturnsIssueNotFound()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new AssignIssueToUserRequest()
            {
                IssueId = Guid.NewGuid(),
                UserId = UsersDbSet.Get().Last().Id
            };

            var httpResponse = await PostAsync(IssueControllerRoutes.AssignToUser, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();

            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.IssueNotFound);
        }

        [Test]
        public async Task AssignIssueToUser_WhenUserNotFound_ReturnsUserNotFound()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new AssignIssueToUserRequest()
            {
                IssueId = IssuesDbSet.Get().First().Id,
                UserId = Guid.NewGuid()
            };

            var httpResponse = await PostAsync(IssueControllerRoutes.AssignToUser, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();

            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.UserNotFound);
        }

        [Test]
        public async Task AssignIssueToUser_WhenNotValidModelPassed_ReturnsValidationError()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            //issue id is empty
            var request = new AssignIssueToUserRequest()
            {
                UserId = Guid.NewGuid()
            };
            var httpResponse = await PostAsync(IssueControllerRoutes.AssignToUser, request);
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.CheckValidationException(1);

            //user id is empty
            request = new AssignIssueToUserRequest()
            {
                IssueId = Guid.NewGuid()
            };
            httpResponse = await PostAsync(IssueControllerRoutes.AssignToUser, request);
            response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.CheckValidationException(1);

            //user id and issue id is empty
            request = new AssignIssueToUserRequest();
            httpResponse = await PostAsync(IssueControllerRoutes.AssignToUser, request);
            response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.CheckValidationException(2);
        }

        #endregion

        #region GetIssueById

        [Test]
        public async Task GetIssueById_WhenFound_ReturnsIssue()
        {
            var expected = IssuesDbSet.Get().First();
            var issueId = expected.Id;

            var httpResponse = await GetAsync(IssueControllerRoutes.BaseRoute + "/" + issueId);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDetailsDto>>();

            response.VerifySuccessResponse();
            response.Data.Should().BeEquivalentTo(GetIssueDetails(expected));
        }

        [Test]
        public async Task GetIssueById_GetIssueByIdWhenNotFound_ReturnsIssueNotFound()
        {
            var issueId = Guid.NewGuid();

            var httpResponse = await GetAsync(IssueControllerRoutes.BaseRoute + "/" + issueId);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDetailsDto>>();

            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.IssueNotFound);
        }

        #endregion

        #region ChangeIssueStatus

        [Test]
        public async Task ChangeIssueStatus_WhenRequestIsValid_ReturnsSuccessStatus()
        {
            var request = new ChangeIssueStatusRequest()
            {
                IssueId = IssuesDbSet.Get().First().Id,
                Status = Status.Closed
            };

            var httpResponse = await PostAsync(IssueControllerRoutes.ChangeIssueStatus, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();

            var issue = await GetIssueFromDatabase(request.IssueId);
            issue.Status.Should().Be(request.Status);
            response.StatusCode.Should().Be(200);
            await ReSeedDatabase();
        }

        [Test]
        public async Task ChangeIssueStatus_WhenIssueNotFound_ReturnsIssueNotFound()
        {
            var request = new ChangeIssueStatusRequest()
            {
                IssueId = Guid.NewGuid(),
                Status = Status.Closed
            };

            var httpResponse = await PostAsync(IssueControllerRoutes.ChangeIssueStatus, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();

            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.IssueNotFound);
        }

        [Test]
        public async Task ChangeIssueStatus_WhenNotValidModelPassed_ReturnsValidationError()
        {
            //issue id is empty
            var request = new ChangeIssueStatusRequest()
            {
                Status = Status.Closed
            };
            var httpResponse = await PostAsync(IssueControllerRoutes.ChangeIssueStatus, request);
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.CheckValidationException(1);

            //status is empty
            request = new ChangeIssueStatusRequest()
            {
                IssueId = Guid.NewGuid()
            };
            httpResponse = await PostAsync(IssueControllerRoutes.ChangeIssueStatus, request);
            response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.CheckValidationException(1);

            //status and issue id is empty
            request = new ChangeIssueStatusRequest();
            httpResponse = await PostAsync(IssueControllerRoutes.ChangeIssueStatus, request);
            response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.CheckValidationException(2);
        }

        #endregion

        #region CreateIssue

        [Test]
        public async Task CreateIssue_WhenProjectNotFound_ReturnsProjectFound()
        {
            var claims = new Claim("roles", "ProjectManager");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new IssueDto()
            {
                ProjectId = Guid.NewGuid(),
                MilestoneId = null,
                Description = "description",
                Status = Status.Closed,
                AssignedToUserId = UsersDbSet.Get().First().Id,
                ReportedByUserId = UsersDbSet.Get().First().Id,
                Title = "title"
            };

            var httpResponse = await PostAsync(IssueControllerRoutes.CreateIssue, request);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.ProjectNotFound);
        }

        [Test]
        public async Task CreateIssue_WhenMilestoneNotFound_ReturnsMilestoneNotFound()
        {
            var claims = new Claim("roles", "ProjectManager");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new IssueDto()
            {
                ProjectId = ProjectsDbSet.Get().First().Id,
                MilestoneId = Guid.NewGuid(),
                Description = "description",
                Status = Status.Closed,
                AssignedToUserId = UsersDbSet.Get().First().Id,
                ReportedByUserId = UsersDbSet.Get().First().Id,
                Title = "title"
            };

            var httpResponse = await PostAsync(IssueControllerRoutes.CreateIssue, request);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.MileStoneNotFound);
        }


        [Test]
        public async Task CreateIssue_WhenModelValid_CreatesIssueInDbAndReturnSuccessResponse()
        {
            var claims = new Claim("roles", "ProjectManager");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var issuesCount = IssuesDbSet.Get().Count();
            var request = new IssueDto()
            {
                ProjectId = ProjectsDbSet.Get().First().Id,
                MilestoneId = null,
                Description = "description",
                Status = Status.Closed,
                AssignedToUserId = UsersDbSet.Get().First().Id,
                ReportedByUserId = UsersDbSet.Get().First().Id,
                Title = "title"
            };

            var httpResponse = await PostAsync(IssueControllerRoutes.CreateIssue, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.VerifySuccessResponse();
            await CheckIssueIsAddedToDatabase(response.Data, issuesCount + 1);
            response.VerifySuccessResponse();
            await ReSeedDatabase();
        }

        [Test]
        public async Task CreateIssue_WhenNotValidModelPassed_ReturnsValidationError()
        {
            var claims = new Claim("roles", "ProjectManager");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new IssueDto()
            {
                ProjectId = ProjectsDbSet.Get().First().Id,
                MilestoneId = null,
                AssignedToUserId = UsersDbSet.Get().First().Id,
                ReportedByUserId = UsersDbSet.Get().First().Id
            };
            var httpResponse = await PostAsync(IssueControllerRoutes.CreateIssue, request);
            var response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.CheckValidationException(4);

            request = new IssueDto()
            {
                ProjectId = ProjectsDbSet.Get().First().Id,
                MilestoneId = Guid.NewGuid(),
                Description = "description",
                Status = Status.Closed,
                Title = "title"
            };
            httpResponse = await PostAsync(IssueControllerRoutes.CreateIssue, request);
            response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.CheckValidationException(1);

            request = new IssueDto()
            {
                ProjectId = ProjectsDbSet.Get().First().Id,
                MilestoneId = Guid.NewGuid(),
                Status = Status.Closed
            };
            httpResponse = await PostAsync(IssueControllerRoutes.CreateIssue, request);
            response = await httpResponse.BodyAs<ApiResponse<IssueDto>>();
            response.CheckValidationException(5);
        }

        #endregion

        #region GetAllIssuesAsync

        [TestCase(1, 2, 1, 2)]
        [TestCase(1, 3, 1, 3)]
        public async Task GetAllIssuesAsync_WhenRequestValid_ReturnsAllIssues(int page, int size, int expectedPage,
            int expectedSize)
        {
            var expected = IssuesDbSet.Get().ToList();
            var pagedRequest = new PagedRequest()
            {
                Page = page,
                PageSize = size
            };
            var httpResponse = await GetAsync(IssueControllerRoutes.BaseRoute + "?" + pagedRequest.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<IssueDetailsDto>>();

            response.EnsurePagedResult(expected.Count, expectedSize, expectedPage);
        }

        [Test]
        public async Task GetAllIssuesAsync_WhenRequestValid_ReturnsValidIssues()
        {
            var pagedRequest = new PagedRequest()
            {
                Page = 1,
                PageSize = 2
            };
            var expected = IssuesDbSet.Get().Take(pagedRequest.PageSize).ToList();
            var mappedExpectedResult = expected.Select(GetIssueDetails).ToList();

            var httpResponse = await GetAsync(IssueControllerRoutes.BaseRoute + "?" + pagedRequest.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<IssueDetailsDto>>();

            response.Data.Should().BeEquivalentTo(mappedExpectedResult,
                opt => opt.Excluding(e => e.TotalRemainingTimeInSeconds)
                    .Excluding(e => e.TotalSpentTimeInSeconds));
        }

        #endregion

        #region helpers

        private async Task CheckIssueIsAddedToDatabase(IssueDto issue, int expectedCount)
        {
            var context = GetService<TimeTrackingDbContext>();
            var issueInDatabase = await context.Issues.OrderBy(e => e.CreatedAt).LastAsync();
            context.Issues.Should().HaveCount(expectedCount);
            issue.Should().BeEquivalentTo(issueInDatabase, opt =>
                opt.ExcludingMissingMembers().Excluding(e => e.Status));
            issue.Status.Should().Be(issue.Status);
        }

        private async Task<Issue> GetIssueFromDatabase(Guid issueId)
        {
            var context = GetService<TimeTrackingDbContext>();
            var issueInDatabase = await context.Issues.FindAsync(issueId);
            return issueInDatabase;
        }

        private IssueDto GetIssueDto(Issue issue)
        {
            var mapper = GetService<IBaseMapper<Issue, IssueDto>>();
            var issueDetailed = mapper.MapToModel(issue);
            return issueDetailed;
        }

        private IssueDetailsDto GetIssueDetails(Issue issue)
        {
            var mapper = GetService<IModelMapper<Issue, IssueDetailsDto>>();
            var issueDetailed = mapper.MapToModel(issue);
            issueDetailed.TotalRemainingTimeInSeconds = (long) (issue.ClosedAt - issue.OpenedAt).TotalSeconds;
            issueDetailed.TotalSpentTimeInSeconds = issue.WorkLogs.Sum(e => e.TimeSpent.Seconds);
            return issueDetailed;
        }

        #endregion
    }
}