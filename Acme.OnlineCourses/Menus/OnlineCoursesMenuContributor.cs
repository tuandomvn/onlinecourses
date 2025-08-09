using Acme.OnlineCourses.Localization;
using Acme.OnlineCourses.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

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
                "~/Index",
                // icon: "fas fa-home",
                order: 0
               
            )
        );

        // About Us menu - chỉ hiển thị cho anonymous và student
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
        
        bool shouldShowPublicMenu = true;
        
        if (currentUser.IsAuthenticated)
        {
            // Ẩn menu homepage role admin hoặc agency
            if (currentUser.Roles.Contains(OnlineCoursesConsts.Roles.Agency)
                || currentUser.Roles.Contains(OnlineCoursesConsts.Roles.Administrator))
            {
                shouldShowPublicMenu = false;
            }
        }
        else
        {
           // context.Menu.AddItem(
           //    new ApplicationMenuItem(
           //        "Login",
           //        l["Menu:Login"],
           //        url: "/Account/Login",
           //        order: 15
           //    )
           //);
        }

        if (shouldShowPublicMenu)
        {
            context.Menu.AddItem(
               new ApplicationMenuItem(
                   OnlineCoursesMenus.Course,
                   l["Menu:Course"],
                   "~/Courses",
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
                    "~/Partners",
                    order: 4
                )
            );

            context.Menu.AddItem(
               new ApplicationMenuItem(
                   OnlineCoursesMenus.EmploymentSupport,
                   l["Menu:EmploymentSupport"],
                   "~/EmploymentSupports",
                   order: 5
               )
           );

            context.Menu.AddItem(
                new ApplicationMenuItem(
                    OnlineCoursesMenus.About,
                    l["Menu:About"],
                    "~/About-Us",
                    order: 6
                )
            );

           

            context.Menu.AddItem(
               new ApplicationMenuItem(
                   OnlineCoursesMenus.Contact,
                   l["Menu:Contact"],
                   "~/Contact",
                   order: 7
               )
            );

            context.Menu.AddItem(
                new ApplicationMenuItem(
                    OnlineCoursesMenus.StudentsProfile,
                    l["Menu:Students:Profile"],
                    url: "/Students/Profile",
                    // icon: "fa-solid fa-address-card",
                    order: 8,
                    requiredPermissionName: OnlineCoursesPermissions.Students.Default
                )
            );
        }

        // List menu - admin only
        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.StudentsList,
                l["Menu:Students:List"],
                url: "/Students",
                // icon: "fas fa-list",
                order: 9,
                requiredPermissionName: OnlineCoursesPermissions.Students.Default
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.Agencies,
                l["Menu:Agencies"],
                url: "/Agencies",
                order: 10,
                requiredPermissionName: OnlineCoursesPermissions.Agencies.Default
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.Report,
                l["Menu:Report"],
                url: "/Reports",
                // icon: "fa-solid fa-file-excel",
                order: 12,
                requiredPermissionName: OnlineCoursesPermissions.Reports.Default
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                OnlineCoursesMenus.EmploymentSupportAdmin,
                l["Menu:EmploymentSupportAdmin"],
                url: "/EmploymentSupportAdmin",
                order: 13,
                requiredPermissionName: OnlineCoursesPermissions.Reports.Default
            )
        );

        //Remove luon menu nay
        //context.Menu.AddItem(
        //    new ApplicationMenuItem(
        //        OnlineCoursesMenus.AgentRegisterAdmin,
        //        l["Menu:AgentRegisterAdmin"],
        //        url: "/AgentRegisterAdmin",
        //        order: 14,
        //        requiredPermissionName: OnlineCoursesPermissions.Reports.Default
        //    )
        //);


        // Login/Logout menu - hiển thị dựa trên trạng thái đăng nhập
        //if (currentUser.IsAuthenticated)
        //{
        //    // User đã đăng nhập - hiển thị Hello message và menu Logout
        //    var fullName = currentUser.Name ?? currentUser.UserName ?? "User";
        //    context.Menu.AddItem(
        //        new ApplicationMenuItem(
        //            "HelloUser",
        //            $"{l["Hello"]}, {fullName}",
        //            url: "#",
        //            order: 14
        //        )
        //    );

        //    context.Menu.AddItem(
        //        new ApplicationMenuItem(
        //            "Logout",
        //            l["Menu:Logout"],
        //            url: "/Account/Logout",
        //            order: 15
        //        )
        //    );
        //}
        //else
        //{
        //    // User chưa đăng nhập - hiển thị menu Login
        //    context.Menu.AddItem(
        //        new ApplicationMenuItem(
        //            "Login",
        //            l["Menu:Login"],
        //            url: "/Account/Login",
        //            order: 15
        //        )
        //    );
        //}

        //administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);
        //administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 2);
        // Ẩn hoàn toàn menu Administration cho tất cả role
        administration.Items.Clear();
    }
}
