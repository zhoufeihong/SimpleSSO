using System.Web;
using System.Web.Optimization;

namespace SimpleSSO
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery/plugins").Include(
           "~/Scripts/jquery.validate.js",
           "~/Scripts/jquery.validate.unobtrusive.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/css/font-awesome.css",
                      "~/Content/site.css"));
            //SimpleSSO
            bundles.Add(new ScriptBundle("~/bundles/SimpleSSO").Include(
            "~/Scripts/SimpleSSO/alert-extend.js"));

            //admin-lte
            bundles.Add(new ScriptBundle("~/bundles/admin-lte").Include(
               "~/Plugins/admin-lte/js/app.js"));

            bundles.Add(new StyleBundle("~/admin-lte/css").Include(
                  "~/Plugins/admin-lte/css/AdminLTE.css",
                  "~/Plugins/admin-lte/css/skins/_all-skins.css"));

            //bootstrap-table
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-table").Include(
               "~/Plugins/bootstrap-table/js/bootstrap-table.js",
               "~/Plugins/bootstrap-table/js/bootstrap-table-locale-all.js",
               "~/Plugins/bootstrap-table/js/bootstrap-table-zh-CN.js"));

            bundles.Add(new StyleBundle("~/bootstrap-table/css").Include(
                  "~/Plugins/bootstrap-table/css/bootstrap-table.css"));

            //bootstrapValidator
            bundles.Add(new ScriptBundle("~/bundles/bootstrapValidator").Include(
               "~/Plugins/bootstrapValidator/js/bootstrapValidator.js"));

            bundles.Add(new StyleBundle("~/bootstrapValidator/css").Include(
                  "~/Plugins/bootstrapValidator/css/bootstrapValidator.css"));

            //fileinput
            bundles.Add(new ScriptBundle("~/bundles/fileinput").Include(
               "~/Plugins/fileinput/js/fileinput.js",
               "~/Plugins/fileinput/js/fileinput-zh.js"));

            bundles.Add(new StyleBundle("~/fileinput/css").Include(
                  "~/Plugins/fileinput/css/fileinput.css"));

            //jquery-linq
            bundles.Add(new ScriptBundle("~/bundles/jquery-linq").Include(
               "~/Plugins/jquery-linq/js/jquery-linq.js"));

            //toastr
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
               "~/Plugins/toastr/js/toastr.js"));

            bundles.Add(new StyleBundle("~/toastr/css").Include(
                  "~/Plugins/toastr/css/toastr.css"));

            //select2
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
               "~/Plugins/select2/js/select2.full.js",
               "~/Plugins/select2/js/i18n/zh-CN.js"));

            bundles.Add(new StyleBundle("~/select2/css").Include(
                  "~/Plugins/select2/css/select2.css"));

            //jquery ui
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
               "~/Plugins/jquery-ui/js/jquery-ui.js"));

            bundles.Add(new StyleBundle("~/jquery-ui/css").Include(
                  "~/Plugins/jquery-ui/css/jquery-ui.css",
                  "~/Plugins/jquery-ui/css/jquery-ui.theme.css"));

        }
    }
}
