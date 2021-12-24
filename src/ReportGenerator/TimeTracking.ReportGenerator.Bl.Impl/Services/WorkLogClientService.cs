using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TimeTracking.Common.HttpClientHandler;
using TimeTracking.Common.Jwt;
using TimeTracking.Common.Wrapper;
using TimeTracking.ReportGenerator.Bl.Abstract;
using TimeTracking.ReportGenerator.Bl.Impl.Heplers;
using TimeTracking.ReportGenerator.Bl.Impl.Settings;
using TimeTracking.ReportGenerator.Models;
using TimeTracking.ReportGenerator.Models.Requests;

namespace TimeTracking.ReportGenerator.Bl.Impl.Services
{
    public class WorkLogClientService : IWorkLogClientService
    {
        private readonly IHttpProvider _provider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeTrackingClientSettings _settings;
        private readonly ILogger<WorkLogClientService> _logger;

        public WorkLogClientService(IHttpProvider provider,
            IOptions<TimeTrackingClientSettings> settings,
            IHttpContextAccessor httpContextAccessor,
            ILogger<WorkLogClientService> logger)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger;
            _settings = settings.Value;
        }


        public async Task<ApiResponse<UserActivityDto>> GetUserActivities(ReportGeneratorRequest request)
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
                var activities = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiResponse<UserActivityDto>>(activities);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while fetching user {0} activities {1}",
                    request.UserId, request.ProjectId);
                return ApiResponse<UserActivityDto>.InternalError();
            }
        }
    }
}