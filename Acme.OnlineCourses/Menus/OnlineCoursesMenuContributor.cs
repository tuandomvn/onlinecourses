using Acme.OnlineCourses.Localization;
using Acme.OnlineCourses.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace Acme.OnlineCourses.Menus;

public class OnlineCoursesMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<OnlineCoursesResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                OnlineCoursesMenus.Home,
                l["Menu:Home"],
                "~/",
                // icon: "fas fa-home",
                order: 0
            )
        );

        context.Menu.AddItem(
           new ApplicationMenuItem(
               OnlineCoursesMenus.Course,
               l["Menu:Course"],
               "~/Course",
               order: 2
           )
       );

        // Register menu - public access
        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.StudentsRegister,
                l["Menu:Students:Register"],
                url: "/Students/Register",
                //icon: "fas fa-user-plus",
                order: 3
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.Partner,
                l["Menu:Partner"],
                "~/Partner",
                order: 4
            )
        );


        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.EmploymentSupport,
                l["Menu:EmploymentSupport"],
                "~/Employment-Support",
                order: 4
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.About,
                l["Menu:About"],
                "~/About-Us",
                order: 5
            )
        );

        context.Menu.AddItem(
           new ApplicationMenuItem(
               OnlineCoursesMenus.Contact,
               l["Menu:Contact"],
               "~/Contact",
               order: 6
           )
       );

        // Register menu - public access
        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.StudentsProfile,
                l["Menu:Students:Profile"],
                url: "/Students/Profile",
                // icon: "fa-solid fa-address-card",
                order: 6,
                   requiredPermissionName: OnlineCoursesPermissions.Students.Default
            )
        );

        // List menu - admin only
        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.StudentsList,
                l["Menu:Students:List"],
                url: "/Students",
                // icon: "fas fa-list",
                order: 7,
                requiredPermissionName: OnlineCoursesPermissions.Students.Default
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.Agencies,
                l["Menu:Agencies"],
                url: "/Agencies",
                // icon: "fas fa-building",
                order: 8,
                requiredPermissionName: OnlineCoursesPermissions.Agencies.Default
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.Blogs,
                l["Menu:Blogs"],
                url: "/Blogs",
                // icon: "fas fa-blog",
                order: 9,
                requiredPermissionName: OnlineCoursesPermissions.Blogs.Default
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.Report,
                l["Menu:Report"],
                url: "/Reports",
                // icon: "fa-solid fa-file-excel",
                order: 10,
                requiredPermissionName: OnlineCoursesPermissions.Reports.Default
            )
        );

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 2);
    }
}
