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
using TimeTracking.Tests.Common;
using TimeTracking.Tests.Common.Data;
using TimeTracking.Tests.Common.Extensions;
using TimeTracking.WebApi;

namespace TimeTracking.IntegrationTests.ControllersTests
{
    public class ProjectControllerTests : RequestBase<Startup>
    {
        [SetUp]
        public override void SetUp()
        {
            Factory = new TimeTrackingWebApplicationFactory();
            base.SetUp();
            var token = GetJwtToken();
            AddAuth(token);
        }

        #region GetProjectById

        [Test]
        public async Task GetProjectById_WhenFound_ReturnsProject()
        {
            var expected = ProjectsDbSet.Get().First();
            var projectId = expected.Id;

            var httpResponse = await GetAsync(ProjectControllerRoutes.BaseRoute + "/" + projectId);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<ProjectDetailsDto>>();
            response.VerifySuccessResponse();
            response.Data.Should().BeEquivalentTo(GetProjectDetails(expected));
        }

        [Test]
        public async Task GetProjectById_WhenNotFound_ReturnsProjectNotFound()
        {
            var projectId = Guid.NewGuid();

            var httpResponse = await GetAsync(ProjectControllerRoutes.BaseRoute + "/" + projectId);
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<ProjectDetailsDto>>();

            response.VerifyNotSuccessResponseWithErrorCodeAndMessage(ErrorCode.ProjectNotFound);
        }

        #endregion

        #region GetAllProjects

        [TestCase(1, 2, 1, 2)]
        [TestCase(1, 3, 1, 3)]
        public async Task GetAllProjects_WhenRequestValid_ReturnsAllProjects(int page, int size, int expectedPage,
            int expectedSize)
        {
            var expected = ProjectsDbSet.Get().ToList();
            var pagedRequest = new PagedRequest()
            {
                Page = page,
                PageSize = size
            };
            var httpResponse = await GetAsync(ProjectControllerRoutes.BaseRoute + "?" + pagedRequest.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<ProjectDetailsDto>>();

            response.EnsurePagedResult(expected.Count, expectedSize, expectedPage);
        }

        [Test]
        public async Task GetAllProjects_WhenRequestValid_ReturnsValidProjects()
        {
            var pagedRequest = new PagedRequest()
            {
                Page = 1,
                PageSize = 2
            };
            var expected = ProjectsDbSet.Get().Take(pagedRequest.PageSize).ToList();
            var mappedExpectedResult = expected.Select(GetProjectDetails).ToList();

            var httpResponse = await GetAsync(ProjectControllerRoutes.BaseRoute + "?" + pagedRequest.ToQueryString());
            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiPagedResponse<ProjectDetailsDto>>();

            response.Data.Should().BeEquivalentTo(mappedExpectedResult);
        }

        #endregion

        #region CreateProject

        [Test]
        public async Task CreateProject_WhenModelValid_CreatesProjectInDbAndReturnsSuccessResponse()
        {
            var claims = new Claim("roles", "ProjectManager");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);
            var projectCount = ProjectsDbSet.Get().Count();
            var request = new ProjectDto()
            {
                Description = "description",
                InitialRisk = DateTimeOffset.Now.AddDays(-3222),
                Abbreviation = "AAAA",
                Name = "name"
            };

            var httpResponse = await PostAsync(ProjectControllerRoutes.CreateProject, request);

            httpResponse.EnsureSuccessStatusCode();
            var response = await httpResponse.BodyAs<ApiResponse<ProjectDto>>();
            await CheckProjectsAddedToDatabase(response.Data, projectCount + 1);
            response.VerifySuccessResponse();
            await ReSeedDatabase();
        }

        [Test]
        public async Task CreateProject_WhenNotValidModelPassed_ReturnsValidationError()
        {
            var claims = new Claim("roles", "ProjectManager");
            var token = GetJwtToken(new[] {claims});
            AddAuth(token);

            //risk not valid
            var request = new ProjectDto()
            {
                Description = "description",
                Abbreviation = "AAAA",
                Name = "name"
            };
            var httpResponse = await PostAsync(ProjectControllerRoutes.CreateProject, request);
            var response = await httpResponse.BodyAs<ApiResponse<ProjectDto>>();
            response.CheckValidationException(1);

            request = new ProjectDto()
            {
                InitialRisk = DateTimeOffset.Now,
                Abbreviation = "AAAA",
                Name = "name"
            };
            httpResponse = await PostAsync(ProjectControllerRoutes.CreateProject, request);
            response = await httpResponse.BodyAs<ApiResponse<ProjectDto>>();
            response.CheckValidationException(2);

            request = new ProjectDto()
            {
                Name = "name"
            };
            httpResponse = await PostAsync(ProjectControllerRoutes.CreateProject, request);
            response = await httpResponse.BodyAs<ApiResponse<ProjectDto>>();
            response.CheckValidationException(5);
        }

        #endregion

        #region helpers

        private ProjectDetailsDto GetProjectDetails(Project project)
        {
            var mapper = GetService<IModelMapper<Project, ProjectDetailsDto>>();
            var model = mapper.MapToModel(project);
            return model;
        }

        private async Task CheckProjectsAddedToDatabase(ProjectDto project, int expectedCount)
        {
            var context = GetService<TimeTrackingDbContext>();
            var projectInDatabase = await context.Projects.LastAsync();
            context.Projects.Should().HaveCount(expectedCount);
            project.Should().BeEquivalentTo(projectInDatabase, opt => opt.ExcludingMissingMembers());
        }

        private async Task<Project> GetProjectFromDatabase(Guid projectId)
        {
            var context = GetService<TimeTrackingDbContext>();
            var projectInDatabase = await context.Projects.FindAsync(projectId);
            return projectInDatabase;
        }

        #endregion
    }
}