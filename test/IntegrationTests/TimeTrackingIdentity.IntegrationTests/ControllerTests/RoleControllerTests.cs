using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.Dal.Impl;
using TimeTracking.Identity.Models.Requests;
using TimeTracking.Identity.WebApi;
using TimeTracking.IntegrationTests;
using TimeTracking.Tests.Common.Extensions;


namespace TimeTrackingIdentity.IntegrationTests.ControllerTests
{
    public class RoleControllerTests : RequestBase<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            Factory = new IdentityWebApplicationFactory();
            base.SetUp();
            var token = GetJwtToken();
            AddAuth(token);
        }

        #region AddToRoleAsync

        [Test]
        public async Task AddUserToRoleAsync_WhenUserNotFoundById_ReturnsNoAccountsRegisteredWithCurrentEmail()
        {
            var request = new AddToRoleRequest
            {
                UserId = Guid.NewGuid(),
                RoleName = "role"
            };

            var httpResponse = await PostAsync(RoleControllerRoutes.AddToRole, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.ResponseException!.ErrorMessage.Should().Be($"No Accounts Registered with id {request.UserId}.");
            response.ResponseException.ErrorCode.Should().Be(ErrorCode.UserNotFound);
        }

        [Test]
        public async Task AddUserToRoleAsync_WhenNoSuchRoleExistsAsync_ReturnsNoSuchRoleExistsAsync()
        {
            var context = GetService<TimeTrackingIdentityDbContext>();
            var userInDatabase = await context.Users.LastAsync();
            var request = new AddToRoleRequest
            {
                UserId = userInDatabase.Id,
                RoleName = "role"
            };

            var httpResponse = await PostAsync(RoleControllerRoutes.AddToRole, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.RoleNotFound);
            response.ResponseException!.ErrorMessage.Should().Be($"Role {request.RoleName} not found.");
        }

        [Test]
        public async Task AddUserToRoleAsync_WhenRequestValid_ReturnsSuccessResponse()
        {
            var context = GetService<TimeTrackingIdentityDbContext>();
            var userInDatabase = await context.Users.LastAsync();
            var request = new AddToRoleRequest
            {
                UserId = userInDatabase.Id,
                RoleName = "TeamLead"
            };

            var httpResponse = await PostAsync(RoleControllerRoutes.AddToRole, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.StatusCode.Should().Be(200);
            response.IsSuccess.Should().BeTrue();
        }

        #endregion
    }
}