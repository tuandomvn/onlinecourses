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
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OnlineCoursesResource>(name);
    }
} 