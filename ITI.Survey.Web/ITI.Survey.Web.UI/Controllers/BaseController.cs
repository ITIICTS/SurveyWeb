using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        public static JsonSerializerSettings JSONSetting
        {
            get
            {
                var jsonSetting = new JsonSerializerSettings { Culture = CultureInfo.CurrentCulture };
                return jsonSetting;
            }
        }

        public static IEnumerable<object> GetErrorList(ModelStateDictionary modelState)
        {
            var listError =
                modelState.Select(c => new { c, firstOrDefault = c.Value.Errors.FirstOrDefault() }).Where(
                    @t => @t.firstOrDefault != null).
                    Select(@t => new
                    {
                        @t.c.Key,
                        @t.firstOrDefault.ErrorMessage
                    });

            return listError;
        }

        public static string GetErrorMessageUpload(ModelStateDictionary modelState)
        {
            var listError =
                modelState.Select(c => new { c, firstOrDefault = c.Value.Errors.FirstOrDefault() }).Where(
                    @t => @t.firstOrDefault != null).
                    Select(@t => new
                    {
                        @t.c.Key,
                        @t.firstOrDefault.ErrorMessage
                    });

            string message = string.Empty;
            foreach (var item in listError)
            {
                message += string.Format("{0} ", item.ErrorMessage);
            }
            return message;
        }
    }
}