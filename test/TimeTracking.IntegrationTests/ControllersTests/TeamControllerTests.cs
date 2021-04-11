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
using TimeTracking.IntegrationTests.Extensions;
using TimeTracking.IntegrationTests.Helpers;
using TimeTracking.Models;
using TimeTracking.Tests.Common;
using TimeTracking.Tests.Common.Data;
using TimeTracking.WebApi;

namespace TimeTracking.IntegrationTests.ControllersTests
{
    public class TeamControllerTests:Request<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var token = MockJwtTokens.GenerateJwtToken();
            AddAuth(token);
        }

        #region GetTeamById
        [Test]
        public async Task GetTeamById_WhenFound_ReturnsTeam()
        {
            var expected = TeamsDbSet.Get().First();
            var teamId = expected.Id;
            
            var httpResponse = await _client.GetAsync(TeamControllerRoutes.BaseRoute+"/"+teamId);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TeamDetailsDto>>();
            
            response.VerifySuccessResponse();
            response.Data.Should().BeEquivalentTo(GetTeamDetails(expected));
        }

        [Test]
        public async Task GetTeamById_WhenNotFound_ReturnsTeamNotFound()
        {
            var teamId = Guid.NewGuid();
            
            var httpResponse = await _client.GetAsync(TeamControllerRoutes.BaseRoute+"/"+teamId);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TeamDetailsDto>>();
            
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.TeamNotFound);
        }
        

        #endregion
        #region helpers
        private TeamDetailsDto GetTeamDetails(Team team)
        {
            var mapper = GetService<IModelMapper<Team, TeamDetailsDto>>();
            var model = mapper.MapToModel(team);
            return model;
        }
        private async Task CheckTeamAddedToDatabase(TeamDto team, int expectedCount)
        {
            var context = GetService<TimeTrackingDbContext>();
            var teamInDatabase = await context.Teams.LastAsync();
            context.Teams.Should().HaveCount(expectedCount);
            team.Should().BeEquivalentTo(teamInDatabase, opt=>opt.ExcludingMissingMembers());
        }
        private async Task<Team> GetTeamFromDatabase(Guid teamId)
        {
            var context = GetService<TimeTrackingDbContext>();
            var teamFromDatabase = await context.Teams.FindAsync(teamId);
            return teamFromDatabase;
        }
        #endregion
        
        #region GetAllTeams
        [TestCase(1,2,1,2)]
        [TestCase(1,3,1,3)]
        public async Task GetAllTeams_WhenRequestValid_ReturnsAllTeams(int page,int size,int expectedPage,int expectedSize)
        {
            var expected = TeamsDbSet.Get().ToList();
            var pagedRequest = new PagedRequest()
            {
                Page = page,
                PageSize = size,
            };
            var httpResponse = await _client.GetAsync(TeamControllerRoutes.BaseRoute+"?"+pagedRequest.GetQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<TeamDetailsDto>>();
           
            response.EnsurePagedResult(expected.Count,expectedSize,expectedPage);
        }
        
        [Test]
        public async Task GetAllTeams_WhenRequestValid_ReturnsValidTeams()
        {
            var pagedRequest = new PagedRequest()
            {
                Page = 1,
                PageSize = 2,
            };
            var expected =  TeamsDbSet.Get().Take(pagedRequest.PageSize).ToList();
            var mappedExpectedResult = expected.Select(GetTeamDetails).ToList();
            
            var httpResponse = await _client.GetAsync(TeamControllerRoutes.BaseRoute+"?"+pagedRequest.GetQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<TeamDetailsDto>>();

            response.Data.Should().BeEquivalentTo(mappedExpectedResult,
                opt=>opt.ExcludingMissingMembers()
                    .Excluding(e=>e.ProjectAbbreviation)
                    .Excluding(e=>e.ProjectName));
        }
        #endregion

        #region CreateTeamAsync
        [Test]
        public async Task CreateTeamAsync_WhenProjectNotFoundReturnsProjectNotFoundResponse()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = MockJwtTokens.GenerateJwtToken(new []{claims});
            AddAuth(token);
            var request = new TeamDto()
            {
                ProjectId = Guid.NewGuid(),
                MembersCount = 2,
                Name = "name",
            };
       
            var httpResponse = await Post(TeamControllerRoutes.CreateTeam, request);
            
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TeamDto>>();
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.ProjectNotFound);
        }
        
        [Test]
        public async Task CreateTeamAsync_WhenModelValid_CreatesTeamInDbAndReturnsSuccessResponse()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = MockJwtTokens.GenerateJwtToken(new []{claims});
            AddAuth(token);
            var projectCount = TeamsDbSet.Get().Count();
            var request = new TeamDto()
            {
                ProjectId =ProjectsDbSet.Get().First().Id,
                MembersCount = 2,
                Name = "name",
            };
       
            var httpResponse = await Post(TeamControllerRoutes.CreateTeam,request);
            
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<TeamDto>>();
            await CheckTeamAddedToDatabase(response.Data,projectCount+1);
            response.VerifySuccessResponse();
            ReSeedDatabase();
        }
   
        [Test]
        public async Task CreateTeamsAsync_WhenNotValidModelPassed_ReturnsValidationError()
        {
            var claims = new Claim("roles", "TeamLead");
            var token = MockJwtTokens.GenerateJwtToken(new []{claims});
            AddAuth(token);
            //project id not valid
            var request =new TeamDto()
            {
                MembersCount = 2,
                Name = "name",
            };
            var httpResponse = await  Post(TeamControllerRoutes.CreateTeam,request);
            var response = await httpResponse.BodyAs<ApiResponse<TeamDto>>();
            response.CheckValidationException(expectedCount:1);

            request =new TeamDto()
            {
                MembersCount = -100,
                Name = "name",
            };
            httpResponse =await   Post(TeamControllerRoutes.CreateTeam,request);
            response = await httpResponse.BodyAs<ApiResponse<TeamDto>>();
            response.CheckValidationException(expectedCount:2);
  
            request  =new TeamDto()
            {
                Name = "name",
            };
            httpResponse = await  Post(TeamControllerRoutes.CreateTeam,request);
            response =await httpResponse.BodyAs<ApiResponse<TeamDto>>();
            response.CheckValidationException(expectedCount:3);
        }
        #endregion
        
    }
}