using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TimeTracking.Common.Helpers;
using TimeTracking.Common.Mappers;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.Dal.Impl;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.Models.Dtos;
using TimeTracking.Identity.WebApi;
using TimeTracking.IntegrationTests;
using TimeTracking.Tests.Common.Extensions;

namespace TimeTrackingIdentity.IntegrationTests.ControllerTests
{
    public class UserControllerTests : RequestBase<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            Factory = new IdentityWebApplicationFactory();
            base.SetUp();
            var token = GetJwtToken();
            AddAuth(token);
        }

        #region GetAll

        [Test]
        public async Task GetAll_WhenRequested_ShouldReturnAllUsersFromDatabase()
        {
            var context = GetService<TimeTrackingIdentityDbContext>();
            var userInDatabase = await context.Users.ToListAsync();

            var httpResponse = await GetAsync(UserControllerRoutes.AllUsers);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<List<UserDto>>>();

            response.Data.Should().HaveCount(userInDatabase.Count);
        }

        #endregion

        #region MyRegion

        [Test]
        public async Task GetUserById_WhenUserNotFound_ShouldReturnNotFoundResponse()
        {
            var httpResponse = await GetAsync(UserControllerRoutes.BaseRoute + "/" + Guid.NewGuid());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<UserDto>>();

            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.UserNotFound);
            response.ResponseException!.ErrorMessage.Should().Be(ErrorCode.UserNotFound.GetDescription());
        }

        [Test]
        public async Task GetUserById_WhenUserFound_ShouldReturnUsersFromDatabase()
        {
            var context = GetService<TimeTrackingIdentityDbContext>();
            var userInDatabase = await context.Users.LastAsync();

            var httpResponse = await GetAsync(UserControllerRoutes.BaseRoute + "/" + userInDatabase.Id);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<UserDto>>();

            response.Data.Should().BeEquivalentTo(GetUserDetails(userInDatabase));
        }

        #endregion

        #region helpers

        private UserDto GetUserDetails(User user)
        {
            var mapper = GetService<IBaseMapper<User, UserDto>>();
            var model = mapper.MapToModel(user);
            return model;
        }

        #endregion
    }
}