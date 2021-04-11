using FluentAssertions;
using TimeTracking.Common.Wrapper;

namespace TimeTracking.IntegrationTests.Extensions
{
    public static class ValidationExceptionExtensions
    {
        public  static void  CheckValidationException(this ApiResponse response, int expectedCount)
        {
            response.ResponseException.Should().NotBeNull();
            response.ResponseException!.ValidationErrors.Should().HaveCount(expectedCount);
        }
    }
}