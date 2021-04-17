namespace ReportGenerator.IntegrationTests
{
    public static class ApiRoutes
    {
        public static string ApiRoute = "api/";
    }

    public static class ReportControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "reports";
    }
}