using AGY.Solution.Helper.Common;
using log4net;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace ITI.Survey.Web.UI.Filters
{
    public class SurveyActionFilterAttribute : ActionFilterAttribute
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string machineName = Environment.MachineName;
            string ipaddress = string.Empty;
            foreach (var item in Dns.GetHostAddresses(Environment.MachineName))
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipaddress = item.MapToIPv4().ToString();
                }
            }

            try
            {
                if (machineName != Decryptor.DecryptString(ConfigurationManager.AppSettings["MachineName"])
                    || ipaddress != Decryptor.DecryptString(ConfigurationManager.AppSettings["Address"]))
                {
                    string controllerName = (string)filterContext.RouteData.Values["controller"];
                    string actionName = (string)filterContext.RouteData.Values["action"];
                    string strHttpMethod = filterContext.HttpContext.Request.HttpMethod;

                    log.InfoFormat("{0} Action {1} -> Message: {2}", controllerName, actionName, strHttpMethod + machineName + ipaddress);

                    filterContext.HttpContext.Session.Clear();
                    System.Web.Security.FormsAuthentication.SignOut();

                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Filter -> Message: {0}", ex.Message);
                log.ErrorFormat("Filter -> StackTrace: {0}", ex.StackTrace);

                filterContext.HttpContext.Session.Clear();
                System.Web.Security.FormsAuthentication.SignOut();

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }


            base.OnActionExecuting(filterContext);
        }
    }
}