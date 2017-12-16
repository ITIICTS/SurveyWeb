using AGY.Solution.Helper;
using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
using ITI.Survey.Web.UI.StackingWebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Controllers
{
    [Authorize]
    public class SearchContainerController : BaseController
    {
        // GET: SearchContainer
        public ActionResult SearchContainerByContNo()
        {
            SearchContainerModel model = new SearchContainerModel();
            return View(model);
        }
        public ActionResult SearchContainerByKodeBlok()
        {
            SearchContainerModel model = new SearchContainerModel();
            return View(model);
        }
        [NonAction]
        IList<SearchContainerModel> SearchContainerByContNo(string userId, string containerNumber)
        {
            IList<SearchContainerModel> result = new List<SearchContainerModel>();
            string contnumber = containerNumber.Trim().ToUpper().Replace(" ", "");
            while (contnumber.Length < 11)
            {
                contnumber = contnumber + " ";
            }
            contnumber = contnumber.Substring(0, 4) + " " + contnumber.Substring(4, 3) + " " + contnumber.Substring(7, 3) + " " + contnumber.Substring(10, 1);
            contnumber = contnumber.Trim();
            using (var stackingService = new StackingSoapClient())
            {
                string xml = stackingService.FillContInOutByContainerForStock(userId, contnumber);
                if (!string.IsNullOrEmpty(xml))
                {
                    DataSet dataSet = Converter.ConvertXmlToDataSet(xml);
                    DataTable dataTable = dataSet.Tables[0];
                    result = dataTable.ToList<SearchContainerModel>();
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult DataTableContainerByContNo(SearchContainerModel searchContainerModel)
        {
            DataTableModel dataTableModel = new DataTableModel { CurrentPage = 1, IsDescending = true, PageSize = int.MaxValue, PropertyOrder = "Cont" };
            IList<SearchContainerModel> resultSearch = SearchContainerByContNo(Username, searchContainerModel.Cont);
            dataTableModel.TotalRow = resultSearch.Count;
            ViewBag.Pagination = PageHelper.CreateLink(dataTableModel.CurrentPage, dataTableModel.TotalRow, dataTableModel.PageSize, true);
            return View("_DataTableContainerByContNo", resultSearch);
        }

        [NonAction]
        IList<SearchContainerModel> SearchContainerByKodeBlok(string userId, string blok)
        {
            string kdblok = blok.Trim().ToUpper().Replace(" ", string.Empty);
            IList<SearchContainerModel> result = new List<SearchContainerModel>();
            using (var stackingService = new StackingSoapClient())
            {
                string xml = stackingService.FillContInOutByBlokForStock(userId, kdblok);
                if (!string.IsNullOrEmpty(xml))
                {
                    DataSet dataSet = Converter.ConvertXmlToDataSet(xml);
                    DataTable dataTable = dataSet.Tables[0];
                    result = dataTable.ToList<SearchContainerModel>();
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult DataTableContainerByKodeBlok(SearchContainerModel searchContainerModel)
        {
            DataTableModel dataTableModel = new DataTableModel { CurrentPage = 1, IsDescending = true, PageSize = int.MaxValue, PropertyOrder = "Cont" };
            IList<SearchContainerModel> resultSearch = SearchContainerByKodeBlok(Username, searchContainerModel.Location);
            dataTableModel.TotalRow = resultSearch.Count;
            ViewBag.Pagination = PageHelper.CreateLink(dataTableModel.CurrentPage, dataTableModel.TotalRow, dataTableModel.PageSize, true);
            return View("_DataTableContainerByBlok", resultSearch);
        }
    }
}