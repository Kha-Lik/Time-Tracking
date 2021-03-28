using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Bl.Abstract.Services;
using TimeTracking.Common.Requests;
using TimeTracking.Common.Wrapper;
using TimeTracking.Models;

namespace TimeTracking.WebApi.Controllers
{
    /// <summary>
    /// Milestone controller
    /// </summary>
    [ApiController]
    [Route("api/milestone")]
    public class MilestoneController : ControllerBase
    {
        private readonly IMileStoneService _mileStoneService;

        /// <summary>
        /// Milestone controller constructor
        /// </summary>
        /// <param name="mileStoneService"></param>
        public MilestoneController(IMileStoneService mileStoneService)
        {
            _mileStoneService = mileStoneService;
        }

        /// <summary>
        /// Creates milestone by request
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ApiResponse<MilestoneDto>> CreateMileStoneAsync([FromBody] MilestoneDto dto)
        {
            return await _mileStoneService.CreateMileStoneAsync(dto);
        }

        /// <summary>
        /// Returns paged response with all milestones
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiPagedResponse<MilestoneDetailsDto>> GetAllMilestones([FromQuery] PagedRequest request)
        {
            return await _mileStoneService.GetAllMilestonesPaged(request);
        }

        /// <summary>
        /// Returns milestone by id
        /// </summary>
        /// <param name="mileStoneId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{mileStoneId}")]
        public async Task<ApiResponse<MilestoneDetailsDto>> GetMileStoneById([FromRoute] Guid mileStoneId)
        {
            return await _mileStoneService.GetMileStoneById(mileStoneId);
        }
    }
}