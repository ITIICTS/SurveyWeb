using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ITI.Survey.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(UserData), new UserDataModelBinder<UserData>());
        }
    }
}
