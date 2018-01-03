using AGY.Solution.DataAccess;
using AGY.Solution.Filter;
using AGY.Solution.Helper.Common;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Controllers
{
    [AGYActionFilter]
    public class BaseController : Controller
    {
        public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BaseController()
        {
            ViewBag.CurrentUserName = System.Web.HttpContext.Current.User.Identity.Name;
        }

        [NonAction]
        public void GetConfigXML()
        {
            try
            {
                using (var globalService = new GlobalWebService.GlobalSoapClient())
                {
                    var dataSet = Converter.ConvertXmlToDataSet(globalService.GetConfigXML(Username));
                    ViewBag.HeavyEquipmentList = GetDropListByDataTable(dataSet.Tables["dt1"]);
                    ViewBag.ContainerSize = GetDropListByDataTable(dataSet.Tables["dt2"]);
                    ViewBag.ContainerType = GetDropListByDataTable(dataSet.Tables["dt3"]);
                    ViewBag.Customers = GetDropListByDataTable(dataSet.Tables["dt4"]);
                    ViewBag.Operators = GetDropListByDataTable(dataSet.Tables["dt5"], "PILIH OPID");
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("GetConfigXML --> Error Message : {0}", ex.Message);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetDropListByDataTable(DataTable dataTable, string optionLabel = "")
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                result.Add(new SelectListItem
                {
                    Value = ConvertData.ToString(dataRow, "Key"),
                    Text = string.IsNullOrWhiteSpace(ConvertData.ToString(dataRow, "Key")) ? optionLabel : ConvertData.ToString(dataRow, "Key")
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

        public static string ErrorMessageFromService(string messageError, string defaultMessageErrorIfMessageErrorIsEmpty)
        {
            return string.IsNullOrWhiteSpace(messageError) ? defaultMessageErrorIfMessageErrorIsEmpty : messageError.Replace("Error :", "");
        }
    }
}