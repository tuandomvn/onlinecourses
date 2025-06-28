using Volo.Abp.AspNetCore.Mvc.UI.Bundling;


namespace Acme.OnlineCourses.ScriptContributors
{
    public class GlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/js/animate.js");
            //context.Files.AddIfNotContains("/js/site.js");
        }
    }
}