namespace TimeTrackingIdentity.IntegrationTests
{
    public static class ApiRoutes
    {
        public static string ApiRoute = "api/";
    }

    public static class UserControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "user";
        public static readonly string AllUsers = BaseRoute + "/all-users";
    }

    public static class RoleControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "role";
        public static readonly string AddToRole = BaseRoute + "/add-to-role";
    }

    public static class AuthControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "auth";
        public static readonly string Register = BaseRoute + "/register";
        public static readonly string ConfirmEmail = BaseRoute + "/confirm-email";
        public static readonly string TokenExchange = BaseRoute + "/token";
        public static readonly string RefreshToken = BaseRoute + "/refresh";
        public static readonly string ForgotPassword = BaseRoute + "/forgot-password";
        public static readonly string ResetPassword = BaseRoute + "/reset-password";
        public static readonly string RevokeToken = BaseRoute + "/revoke-token";
        public static readonly string ResendEmailVerificationCode = BaseRoute + "/resend-email-verification-code";
    }
}