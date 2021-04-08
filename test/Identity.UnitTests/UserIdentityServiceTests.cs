using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TimeTracking.Common.Helpers;
using TimeTracking.Common.Mappers;
using TimeTracking.Common.Wrapper;
using TimeTracking.Contracts.Events;
using TimeTracking.Identity.BL.Impl.Services;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.Models.Dtos;
using TimeTracking.Identity.Models.Requests;

namespace Identity.UnitTests
{
    [TestFixture]
    public class UserIdentityServiceTests
    {
        private static Fixture Fixture = new Fixture();
        private Mock<UserManager<User>> _mockUserManger;
        private Mock<ISystemClock> _mockSystemClock;
        private Mock<IEmailHelperService> mockEmailHelperService;
        private Mock<IBaseMapper<User, UserDto>> mockUserMapper;
        private Mock<IPublishEndpoint> mockPublishEndpoint;
        private UserIdentityService sut;

        [SetUp]
        public void SetUp()
        {
            this._mockUserManger = MockHelpers.MockUserManager<User>();
            this._mockSystemClock = new Mock<ISystemClock>();
            this.mockEmailHelperService = new Mock<IEmailHelperService>();
            this.mockUserMapper = new Mock<IBaseMapper<User, UserDto>>();
            this.mockPublishEndpoint = new Mock<IPublishEndpoint>();
            var logger = new Mock<ILogger<UserIdentityService>>();
            this.sut = new UserIdentityService(_mockUserManger.Object,
                _mockSystemClock.Object,
                mockEmailHelperService.Object,
                mockUserMapper.Object,
                mockPublishEndpoint.Object,
                logger.Object);
        }


        #region RegisterAsync

        [Test]
        public async Task RegisterAsync_WhenCreatedSuccessfully_ShouldReturnSuccessResponse()
        {
            var request = new RegistrationRequest()
            {
                Email = "email",
                FirstName = "first name",
                LastName = "last name",
                Password = "password",
                Username = "userName"
            };
            _mockUserManger.Setup(e => e.CreateAsync(It.IsAny<User>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);
            mockEmailHelperService.Setup(e => e.SendEmailConfirmationEmail(It.IsAny<User>()))
                .ReturnsAsync(ApiResponse.Success());

            var response = await sut.RegisterAsync(request);

            mockEmailHelperService.Verify(e => e.SendEmailConfirmationEmail(It.IsAny<User>()));
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task RegisterAsync_WhenUserCreationFailed_ShouldReturnSendEmailConfirmationFailed()
        {
            var request = new RegistrationRequest()
            {
                Email = "email",
                FirstName = "first name",
                LastName = "last name",
                Password = "password",
                Username = "userName"
            };
            var identityResult = IdentityResult.Failed();
            _mockUserManger.Setup(e => e.CreateAsync(It.IsAny<User>(), request.Password))
                .ReturnsAsync(identityResult);

            var response = await sut.RegisterAsync(request);

            response.IsSuccess.Should().BeFalse();
            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.UserRegistrationFailed);
            response.ResponseException.ErrorMessage.Should().Be(identityResult.ToString());
        }

        [Test]
        public async Task RegisterAsync_WhenSendMessageFailed_ShouldReturnUserRegistrationFailed()
        {
            var request = new RegistrationRequest()
            {
                Email = "email",
                FirstName = "first name",
                LastName = "last name",
                Password = "password",
                Username = "userName"
            };
            var emailFailedResponse = ApiResponse.Failed();
            var identityResult = IdentityResult.Success;
            _mockUserManger.Setup(e => e.CreateAsync(It.IsAny<User>(), request.Password))
                .ReturnsAsync(identityResult);
            mockEmailHelperService.Setup(e => e.SendEmailConfirmationEmail(It.IsAny<User>()))
                .ReturnsAsync(emailFailedResponse);

            var response = await sut.RegisterAsync(request);

            mockEmailHelperService.Verify(e => e.SendEmailConfirmationEmail(It.IsAny<User>()));
            response.IsSuccess.Should().BeFalse();
            response.Should().BeEquivalentTo(emailFailedResponse);
        }

        #endregion

        #region ForgotPasswordAsync

        [Test]
        public async Task ForgotPasswordAsync_WhenUserNotFound_ShouldReturnEmailConfirmationFailed()
        {
            var request = new ForgotPasswordRequest()
            {
                Email = "email"
            };
            _mockUserManger.Setup(e => e.FindByEmailAsync(request.Email))
                .ReturnsAsync((User) null);
         
            var response = await sut.ForgotPasswordAsync(request);

            response.IsSuccess.Should().BeFalse();
            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.EmailConfirmationFailed);
            response.ResponseException.ErrorMessage.Should().Be(ErrorCode.EmailConfirmationFailed.GetDescription());
        }
        
        [Test]
        public async Task ForgotPasswordAsync_WhenEmailNotConfirmed_ShouldReturnEmailConfirmationFailed()
        {
            var request = new ForgotPasswordRequest()
            {
                Email = "email"
            };
            _mockUserManger.Setup(e => e.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(false);

            var response = await sut.ForgotPasswordAsync(request);

            response.IsSuccess.Should().BeFalse();
            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.EmailConfirmationFailed);
            response.ResponseException.ErrorMessage.Should().Be(ErrorCode.EmailConfirmationFailed.GetDescription());
        }
        
        [Test]
        public async Task ForgotPasswordAsync_WhenEmailConfirmedAndUserFound_ShouldSendResetPasswordConfirmationEmail()
        {
            var request = new ForgotPasswordRequest()
            {
                Email = "email"
            };
            var user = new User();
            _mockUserManger.Setup(e => e.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(true);
            _mockUserManger.Setup(e => e.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);
            
            var response = await sut.ForgotPasswordAsync(request);

            mockEmailHelperService.Verify(e=>e.SendResetPasswordConfirmationEmail(user));
        }

        #endregion

        #region ResetPasswordAsync

        [Test]
        public async Task ResetPasswordAsync_WhenUSerNotFound_ShouldReturnUserNotFoundResponse()
        {
            var request = new ResetPasswordRequest()
            {
                Password = "password",
                UserId = Guid.NewGuid()
            };
            _mockUserManger.Setup(e => e.FindByIdAsync(request.UserId.ToString()))
                .ReturnsAsync((User) null);

            var response = await sut.ResetPasswordAsync(request);

            response.IsSuccess.Should().BeFalse();
            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.UserNotFound);
            response.ResponseException.ErrorMessage.Should().Be(ErrorCode.UserNotFound.GetDescription());
        }
        
        [Test]
        public async Task ResetPasswordAsync_WhenPasswordResetFailed_ShouldReturnResetPasswordFailed()
        {
            var request = new ResetPasswordRequest()
            {
                Password = "password",
                UserId = Guid.NewGuid(),
                Code ="verification code",
            };
            _mockUserManger.Setup(e => e.FindByIdAsync(request.UserId.ToString()))
                .ReturnsAsync(new User());
            _mockUserManger.Setup(e => e.ResetPasswordAsync(It.IsAny<User>(), request.Code, request.Password))
                .ReturnsAsync(IdentityResult.Failed());

            var response = await sut.ResetPasswordAsync(request);

            response.IsSuccess.Should().BeFalse();
            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.ResetPasswordFailed);
            response.ResponseException.ErrorMessage.Should().Be(ErrorCode.ResetPasswordFailed.GetDescription());
        }
        
        [Test]
        public async Task ResetPasswordAsync_WhenPasswordResetSuccess_ShouldReturnSuccessResult()
        {
            var request = new ResetPasswordRequest()
            {
                Password = "password",
                UserId = Guid.NewGuid(),
                Code ="verification code",
            };
            _mockUserManger.Setup(e => e.FindByIdAsync(request.UserId.ToString()))
                .ReturnsAsync(new User());
            _mockUserManger.Setup(e => e.ResetPasswordAsync(It.IsAny<User>(), request.Code, request.Password))
                .ReturnsAsync(IdentityResult.Success);

            var response = await sut.ResetPasswordAsync(request);

            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(200);
        }

        #endregion

        #region ResentEmailAsync

        [Test]
        public async Task ResentEmailAsync_WhenFailedToFindUSerByEmail_ShouldReturnUserNotFound()
        {
            
            var request = new ResendEmailRequest()
            {
               Email = "email",
            };
            _mockUserManger.Setup(e => e.FindByEmailAsync(request.Email))
                .ReturnsAsync((User) null);

            var response = await sut.ResentEmailAsync(request);

            response.IsSuccess.Should().BeFalse();
            response.ResponseException!.ErrorCode.Should().Be(ErrorCode.UserNotFound);
            response.ResponseException.ErrorMessage.Should().Be(ErrorCode.UserNotFound.GetDescription());
        }

        [Test]
        public async Task ResentEmailAsync_WhenFoundUserByEmail_ShouldCallSendEmailConfirmationEmail()
        {
            var request = new ResendEmailRequest()
            {
                Email = "email",
            };
            var user = Fixture.Create<User>();
            _mockUserManger.Setup(e => e.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            var response = await sut.ResentEmailAsync(request);

            mockEmailHelperService.Verify(e=>e.SendEmailConfirmationEmail(user));
        }
        
        #endregion

        #region ConfirmEmailAsync

        [Test]
        public async Task ConfirmEmailAsync_WhenUserNotFound_ShouldReturnUserNotFound()
        {
            var request = new EmailConfirmationRequest()
            {
                Code = "code",
                UserId = Guid.NewGuid(),
            };

            var response = await sut.ConfirmEmailAsync(request);
        }
/*   public async Task<ApiResponse> ConfirmEmailAsync(EmailConfirmationRequest request)
        {
            var userFoundedResponse = await FindUserByIdAsync(request.UserId);
            if (!userFoundedResponse.IsSuccess)
            {
                return userFoundedResponse;
            }

            var result = await _userManager.ConfirmEmailAsync(userFoundedResponse.Data, request.Code);
            if (result.Succeeded)
            {

                var addToRoleAsync = await _userManager.AddToRoleAsync(userFoundedResponse.Data, AuthorizationData.DefaultRole.ToString());
                if (!addToRoleAsync.Succeeded)
                {
                    _logger.LogError("Failed to add user to to role with reason {0}", addToRoleAsync.ToString());
                    return new ApiResponse(
                        new ApiError()
                        {
                            ErrorCode = ErrorCode.AddUserToRoleFailed,
                            ErrorMessage = $"User registration failed with.",
                        });
                }
                return ApiResponse.Success();
            }

            _logger.LogWarning("Email confirmation failed for user with id {0} by reason {1}",
                request.UserId, result.Errors.ToString());
            return new ApiResponse()
            {
                ResponseException = new ApiError()
                {
                    ErrorCode = ErrorCode.EmailConfirmationFailed,
                    ErrorMessage = ErrorCode.EmailConfirmationFailed.GetDescription(),
                },
                StatusCode = 400,
            };
        }*/
        

        #endregion
    }
}