namespace SkillExchange.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            // Custom scripts
            bundles.Add(new ScriptBundle("~/bundles/populate-dropdowns-data").Include(
                      "~/Scripts/custom/populate-dropdowns-data.js"));

            bundles.Add(new ScriptBundle("~/bundles/autocomplete").Include(
                     "~/Scripts/custom/autocomplete.js"));

            bundles.Add(new ScriptBundle("~/bundles/notifications").Include(
                     "~/Scripts/custom/notifications.js"));

            bundles.Add(new ScriptBundle("~/bundles/messages").Include(
                     "~/Scripts/custom/messages.js"));

            // Styles
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include(
                      "~/Content/jquery-ui.css"));
        }
    }
}