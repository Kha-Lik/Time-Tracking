using System.Threading.Tasks;
using TimeTracking.Identity.Entities;
using TimeTracking.Identity.Models;

namespace TimeTracking.Identity.BL.Abstract.Factories
{
    public interface IJwtFactory
    {
        Task<JwtAccessToken> GenerateEncodedAccessToken(User user);
        RefreshToken GenerateRefreshToken(int size = 32);
    }
}