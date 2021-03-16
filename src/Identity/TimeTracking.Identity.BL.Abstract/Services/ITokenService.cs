using System.Threading.Tasks;
using TimeTracking.Identity.Models.Requests;
using TimeTracking.Identity.Models.Responses;

namespace TimeTracking.Identity.BL.Abstract.Services
{
    public interface ITokenService
    {
        Task<AuthResponse> LoginAsync(TokenExchangeRequest request);
        Task<AuthResponse> RefreshTokenAsync(string token);
        Task<AuthResponse> RevokeTokenAsync(string token);
    }
}