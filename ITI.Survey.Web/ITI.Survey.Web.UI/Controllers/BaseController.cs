using AGY.Solution.DataAccess;
using AGY.Solution.Helper.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Controllers
{
    public class BaseController : Controller
    {
        public static string SESSION_CURUSER = "CURRUSER";

        public BaseController()
        {
            ViewBag.CurrentUserName = System.Web.HttpContext.Current.User.Identity.Name;
        }

        [NonAction]
        public void GetConfigXML()
        {
            using (var globalService = new GlobalWebService.GlobalSoapClient())
            {
                var ds = Converter.ConvertXmlToDataSet(globalService.GetConfigXML(Username));
                ViewBag.HeavyEquipmentList = GetDropListByDataTable(ds.Tables["dt1"]);
                ViewBag.ContainerSize = GetDropListByDataTable(ds.Tables["dt2"]);
                ViewBag.ContainerType = GetDropListByDataTable(ds.Tables["dt3"]);
                ViewBag.Customers = GetDropListByDataTable(ds.Tables["dt4"]);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetDropListByDataTable(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new SelectListItem
                {
                    Value = ConvertData.ToString(dr, "Key"),
                    Text = ConvertData.ToString(dr, "Key")
                });
            }

            return result.OrderBy(x => x.Value);
        }

        public static JsonSerializerSettings JSONSetting
        {
            get
            {
                var jsonSetting = new JsonSerializerSettings { Culture = CultureInfo.CurrentCulture };
                return jsonSetting;
            }
        }

        public string Username
        {
            get
            {
                return System.Web.HttpContext.Current.User.Identity.Name;
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