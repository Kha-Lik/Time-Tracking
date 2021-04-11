namespace TimeTracking.IntegrationTests
{
    public static class ApiRoutes
    {
        public static string ApiRoute = "api/";

    }
    public  static class IssueControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "issue";
        public static readonly string CreateIssue = BaseRoute+"/create-issue";
        public static readonly string ChangeIssueStatus = BaseRoute+"/change-status";
        public static readonly string AssignToUser = BaseRoute +"/assign-to-user";
    }
    
    public  static class MilestoneControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "milestone";
        public static string CreateMilestone = BaseRoute+"/create";
    }
    
    public  static class ProjectControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "project";
        public static string CreateProject = BaseRoute+"/create";
    }
    
    public  static class TeamControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "team";
        public static string CreateTeam = BaseRoute+"/create";
    } 
    public  static class UserControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "user";
        public static string AddUserToTeam = BaseRoute+"/add-to-team";
        public static string AllUsers = BaseRoute+"/all-users";
    }   
    public  static class WorkLogControllerRoutes
    {
        public static readonly string BaseRoute = ApiRoutes.ApiRoute + "workLog";
        public static string CreateWorkLog = BaseRoute+"/create";
        public static string GetUserWorkLogs = BaseRoute+"/get-user-activities";
        public static string UpdateWorkLog = BaseRoute+"/update";
        public static string UpdateWorkLogStatus = BaseRoute+"/update-status";
    }
}