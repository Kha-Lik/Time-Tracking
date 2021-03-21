using System;

namespace TimeTracking.Bl.Abstract.Services
{
    public interface IUserProvider
    {
       Guid GetUserId();
    }
}