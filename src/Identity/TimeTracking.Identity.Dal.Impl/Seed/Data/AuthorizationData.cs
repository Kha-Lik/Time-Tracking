namespace TimeTracking.Identity.Dal.Impl.Seed.Data
{
    public static class AuthorizationData
    {
        public enum Roles
        {
            Engineer,
            ProjectManager,
            TeamLead,
        }
        public const string DefaultUsername = "user";
        public const string DefaultEmail = "user@secureapi.com";
        public const string DefaultPassword = "Pa$$w0rd_";
        public const Roles DefaultRole = Roles.Engineer;
    }
}