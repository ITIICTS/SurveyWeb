using ITI.Survey.Web.UI.Filters;
using System.Web;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ValidateAntiForgeryToken());
        }
    }
}
