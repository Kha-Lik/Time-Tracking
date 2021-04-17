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
    public class UserControllerTests : RequestBase<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            Factory = new TimeTrackingWebApplicationFactory();
            base.SetUp();
            var token = GetJwtToken();
            AddAuth(token);
        }


        #region GetUserById

        [Test]
        public async Task GetUserById_WhenFound_ReturnsTeam()
        {
            var expected = UsersDbSet.Get().First();
            var userId = expected.Id;

            var httpResponse = await GetAsync(UserControllerRoutes.BaseRoute + "/" + userId);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TimeTrackingUserDetailsDto>>();

            response.VerifySuccessResponse();
            response.Data.Should().BeEquivalentTo(GetUserDetails(expected));
        }

        [Test]
        public async Task GetUserById_WhenNotFound_ReturnsUserNotFound()
        {
            var userId = Guid.NewGuid();

            var httpResponse = await GetAsync(UserControllerRoutes.BaseRoute + "/" + userId);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TimeTrackingUserDetailsDto>>();
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.UserNotFound);
        }

        #endregion

        #region AddUserToTeam

        [Test]
        public async Task AddUserToTeam_WhenTeamNotFound_ReturnTeamNotFound()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new AssignUserToTeamRequest()
            {
                TeamId = Guid.NewGuid(),
                UserId = UsersDbSet.Get().First().Id
            };

            var httpResponse = await PostAsync(UserControllerRoutes.AddUserToTeam, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TimeTrackingUserDetailsDto>>();

            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.TeamNotFound);
        }

        [Test]
        public async Task AddUserToTeam_WhenUserNotFound_ReturnTeamNotFound()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new AssignUserToTeamRequest()
            {
                TeamId = TeamsDbSet.Get().First().Id,
                UserId = Guid.NewGuid()
            };

            var httpResponse = await PostAsync(UserControllerRoutes.AddUserToTeam, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TimeTrackingUserDetailsDto>>();

            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.UserNotFound);
        }

        [Test]
        public async Task AddUserToTeam_WhenRequestNotValid_ReturnValidationErrors()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new AssignUserToTeamRequest()
            {
                TeamId = Guid.Empty,
                UserId = Guid.Empty
            };

            var httpResponse = await PostAsync(UserControllerRoutes.AddUserToTeam, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TimeTrackingUserDetailsDto>>();
            response.CheckValidationException(2);

            //team id is empty
            request = new AssignUserToTeamRequest()
            {
                UserId = Guid.NewGuid()
            };
            httpResponse = await PostAsync(UserControllerRoutes.AddUserToTeam, request);
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse<TimeTrackingUserDetailsDto>>();
            response.CheckValidationException(1);

            //user id is empty
            request = new AssignUserToTeamRequest()
            {
                TeamId = Guid.NewGuid()
            };
            httpResponse = await PostAsync(UserControllerRoutes.AddUserToTeam, request);
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse<TimeTrackingUserDetailsDto>>();
            response.CheckValidationException(1);
        }

        [Test]
        public async Task AddUserToTeam_WhenRequestValid_ReturnValidationTimeTrackingUser()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var request = new AssignUserToTeamRequest()
            {
                TeamId = TeamsDbSet.Get().First().Id,
                UserId = UsersDbSet.Get().First().Id
            };

            var httpResponse = await PostAsync(UserControllerRoutes.AddUserToTeam, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TimeTrackingUserDetailsDto>>();

            response.VerifySuccessResponse();
            var user = await GetUserFromDatabase(request.UserId);
            user.TeamId.Should().Be(request.TeamId);
            await ReSeedDatabase();
        }

        #endregion

        #region helpers

        private TimeTrackingUserDetailsDto GetUserDetails(TimeTrackingUser user)
        {
            var mapper = GetService<IModelMapper<TimeTrackingUser, TimeTrackingUserDetailsDto>>();
            var model = mapper.MapToModel(user);
            return model;
        }

        private async Task CheckUserAddedToDatabase(TimeTrackingUserDto user, int expectedCount)
        {
            var context = GetService<TimeTrackingDbContext>();
            var userInDatabase = await context.Users.LastAsync();
            context.Users.Should().HaveCount(expectedCount);
            user.Should().BeEquivalentTo(userInDatabase, opt => opt.ExcludingMissingMembers());
        }

        private async Task<TimeTrackingUser> GetUserFromDatabase(Guid userId)
        {
            var context = GetService<TimeTrackingDbContext>();
            var userFromDatabase = await context.Users.FindAsync(userId);
            return userFromDatabase;
        }

        #endregion

        #region GetAllUsers

        [TestCase(1, 2, 1, 2)]
        [TestCase(1, 3, 1, 3)]
        public async Task GetAllUsers_WhenRequestValid_ReturnsValidPageResult(int page, int size, int expectedPage,
            int expectedSize)
        {
            var expected = UsersDbSet.Get().ToList();
            var pagedRequest = new PagedRequest()
            {
                Page = page,
                PageSize = size
            };
            var httpResponse = await GetAsync(UserControllerRoutes.AllUsers + "?" + pagedRequest.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<TimeTrackingUserDetailsDto>>();

            response.EnsurePagedResult(expected.Count, expectedSize, expectedPage);
        }

        [Test]
        public async Task GetAllUsers_WhenRequestValid_ReturnsValidUsers()
        {
            var pagedRequest = new PagedRequest()
            {
                Page = 1,
                PageSize = 2
            };
            var expected = UsersDbSet.Get().Take(pagedRequest.PageSize).ToList();
            var mappedExpectedResult = expected.Select(GetUserDetails).ToList();

            var httpResponse = await GetAsync(UserControllerRoutes.AllUsers + "?" + pagedRequest.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<TimeTrackingUserDetailsDto>>();

            response.Data.Should().BeEquivalentTo(mappedExpectedResult,
                opt => opt.ExcludingMissingMembers());
        }

        #endregion
    }
}