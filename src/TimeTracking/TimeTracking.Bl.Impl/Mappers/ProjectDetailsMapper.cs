using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class ProjectDetailsMapper : IModelMapper<Project, ProjectDetailsDto>
    {
        public ProjectDetailsDto MapToModel(Project entity)
        {
            return new ProjectDetailsDto()
            {
                Abbreviation = entity.Abbreviation,
                Description = entity.Description,
                InitialRisk = entity.InitialRisk,
                Name = entity.Name,
                ProjectId = entity.Id,
            };
        }
    }
}