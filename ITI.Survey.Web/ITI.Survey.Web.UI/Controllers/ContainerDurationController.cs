using AGY.Solution.Helper;
using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Controllers
{
    [Authorize]
    public class ContainerDurationController : BaseController
    {
        // GET: ContainerDuration
        public ActionResult Index()
        {
            ContainerDurationModel model = new ContainerDurationModel();
            GetConfigXML();
            return View(model);
        }

        [NonAction]
        IList<ContainerDurationModel> DataContainerDuration(string userId, string customerCode, string size, string type, string condition, int minDuration, string sortBy)
        {
            IList<ContainerDurationModel> result = new List<ContainerDurationModel>();

            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                String xml = stackingService.FillContainerDuration(userId, customerCode, size, type, condition, minDuration, sortBy);
                if (!string.IsNullOrWhiteSpace(xml) || !xml.Contains("Error"))
                {
                    DataSet ds = Converter.ConvertXmlToDataSet(xml);
                    DataTable dt = ds.Tables[0];
                    result = dt.ToList<ContainerDurationModel>();
                }
            }

            return result;
        }

        [HttpPost]
        public ActionResult DataTableContainerDuration(ContainerDurationModel objSearch)
        {
            DataTableModel objPage = new DataTableModel { CurrentPage = 1, IsDescending = true, PageSize = int.MaxValue, PropertyOrder = "Cont" };
            IList<ContainerDurationModel> resultSearch = DataContainerDuration(Username, objSearch.CustomerCode, objSearch.Size, objSearch.Type, objSearch.Condition, objSearch.Duration, objPage.PropertyOrder);
            objPage.TotalRow = resultSearch.Count;
            ViewBag.Pagination = PageHelper.CreateLink(objPage.CurrentPage, objPage.TotalRow, objPage.PageSize, true);
            return View("_DataTableContainerDuration", resultSearch);
        }

        //[HttpPost]
        //public ActionResult DataTableContainerDurationJson(string stringSearch, int page = 1, int pageSize = int.MaxValue, string propertyOrder = "CreatedDate", bool isDescending = true)
        //{
        //    ContainerDurationModel objSearch = Newtonsoft.Json.JsonConvert.DeserializeObject<ContainerDurationModel>(stringSearch, JSONSetting);
        //    DataTableModel objPage = new DataTableModel { CurrentPage = page, IsDescending = isDescending, PageSize = pageSize, PropertyOrder = propertyOrder };
        //    IList<ContainerDurationModel> resultSearch = DataContainerDuration(Username, objSearch.CustomerCode, objSearch.Size, objSearch.Type, objSearch.Condition, objSearch.Duration, objPage.PropertyOrder);
        //    objPage.TotalRow = resultSearch.Count;
        //    string pagination = PageHelper.CreateLink(objPage.CurrentPage, objPage.TotalRow, objPage.PageSize, true);
        //    var jsonResult = new
        //    {
        //        DataTable = resultSearch,
        //        TotalRow = objPage.TotalRow,
        //        Pagination = pagination
        //    };
        //    return Json(jsonResult, JsonRequestBehavior.AllowGet);
        //}
    }
}