using Acme.OnlineCourses.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Acme.OnlineCourses.Permissions;

public class OnlineCoursesPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var onlineCoursesGroup = context.AddGroup(OnlineCoursesPermissions.GroupName, L("Permission:OnlineCourses"));

        var studentsPermission = onlineCoursesGroup.AddPermission(OnlineCoursesPermissions.Students.Default, L("Permission:Students"));
        studentsPermission.AddChild(OnlineCoursesPermissions.Students.Create, L("Permission:Students.Create"));
        studentsPermission.AddChild(OnlineCoursesPermissions.Students.Edit, L("Permission:Students.Edit"));
        studentsPermission.AddChild(OnlineCoursesPermissions.Students.Delete, L("Permission:Students.Delete"));

        var agenciesPermission = onlineCoursesGroup.AddPermission("AgencyManagement", L("Permission:Agencies"));
        agenciesPermission.AddChild("AgencyManagement.Create", L("Permission:Agencies.Create"));
        agenciesPermission.AddChild("AgencyManagement.Edit", L("Permission:Agencies.Edit"));
        agenciesPermission.AddChild("AgencyManagement.Delete", L("Permission:Agencies.Delete"));

        var blogsPermission = onlineCoursesGroup.AddPermission(OnlineCoursesPermissions.Admins.Default, L("Permission:Blogs"));
        blogsPermission.AddChild(OnlineCoursesPermissions.Admins.Create, L("Permission:Blogs.Create"));
        blogsPermission.AddChild(OnlineCoursesPermissions.Admins.Edit, L("Permission:Blogs.Edit"));
        blogsPermission.AddChild(OnlineCoursesPermissions.Admins.Delete, L("Permission:Blogs.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OnlineCoursesResource>(name);
    }
} 