using System.Threading.Tasks;
using Acme.OnlineCourses.Localization;
using Acme.OnlineCourses.MultiTenancy;
using Volo.Abp.UI.Navigation;

namespace Acme.OnlineCourses.Web.Menus
{
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
            var l = context.GetLocalizer<OnlineCoursesResource>();

            context.Menu.AddItem(
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
                    "Agencies",
                    l["Menu:Agencies"],
                    "/Agencies",
                    icon: "fas fa-building",
                    order: 1
                )
            );
        }
    }
} 