using Volo.Abp.Reflection;

namespace Acme.OnlineCourses.Permissions;

//Các nhóm permission trên trang phân quyền
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

    public static class Agencies
    {
        public const string Default = GroupName + ".Agencies";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Blogs
    {
        public const string Default = GroupName + ".Blogs";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Reports
    {
        public const string Default = GroupName + ".Reports";
    }
} 