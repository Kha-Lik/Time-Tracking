using System;
using System.Runtime.Serialization;

namespace TimeTracking.Identity.BL.Impl.Exceptions
{
    [Serializable]
    public class AuthorizationError : Exception
    {
        public AuthorizationError()
        {
        }

        protected AuthorizationError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AuthorizationError(string? message) : base(message)
        {
        }

        public AuthorizationError(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}