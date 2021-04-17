using System;
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
using TimeTracking.Identity.Models.Requests;
using TimeTracking.Identity.Models.Responses;
using TimeTracking.Identity.WebApi;
using TimeTracking.IntegrationTests;
using TimeTracking.Tests.Common.Extensions;


namespace TimeTrackingIdentity.IntegrationTests.ControllerTests
{
    [TestFixture]
    public class AuthControllerTests : RequestBase<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            Factory = new IdentityWebApplicationFactory();
            base.SetUp();
            var token = GetJwtToken();
            AddAuth(token);
        }

        #region Register

        [Test]
        public async Task Register_WhenPasswordNotValid_ReturnsUserRegistrationFailed()
        {
            var request = new RegistrationRequest()
            {
                Username = "userName",
                Password = "passwordNotValid",
                FirstName = "name",
                LastName = "last name",
                Email = "email@email.com"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.Register, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.UserRegistrationFailed);
        }

        [Test]
        public async Task Register_WhenModelNotValid_ReturnsUserRegistrationFailed()
        {
            var request = new RegistrationRequest()
            {
                Password = "qWerty_21",
                FirstName = "name",
                LastName = "last name",
                Email = "email",
                Username = "userName"
            };
            var httpResponse = await PostAsync(AuthControllerRoutes.Register, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();
            response.CheckValidationException(1);

            request = new RegistrationRequest();
            httpResponse = await PostAsync(AuthControllerRoutes.Register, request);
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse>();
            response.CheckValidationException(10);

            request = new RegistrationRequest()
            {
                Username = "userName",
                FirstName = "name",
                LastName = "last name",
                Email = "email"
            };
            httpResponse = await PostAsync(AuthControllerRoutes.Register, request);
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse>();
            response.CheckValidationException(3);
        }


        [Test]
        public async Task Register_WhenValidRequest_CreatesUserInDatabase()
        {
            var request = new RegistrationRequest()
            {
                Username = "userName",
                Password = "qWerty_21",
                FirstName = "name",
                LastName = "last name",
                Email = "email@email.com"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.Register, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            await CheckUserAddedToDatabase(request, 6);
            response.StatusCode.Should().Be(200);
            await ReSeedDatabase();
        }

        #endregion

        #region helpers

        private UserDto GetUserDetails(User user)
        {
            var mapper = GetService<IBaseMapper<User, UserDto>>();
            var model = mapper.MapToModel(user);
            return model;
        }

        private async Task CheckUserAddedToDatabase(RegistrationRequest request, int expectedCount)
        {
            var context = GetService<TimeTrackingIdentityDbContext>();
            var userInDatabase = await context.Users.LastAsync();
            context.Users.Should().HaveCount(expectedCount);
            userInDatabase.Email.Should().Be(request.Email);
            userInDatabase.UserName.Should().Be(request.Username);
            userInDatabase.FirstName.Should().Be(request.FirstName);
            userInDatabase.LastName.Should().Be(request.LastName);
        }

        private async Task<User> GetUserFromDatabase(Guid userId)
        {
            var context = GetService<TimeTrackingIdentityDbContext>();
            var userFromDatabase = await context.Users.FindAsync(userId);
            return userFromDatabase;
        }

        #endregion

        #region ConfirmEmail

        [Test]
        public async Task ConfirmEmail_WhenUserNotFound_ReturnsUserNotFoundResponses()
        {
            var request = new EmailConfirmationRequest()
            {
                Code = "emailCode"
            };
            var httpResponse = await GetAsync(AuthControllerRoutes.ConfirmEmail + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();
            response.CheckValidationException(1);

            request = new EmailConfirmationRequest()
            {
                Code = "emailCode"
            };
            httpResponse = await GetAsync(AuthControllerRoutes.ConfirmEmail + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse>();
            response.CheckValidationException(1);

            request = new EmailConfirmationRequest();
            httpResponse = await GetAsync(AuthControllerRoutes.ConfirmEmail + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            response = await httpResponse.BodyAs<ApiResponse>();
            response.CheckValidationException(3);
        }

        [Test]
        public async Task ConfirmEmail_WhenCodeNotValid_ReturnsUserNotFoundResponses()
        {
            var context = GetService<TimeTrackingIdentityDbContext>();
            var userFromDatabase = await context.Users.FirstOrDefaultAsync();
            var request = new EmailConfirmationRequest()
            {
                UserId = userFromDatabase.Id,
                Code = "emailCode"
            };

            var httpResponse = await GetAsync(AuthControllerRoutes.ConfirmEmail + "?" + request.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.EmailConfirmationFailed);
            response.ResponseException!.ErrorMessage.Should().Be(ErrorCode.EmailConfirmationFailed.GetDescription());
        }

        #endregion

        #region CreateTokenAsync

        [Test]
        public async Task CreateTokenAsync_WhenCodeNotValid_ReturnsNoAccountsRegisteredWithCurrentEmail()
        {
            var request = new TokenExchangeRequest()
            {
                Password = "password",
                Email = "exapmleNotFoundUser@emal.com"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.TokenExchange, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<AuthResponse>();

            response.Message.Should().Be($"No Accounts Registered with {request.Email}.");
        }

        [Test]
        public async Task CreateTokenAsync_WhenEmailNotConfirmed_ReturnsNoAccountsRegisteredWithCurrentEmail()
        {
            var request = new TokenExchangeRequest()
            {
                Password = "password",
                Email = "notConfirmedEmail@email.com"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.TokenExchange, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<AuthResponse>();

            response.Message.Should().Be($"Current email {request.Email} is not confirmed.");
        }

        [Test]
        public async Task CreateTokenAsync_WhenUserIsLockedOut_ReturnsAccountLockOut()
        {
            var request = new TokenExchangeRequest()
            {
                Password = "password",
                Email = "lockoutEmail@email.com"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.TokenExchange, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<AuthResponse>();

            response.Message.Should().Be($"This account has been locked.");
        }

        [Test]
        public async Task CreateTokenAsync_WhenUserWithNotValidPassword_ReturnsIncorrectCredentials()
        {
            var request = new TokenExchangeRequest()
            {
                Password = "password",
                Email = "user@secureapi.comEngineer"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.TokenExchange, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<AuthResponse>();

            response.Message.Should().Be($"Incorrect Credentials for user {request.Email}.");
        }

        #endregion

        #region ForgotPassword

        [Test]
        public async Task ForgotPasswordAsync_WhenUserNotFoundByEmail_ReturnsEmailConfirmationFailed()
        {
            var request = new ForgotPasswordRequest()
            {
                Email = "exapmleNotFoundUser@email.com"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.ForgotPassword, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.EmailConfirmationFailed);
            response.ResponseException!.ErrorMessage.Should().Be(ErrorCode.EmailConfirmationFailed.GetDescription());
        }

        [Test]
        public async Task ForgotPasswordAsync_WhenUserEmailNotConfirmed_ReturnsEmailConfirmationFailed()
        {
            var request = new ForgotPasswordRequest()
            {
                Email = "exapmleNotFoundUser@email.com"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.ForgotPassword, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.EmailConfirmationFailed);
            response.ResponseException!.ErrorMessage.Should().Be(ErrorCode.EmailConfirmationFailed.GetDescription());
        }

        [Test]
        public async Task ForgotPasswordAsync_WhenRequestIsValid_ReturnsSuccessResponse()
        {
            var request = new ForgotPasswordRequest()
            {
                Email = "user@secureapi.comEngineer"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.ForgotPassword, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.StatusCode.Should().Be(200);
            response.IsSuccess.Should().BeTrue();
        }

        #endregion

        #region ResetPassword

        [Test]
        public async Task ResetPasswordAsync_WhenUserNotFoundById_ReturnsUserNotFound()
        {
            var request = new ResetPasswordRequest()
            {
                UserId = Guid.NewGuid(),
                Password = "PASSWORD",
                Code = "code"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.ResetPassword, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.UserNotFound);
            response.ResponseException!.ErrorMessage.Should().Be(ErrorCode.UserNotFound.GetDescription());
        }

        [Test]
        public async Task ResetPasswordAsync_WhenCodeNotValid_ReturnsResetPasswordFailed()
        {
            var context = GetService<TimeTrackingIdentityDbContext>();
            var userFromDatabase = await context.Users.FirstOrDefaultAsync();
            var request = new ResetPasswordRequest()
            {
                UserId = userFromDatabase.Id,
                Password = "PASSWORD",
                Code = "code"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.ResetPassword, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.ResetPasswordFailed);
            response.ResponseException!.ErrorMessage.Should().Be(ErrorCode.ResetPasswordFailed.GetDescription());
        }

        #endregion

        #region ResendVerificationEmail

        [Test]
        public async Task ResendVerificationEmail_WhenUserNotFoundById_ReturnsUserNotFound()
        {
            var request = new ResendEmailRequest()
            {
                Email = "exapmleNotFoundUser@email.com"
            };
            var httpResponse = await PostAsync(AuthControllerRoutes.ResendEmailVerificationCode, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.UserNotFound);
            response.ResponseException!.ErrorMessage.Should().Be(ErrorCode.UserNotFound.GetDescription());
        }

        [Test]
        public async Task ResendVerificationEmail_WhenUserFoundByEmail_ReturnsSuccessResponse()
        {
            var request = new ResendEmailRequest()
            {
                Email = "user@secureapi.comEngineer"
            };

            var httpResponse = await PostAsync(AuthControllerRoutes.ResendEmailVerificationCode, request);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse>();

            response.StatusCode.Should().Be(200);
            response.IsSuccess.Should().BeTrue();
        }

        #endregion
    }
}