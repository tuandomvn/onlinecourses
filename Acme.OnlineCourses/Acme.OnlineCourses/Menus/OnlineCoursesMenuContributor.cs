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

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<OnlineCoursesResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                OnlineCoursesMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fas fa-home",
                order: 0
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesPermissions.Students.Default,
                l["Menu:Students"],
                url: "/Students",
                icon: "fas fa-user-graduate",
                order: 1
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesPermissions.Agencies.Default,
                l["Menu:Agencies"],
                url: "/Agencies",
                icon: "fas fa-building",
                order: 2
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesPermissions.Blogs.Default,
                l["Menu:Blogs"],
                url: "/Blogs",
                icon: "fas fa-blog",
                order: 3
            )
        );

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 2);

        return Task.CompletedTask;
    }
}
