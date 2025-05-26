namespace Acme.OnlineCourses.Permissions;

public static class OnlineCoursesPermissions
{
    public const string GroupName = "OnlineCourses";

    public static class Students
    {
        public const string Default = GroupName + ".Students";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
} 