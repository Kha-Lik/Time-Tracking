using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TimeTracking.Common.Enums;
using TimeTracking.Common.Helpers;
using TimeTracking.Common.Jwt;
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
    public class WorkLogControllerTests : RequestBase<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            Factory = new TimeTrackingWebApplicationFactory();
            base.SetUp();
            var claims = new List<Claim>()
            {
                new Claim(Constants.Strings.JwtClaimIdentifiers.Id, Guid.NewGuid().ToString())
            };
            var token = GetJwtToken(claims);
            AddAuth(token);
        }

        #region CreateWorkLog

        [Test]
        public async Task CreateWorkLog_WhenIssueNotFound_ReturnsIssueNotFound()
        {
            var request = new WorkLogDto()
            {
                Description = "description",
                StartDate = DateTimeOffset.Now,
                IssueId = Guid.NewGuid(),
                ActivityType = ActivityType.Research,
                TimeSpent = TimeSpan.FromDays(2)
            };

            var httpResponse = await PostAsync(WorkLogControllerRoutes.CreateWorkLog, request);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.IssueNotFound);
        }

        [Test]
        public async Task CreateWorkLog_WhenModelValid_CreatesWorkLogInDbAndReturnSuccessResponse()
        {
            var workLogCount = WorklogsDbSet.Get().Count();
            var request = new WorkLogDto()
            {
                Description = "description",
                StartDate = DateTimeOffset.Now,
                IssueId = IssuesDbSet.Get().First().Id,
                ActivityType = ActivityType.Research,
                TimeSpent = TimeSpan.FromDays(2)
            };

            var httpResponse = await PostAsync(WorkLogControllerRoutes.CreateWorkLog, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.VerifySuccessResponse();
            await CheckWorkLogAddedToDatabase(response.Data, workLogCount + 1);
            response.VerifySuccessResponse();
            await ReSeedDatabase();
        }

        [Test]
        public async Task CreateWorkLog_WhenNotValidModelPassed_ReturnsValidationError()
        {
            //issue id not valid
            var request = new WorkLogDto()
            {
                Description = "description",
                StartDate = DateTimeOffset.Now,
                ActivityType = ActivityType.Research,
                TimeSpent = TimeSpan.FromDays(2)
            };
            var httpResponse = await PostAsync(WorkLogControllerRoutes.CreateWorkLog, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.CheckValidationException(1);

            request = new WorkLogDto()
            {
                StartDate = DateTimeOffset.Now,
                ActivityType = ActivityType.Research,
                TimeSpent = TimeSpan.FromDays(2)
            };
            httpResponse = await PostAsync(WorkLogControllerRoutes.CreateWorkLog, request);
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.CheckValidationException(3);

            request = new WorkLogDto()
            {
                StartDate = DateTimeOffset.Now,
                ActivityType = ActivityType.Research
            };
            httpResponse = await PostAsync(WorkLogControllerRoutes.CreateWorkLog, request);
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.CheckValidationException(4);
        }

        #endregion

        #region helpers

        private WorkLogDetailsDto GetWorkLogDetails(WorkLog workLog)
        {
            var mapper = GetService<IModelMapper<WorkLog, WorkLogDetailsDto>>();
            var model = mapper.MapToModel(workLog);
            return model;
        }

        private WorkLogDto GetWorkLogDto(WorkLog workLog)
        {
            var mapper = GetService<IBaseMapper<WorkLog, WorkLogDto>>();
            var model = mapper.MapToModel(workLog);
            return model;
        }

        private async Task CheckWorkLogAddedToDatabase(WorkLogDto workLog, int expectedCount)
        {
            var context = GetService<TimeTrackingDbContext>();
            var workLogInDatabase = await context.WorkLogs.LastAsync();
            context.WorkLogs.Should().HaveCount(expectedCount);
            workLog.Should().BeEquivalentTo(workLogInDatabase, opt => opt.ExcludingMissingMembers());
        }

        private async Task<WorkLog> GetWorkLogFromDatabase(Guid workLogId)
        {
            var context = GetService<TimeTrackingDbContext>();
            var workLogFromDatabase = await context.WorkLogs.FindAsync(workLogId);
            return workLogFromDatabase;
        }

        #endregion

        #region GetActivitiesForUser

        [Test]
        public async Task GetActivitiesForUser_WhenModelNotValid_ReturnsValidationErrors()
        {
            //project id not set
            var request = new ActivitiesRequest()
            {
                ProjectId = Guid.Empty,
                UserId = Guid.NewGuid()
            };
            var httpResponse = await GetAsync(WorkLogControllerRoutes.GetUserWorkLogs + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<UserActivityDto>>();
            response.CheckValidationException(1);

            //user id not set
            request = new ActivitiesRequest()
            {
                ProjectId = Guid.NewGuid(),
                UserId = Guid.Empty
            };
            httpResponse = await GetAsync(WorkLogControllerRoutes.GetUserWorkLogs + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse<UserActivityDto>>();
            response.CheckValidationException(1);

            //empty request
            request = new ActivitiesRequest()
            {
            };
            httpResponse = await GetAsync(WorkLogControllerRoutes.GetUserWorkLogs + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse<UserActivityDto>>();
            response.CheckValidationException(2);
        }


        [Test]
        public async Task GetActivitiesForUser_WhenUserNotFound_ReturnsUserNotFoundResponse()
        {
            var request = new ActivitiesRequest()
            {
                ProjectId = ProjectsDbSet.Get().First().Id,
                UserId = Guid.NewGuid()
            };

            var httpResponse = await GetAsync(WorkLogControllerRoutes.GetUserWorkLogs + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<UserActivityDto>>();

            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.UserNotFound);
        }

        [Test]
        public async Task GetActivitiesForUser_WhenModelValid_ReturnsUserActivities()
        {
            var user = UsersDbSet.Get().First();
            var request = new ActivitiesRequest()
            {
                ProjectId = ProjectsDbSet.Get().First().Id,
                UserId = user.Id
            };
            var workLogItems = WorklogsDbSet.Get().Where(e => e.UserId.Equals(request.UserId))
                .Include(e => e.Issue)
                .ThenInclude(e => e.Project)
                .Where(e => e.Issue.ProjectId.Equals(request.ProjectId));
            var httpResponse = await GetAsync(WorkLogControllerRoutes.GetUserWorkLogs + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<UserActivityDto>>();

            response.VerifySuccessResponse();
            response.Data.UserId.Should().Be(request.UserId);
            response.Data.UserName.Should().Be(user.FirstName);
            response.Data.UserSurname.Should().Be(user.LastName);
            response.Data.UserEmail.Should().Be(user.Email);
            response.Data.ProjectName.Should().Be(workLogItems?.Select(e => e.Issue.Project.Name).FirstOrDefault());
            response.Data.TotalWorkLogInSeconds.Should().Be((long?) workLogItems?.Sum(e => e.TimeSpent.TotalSeconds));
            response.Data.WorkLogItems.Should()
                .BeEquivalentTo(workLogItems?.Select(e => GetWorkLogDetails(e)).ToList());
            await ReSeedDatabase();
        }

        #endregion

        #region UpdateWorkLog

        [Test]
        public async Task UpdateWorkLog_WhenWorkLogNotFound_ReturnsWorkLogNotFound()
        {
            var request = new WorkLogUpdateRequest()
            {
                Description = "description",
                ActivityType = ActivityType.Coding,
                StartDate = DateTimeOffset.MaxValue.AddDays(-3333),
                TimeSpent = TimeSpan.FromDays(2),
                WorkLogId = Guid.NewGuid()
            };

            var httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLog, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();

            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.WorkLogNotFound);
        }

        [Test]
        public async Task UpdateWorkLog_WhenModelValid_UpdatesWorkLogInDatabase()
        {
            var request = new WorkLogUpdateRequest()
            {
                Description = "description",
                ActivityType = ActivityType.Coding,
                StartDate = DateTimeOffset.MaxValue.AddDays(-3333),
                TimeSpent = TimeSpan.FromDays(2),
                WorkLogId = WorklogsDbSet.Get().First().Id
            };

            var httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLog, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();

            response.VerifySuccessResponse();
            var worklog = await GetWorkLogFromDatabase(request.WorkLogId);
            worklog.Description.Should().Be(request.Description);
            worklog.ActivityType.Should().Be(request.ActivityType);
            worklog.StartDate.Should().Be(request.StartDate);
            worklog.TimeSpent.Should().Be(request.TimeSpent);
            await ReSeedDatabase();
        }


        [Test]
        public async Task UpdateWorkLog_WhenModelNotValid_UpdatesWorkLogInDatabase()
        {
            var request = new WorkLogUpdateRequest()
            {
                ActivityType = ActivityType.Coding,
                StartDate = DateTimeOffset.MaxValue.AddDays(-3333),
                TimeSpent = TimeSpan.FromDays(2),
                WorkLogId = WorklogsDbSet.Get().First().Id
            };

            var httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLog, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.CheckValidationException(2);

            request = new WorkLogUpdateRequest()
            {
                ActivityType = ActivityType.Coding,
                TimeSpent = TimeSpan.FromDays(2),
                WorkLogId = WorklogsDbSet.Get().First().Id
            };

            httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLog, request);
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.CheckValidationException(3);

            request = new WorkLogUpdateRequest()
            {
                ActivityType = ActivityType.Coding,
                WorkLogId = WorklogsDbSet.Get().First().Id
            };

            httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLog, request);
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.CheckValidationException(4);
        }

        #endregion

        #region GetWorkLog

        [Test]
        public async Task GetWorkLog_WhenFound_ReturnsWorkLog()
        {
            var expected = WorklogsDbSet.Get().First();
            var workLogId = expected.Id;

            var httpResponse = await GetAsync(WorkLogControllerRoutes.BaseRoute + "/" + workLogId);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDetailsDto>>();
            response.VerifySuccessResponse();
            response.Data.Should().BeEquivalentTo(GetWorkLogDetails(expected),
                opt => opt.ExcludingMissingMembers()
                    .Excluding(e => e.WorkLogId)
                    .Excluding(e => e.UserId));
        }

        [Test]
        public async Task GetWorkLog_WhenNotFound_ReturnsWorkLogNotFound()
        {
            var workLogId = Guid.NewGuid();

            var httpResponse = await GetAsync(WorkLogControllerRoutes.BaseRoute + "/" + workLogId);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDetailsDto>>();
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.WorkLogNotFound);
        }

        #endregion

        #region GetAllWorkLogsPaged

        [TestCase(1, 2, 1, 2)]
        [TestCase(1, 3, 1, 3)]
        public async Task GetAllWorkLogsPaged_WhenRequestValid_ReturnsAllWorkLogs(int page, int size, int expectedPage,
            int expectedSize)
        {
            var expected = WorklogsDbSet.Get().ToList();
            var pagedRequest = new PagedRequest()
            {
                Page = page,
                PageSize = size
            };
            var httpResponse = await GetAsync(WorkLogControllerRoutes.BaseRoute + "?" + pagedRequest.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<WorkLogDetailsDto>>();

            response.EnsurePagedResult(expected.Count, expectedSize, expectedPage);
        }

        [Test]
        public async Task GetAllProjects_WhenRequestValid_ReturnsValidProjects()
        {
            var pagedRequest = new PagedRequest()
            {
                Page = 1,
                PageSize = 6
            };
            var expected = WorklogsDbSet.Get().Take(pagedRequest.PageSize).ToList();
            var mappedExpectedResult = expected.Select(GetWorkLogDto).ToList();

            var httpResponse = await GetAsync(WorkLogControllerRoutes.BaseRoute + "?" + pagedRequest.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<WorkLogDto>>();

            response.Data.Should().BeEquivalentTo(mappedExpectedResult);
        }

        #endregion

        #region UpdateWorkLogStatus

        [Test]
        public async Task UpdateWorkLogStatus_WhenNotFound_ReturnsWorkLogNotFound()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new UpdateWorkLogStatusRequest
            {
                Description = "desc",
                IsApproved = true,
                WorkLogId = Guid.NewGuid()
            };

            var httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLogStatus, request);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.WorkLogNotFound);
        }

        [Test]
        public async Task UpdateWorkLogStatus_WhenModelValidFound_UpdatesWorkLog()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new UpdateWorkLogStatusRequest
            {
                Description = "desc",
                IsApproved = false,
                WorkLogId = WorklogsDbSet.Get().First().Id
            };

            var httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLogStatus, request);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();

            response.VerifySuccessResponse();
            var workLog = await GetWorkLogFromDatabase(request.WorkLogId);
            workLog.IsApproved.Should().Be(request.IsApproved);
            await ReSeedDatabase();
        }

        [Test]
        public async Task UpdateWorkLogStatus_WhenModelNotValid_ReturnsValidationErrors()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            //description not set 
            var request = new UpdateWorkLogStatusRequest
            {
                IsApproved = false,
                WorkLogId = WorklogsDbSet.Get().First().Id
            };
            var httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLogStatus, request);
            var response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.CheckValidationException(2);

            //description and work log id not set 
            request = new UpdateWorkLogStatusRequest
            {
                IsApproved = false
            };
            httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLogStatus, request);
            response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.CheckValidationException(3);

            //empty request
            request = new UpdateWorkLogStatusRequest();
            httpResponse = await PostAsync(WorkLogControllerRoutes.UpdateWorkLogStatus, request);
            response = await httpResponse.BodyAs<ApiResponse<WorkLogDto>>();
            response.CheckValidationException(3);
        }

        #endregion

        #region DeleteWorklog

        [Test]
        public async Task DeleteWorklog_WhenNotFound_ReturnsWorkLogNotFound()
        {
            var workLogId = Guid.NewGuid();

            var httpResponse = await DeleteAsync(WorkLogControllerRoutes.BaseRoute + "/" + workLogId);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.StatusCode.Should().Be(400);
            response.ResponseException.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.WorkLogNotFound);
            response.ResponseException.ErrorMessage.Should().NotBeNull(ErrorCode.WorkLogNotFound.GetDescription());
        }

        [Test]
        public async Task DeleteWorklog_WhenFound_DeletesWorkLogFromDatabase()
        {
            var expected = WorklogsDbSet.Get().First();
            var workLogId = expected.Id;

            var httpResponse = await DeleteAsync(WorkLogControllerRoutes.BaseRoute + "/" + workLogId);

            var response = await httpResponse.BodyAs<ApiResponse>();

            response.StatusCode.Should().Be(200);
            var workLog = await GetWorkLogFromDatabase(workLogId);
            workLog.Should().BeNull();
            await ReSeedDatabase();
        }

        #endregion
    }
}