using AGY.Solution.Helper;
using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
using ITI.Survey.Web.UI.StackingWebService;
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

            using (var stackingService = new StackingSoapClient())
            {
                string xml = stackingService.FillContainerDuration(userId, customerCode, size, type, condition, minDuration, sortBy);
                if (!string.IsNullOrWhiteSpace(xml) || !xml.Contains("Error"))
                {
                    DataSet dataSet = Converter.ConvertXmlToDataSet(xml);
                    DataTable dataTable = dataSet.Tables[0];
                    result = dataTable.ToList<ContainerDurationModel>();
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
    }
}