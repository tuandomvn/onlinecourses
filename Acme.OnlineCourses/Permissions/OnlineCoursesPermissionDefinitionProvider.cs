using Acme.OnlineCourses.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Acme.OnlineCourses.Permissions;

public class OnlineCoursesPermissionDefinitionProvider : PermissionDefinitionProvider
{
    //Tạo nhóm quyền hạn, để thể hiện trên trang Permission Management
    //Link với phân quyền ban đầu: SeedRolesAsync
    public override void Define(IPermissionDefinitionContext context)
    {
        var onlineCoursesGroup = context.AddGroup(OnlineCoursesPermissions.GroupName, L("Permission:OnlineCourses"));

        var studentsPermission = onlineCoursesGroup.AddPermission(OnlineCoursesPermissions.Students.Default, L("Permission:Students"));
        studentsPermission.AddChild(OnlineCoursesPermissions.Students.Create, L("Permission:Students.Create"));
        studentsPermission.AddChild(OnlineCoursesPermissions.Students.Edit, L("Permission:Students.Edit"));
        studentsPermission.AddChild(OnlineCoursesPermissions.Students.Delete, L("Permission:Students.Delete"));

        var agenciesPermission = onlineCoursesGroup.AddPermission(OnlineCoursesPermissions.Agencies.Default, L("Permission:Agencies"));
        agenciesPermission.AddChild(OnlineCoursesPermissions.Agencies.Create, L("Permission:Agencies.Create"));
        agenciesPermission.AddChild(OnlineCoursesPermissions.Agencies.Edit, L("Permission:Agencies.Edit"));
        agenciesPermission.AddChild(OnlineCoursesPermissions.Agencies.Delete, L("Permission:Agencies.Delete"));

        var blogsPermission = onlineCoursesGroup.AddPermission(OnlineCoursesPermissions.Blogs.Default, L("Permission:Blogs"));
        blogsPermission.AddChild(OnlineCoursesPermissions.Blogs.Create, L("Permission:Blogs.Create"));
        blogsPermission.AddChild(OnlineCoursesPermissions.Blogs.Edit, L("Permission:Blogs.Edit"));
        blogsPermission.AddChild(OnlineCoursesPermissions.Blogs.Delete, L("Permission:Blogs.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OnlineCoursesResource>(name);
    }
} 