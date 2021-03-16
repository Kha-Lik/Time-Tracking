using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Models;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class ProjectMapper:IBaseMapper<Project,ProjectDto>
    {
        public ProjectDto MapToModel(Project entity)
        {
            return new ProjectDto()
            {
                Abbreviation = entity.Abbreviation,
                Description = entity.Description,
                InitialRisk = entity.InitialRisk,
                Name = entity.Name,
            };
        }

        public Project MapToEntity(ProjectDto model)
        {
            return new Project()
            {
                Abbreviation = model.Abbreviation,
                Description = model.Description,
                InitialRisk = model.InitialRisk,
                Name = model.Name,
            };
        }
    }
}