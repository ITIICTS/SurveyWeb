using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidateAntiForgeryToken : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            //  Only validate POSTs
            if (request.HttpMethod != WebRequestMethods.Http.Post ||
                ReferenceEquals(filterContext.RouteData.Values["controller"], "Error")) return;
            //  Ajax POSTs and normal form posts have to be treated differently when it comes
            //  to validating the AntiForgeryToken
            if (request.IsAjaxRequest())
            {
                var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                var cookieValue = antiForgeryCookie != null
                                      ? antiForgeryCookie.Value
                                      : null;

                if (request.Headers["__RequestVerificationToken"] != null)
                    AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);

            }
            else
            {
                new ValidateAntiForgeryTokenAttribute()
                    .OnAuthorization(filterContext);
            }
        }
    }
}