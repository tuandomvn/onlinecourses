namespace Acme.OnlineCourses.Menus;

public class OnlineCoursesMenus
{
    public const string GroupName = "OnlineCourses";

    public const string Home = GroupName + ".Home";
    public const string About = GroupName + ".About";
    public const string StudentsMenu = GroupName + ".Students";
    public const string Agencies = GroupName + ".Agencies";
    public const string Blogs = GroupName + ".Blogs";

    public static class Students
    {
        public const string Register = OnlineCoursesMenus.StudentsMenu + ".Register";
        public const string List = OnlineCoursesMenus.StudentsMenu + ".List";
    }
}
