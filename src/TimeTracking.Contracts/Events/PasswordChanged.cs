using System;

namespace TimeTracking.Contracts.Events
{
    public interface PasswordChanged
    {
        public Guid UserId
        {
            get;
            set;
        }
    }
}