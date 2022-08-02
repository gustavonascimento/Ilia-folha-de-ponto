namespace WebApi.Entities
{
    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string AdminOrUser = Admin + "," + User;
    }
}