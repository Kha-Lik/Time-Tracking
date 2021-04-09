using System.Threading.Tasks;
using TimeTracking.Common.Wrapper;
using TimeTracking.Identity.Models.Requests;

namespace TimeTracking.Identity.BL.Abstract.Services
{
    public interface IRoleService
    {
        Task<ApiResponse> AddUserToRoleAsync(AddToRoleRequest model);
    }
}