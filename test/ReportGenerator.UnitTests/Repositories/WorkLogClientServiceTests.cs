using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoMockHelper.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using TimeTracking.Common.HttpClientHandler;
using TimeTracking.Common.Wrapper;
using TimeTracking.ReportGenerator.Bl.Impl.Services;
using TimeTracking.ReportGenerator.Bl.Impl.Settings;
using TimeTracking.ReportGenerator.Models;
using TimeTracking.ReportGenerator.Models.Requests;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace ReportGenerator.UnitTests.Repositories
{
    [TestFixture]
    public class WorkLogClientServiceTests: AutoMockContext<WorkLogClientService>
    {
        private static Fixture Fixture = new Fixture();

        [Test]
        public async Task GetUserActivities_WhenSendSuccessfully_ReturnListOfUserActivities_()
        {
            var request = Fixture.Freeze<ReportGeneratorRequest>();
            var expectedActivities = Fixture.Create<ApiResponse<UserActivityDto>>();
            MockHttpContextGetToken( MockFor<IHttpContextAccessor>(), "access_token","qwqw");
            MockFor<IOptions<TimeTrackingClientSettings>>().Setup(e => e.Value)
                .Returns(new TimeTrackingClientSettings()
                {
                    Url = "http://url/",
                    IdentityUrl = "http://identity.url/"
                });
            HttpResponseMessage httpMessage = new HttpResponseMessage();    
            httpMessage.StatusCode = System.Net.HttpStatusCode.OK;//Setting statuscode    
            httpMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expectedActivities));
            httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            MockFor<IHttpProvider>().Setup(e=>e.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(httpMessage)
                .Verifiable();
           
            var response = await ClassUnderTest.GetUserActivities(request);
            
            response.Data.Should().BeEquivalentTo(expectedActivities.Data);
        }
        
        
        [Test]
        public async Task GetUserActivities_WhenExceptionThrown_ShouldReturnInternalError()
        {
            var request = Fixture.Freeze<ReportGeneratorRequest>();
            var expectedActivities = Fixture.Create<ApiResponse<UserActivityDto>>();
            MockHttpContextGetToken( MockFor<IHttpContextAccessor>(), "access_token","qwqw");
            MockFor<IOptions<TimeTrackingClientSettings>>().Setup(e => e.Value)
                .Returns(new TimeTrackingClientSettings()
                {
                    Url = "http://url/",
                    IdentityUrl = "http://identity.url/"
                });
            HttpResponseMessage responseh = new HttpResponseMessage();    
            responseh.StatusCode = System.Net.HttpStatusCode.OK;//Setting statuscode    
            responseh.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expectedActivities));
            responseh.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            MockFor<IHttpProvider>().Setup(e => e.GetAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());
           
            var response = await ClassUnderTest.GetUserActivities(request);
            
            response.VerifyInternalError();
        }
        
        
        
        private void MockHttpContextGetToken(
            Mock<IHttpContextAccessor> httpContextAccessorMock,
            string tokenName, string tokenValue, string scheme = null)
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            httpContextAccessorMock
                .Setup(x => x.HttpContext.RequestServices.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationServiceMock.Object);

            var authResult = AuthenticateResult.Success(
                new AuthenticationTicket(new ClaimsPrincipal(), scheme));

            authResult.Properties.StoreTokens(new[]
            {
                new AuthenticationToken { Name = tokenName, Value = tokenValue }
            });

            authenticationServiceMock
                .Setup(x => x.AuthenticateAsync(httpContextAccessorMock.Object.HttpContext, scheme))
                .ReturnsAsync(authResult);
        }
        /*public async Task<ApiResponse<UserActivityDto>> GetUserActivities(ReportGeneratorRequest request)
        {
            try
            {
                var nw = new NameValueCollection()
                {
                    {nameof(ReportGeneratorRequest.ProjectId), request.ProjectId.ToString()},
                    {nameof(ReportGeneratorRequest.UserId), request.UserId.ToString()},
                };
                _provider.SetBearerAuthorization(await _httpContextAccessor.HttpContext.GetTokenAsync("access_token"));

                var response = await _provider.GetAsync(_settings.Url + Routes.UserActivities + nw.ToQueryString());

                response.EnsureSuccessStatusCode();

                var activities = await response.Content.ReadAsAsync<ApiResponse<UserActivityDto>>();
                return activities;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while fetching user {0} activities {1}",
                    request.UserId, request.ProjectId);
                return ApiResponse<UserActivityDto>.InternalError();
            }
        }*/
    }
}