using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TimeTracking.Common.Jwt;
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
    public class MilestoneControllerTests:Request<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var claims = new List<Claim>()
            {
                new Claim(Constants.Strings.JwtClaimIdentifiers.Id.ToString(), Guid.NewGuid().ToString())
            };
            var token = MockJwtTokens.GenerateJwtToken(claims);
            AddAuth(token);
        }


        #region CreateMilestone
         [Test]
        public async Task CreateMilestone_WhenProjectNotFound_ReturnsProjectFound()
        {
            var request = new MilestoneDto()
            {
                State = State.Closed,
                Description = "description",
                DueDate = DateTimeOffset.Now,
                ProjectId = Guid.NewGuid(),
                Title = "tittle"
            };

            var httpResponse = await Post(MilestoneControllerRoutes.CreateMilestone, request);
            
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<MilestoneDto>>();
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.ProjectNotFound);
        }
        
     
        [Test]
        public async Task CreateMilestone_WhenModelValid_CreatesMilestoneInDbAndReturnsSuccessResponse()
        {
            var milestonesCount = MilestonesDbSet.Get().Count();
            var request = new MilestoneDto()
            {
                State = State.Closed,
                Description = "description",
                DueDate = DateTimeOffset.UtcNow.AddDays(-3050),
                ProjectId = ProjectsDbSet.Get().First().Id,
                Title = "tittle"
            };
            
            var httpResponse = await Post(MilestoneControllerRoutes.CreateMilestone, request);
            
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<MilestoneDto>>();
            await CheckMilestoneIsAddedToDatabase(response.Data,milestonesCount+1);
            response.VerifySuccessResponse();
            ReSeedDatabase();
        }
        
        [Test]
        public async Task CreateMilestone_WhenNotValidModelPassed_ReturnsValidationError()
        {
            var request =new MilestoneDto()
            {
                State = State.Closed,
                Description = "description",
                DueDate = DateTimeOffset.Now,
                Title = "tittle"
            };
            var httpResponse = await  Post(MilestoneControllerRoutes.CreateMilestone, request);
            var response = await httpResponse.BodyAs<ApiResponse<MilestoneDto>>();
            response.CheckValidationException(expectedCount:1);

            request =new MilestoneDto()
            {
                State = State.Closed,
                Description = "description",
                DueDate = DateTimeOffset.Now,
            };
            httpResponse =await  Post(MilestoneControllerRoutes.CreateMilestone, request);
            response = await httpResponse.BodyAs<ApiResponse<MilestoneDto>>();
            response.CheckValidationException(expectedCount:3);
  
            request =new MilestoneDto()
            {
                State = State.Closed,
                DueDate = DateTimeOffset.Now,
            };
            httpResponse = await  Post(MilestoneControllerRoutes.CreateMilestone, request);
            response =await httpResponse.BodyAs<ApiResponse<MilestoneDto>>();
            response.CheckValidationException(expectedCount:5);
        }
        

        #endregion
        #region GetMileStoneById

        [Test]
        public async Task GetMileStoneById_WhenFound_ReturnsMilestone()
        {
            var expected = MilestonesDbSet.Get().First();
            var milestoneId = expected.Id;
            
            var httpResponse = await _client.GetAsync(MilestoneControllerRoutes.BaseRoute+"/"+milestoneId);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<MilestoneDetailsDto>>();
            
            response.VerifySuccessResponse();
            response.Data.Should().BeEquivalentTo(GetMilestoneDetails(expected));
        }

        [Test]
        public async Task GetMileStoneById_WhenNotFound_ReturnsMilestoneNotFound()
        {

            var milestoneId = Guid.NewGuid();
            
            var httpResponse = await _client.GetAsync(MilestoneControllerRoutes.BaseRoute+"/"+milestoneId);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<MilestoneDetailsDto>>();
            
            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.MileStoneNotFound);
        }
        #endregion

        #region GetAllMilestones
        
        [TestCase(1,2,1,2)]
        [TestCase(1,3,1,3)]
        public async Task GetAllMilestones_WhenRequestValid_ReturnsAllMilestones(int page,int size,int expectedPage,int expectedSize)
        {
            var expected = MilestonesDbSet.Get().ToList();
            var pagedRequest = new PagedRequest()
            {
                Page = page,
                PageSize = size,
            };
            var httpResponse = await _client.GetAsync(MilestoneControllerRoutes.BaseRoute+"?"+pagedRequest.GetQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<MilestoneDetailsDto>>();
           
            response.EnsurePagedResult(expected.Count,expectedSize,expectedPage);
        }
        
        [Test]
        public async Task GetAllIssuesAsync_WhenRequestValid_ReturnsValidIssues()
        {
            var pagedRequest = new PagedRequest()
            {
                Page = 1,
                PageSize = 2,
            };
            var expected =  MilestonesDbSet.Get().Take(pagedRequest.PageSize).ToList();
            var mappedExpectedResult = expected.Select(GetMilestoneDetails).ToList();
            
            var httpResponse = await _client.GetAsync(MilestoneControllerRoutes.BaseRoute+"?"+pagedRequest.GetQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<MilestoneDetailsDto>>();

            response.Data.Should().BeEquivalentTo(mappedExpectedResult);

        }

        #endregion
        #region helpers
        private MilestoneDetailsDto GetMilestoneDetails(Milestone milestone)
        {
            var mapper = GetService<IModelMapper<Milestone, MilestoneDetailsDto>>();
            var model = mapper.MapToModel(milestone);
            return model;
        }
        
        private async Task CheckMilestoneIsAddedToDatabase(MilestoneDto milestone, int expectedCount)
        {
            
            var context = GetService<TimeTrackingDbContext>();
            var milestoneInDatabase = await context.Milestones.OrderBy(e=>e.CreatedAt).LastAsync();
            context.Milestones.Should().HaveCount(expectedCount);
            milestone.Should().BeEquivalentTo(milestoneInDatabase, opt=>opt.ExcludingMissingMembers());
        }
        private async Task<Milestone> GetMilestoneFromDatabase(Guid milestoneId)
        {
            var context = GetService<TimeTrackingDbContext>();
            var milestoneInDatabase = await context.Milestones.FindAsync(milestoneId);
            return milestoneInDatabase;
        }
        #endregion
    }
}