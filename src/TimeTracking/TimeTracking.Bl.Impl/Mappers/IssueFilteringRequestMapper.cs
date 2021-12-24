using TimeTracking.Common.Mappers;
using TimeTracking.Entities;
using TimeTracking.Entities.FilterModels;
using TimeTracking.Models;
using TimeTracking.Models.Filtering;

namespace TimeTracking.Bl.Impl.Mappers
{
    public class IssueFilteringRequestMapper: IModelMapper<IssueFilteringRequest, IssueFilteringModel>
    {
        public IssueFilteringModel MapToModel(IssueFilteringRequest entity)
        {
            return new IssueFilteringModel()
            {
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                MilestoneId = entity.MilestoneId,
            };
        }
    }
}