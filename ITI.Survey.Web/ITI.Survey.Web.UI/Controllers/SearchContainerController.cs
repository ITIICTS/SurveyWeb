using AGY.Solution.Helper;
using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Filters;
using ITI.Survey.Web.UI.Models;
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
            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                String xml = stackingService.FillContInOutByContainerNumber(userId, contnumber);
                if (!string.IsNullOrEmpty(xml))
                {
                    DataSet ds = Converter.ConvertXmlToDataSet(xml);
                    DataTable dt = ds.Tables[0];
                    result = dt.ToList<SearchContainerModel>();
                }
            }

            return result;
        }

        [HttpPost]
        public ActionResult DataTableContainerByContNo(SearchContainerModel objSearch)
        {
            DataTableModel objPage = new DataTableModel { CurrentPage = 1, IsDescending = true, PageSize = int.MaxValue, PropertyOrder = "Cont" };
            IList<SearchContainerModel> resultSearch = SearchContainerByContNo(Username, objSearch.Cont);
            objPage.TotalRow = resultSearch.Count;
            ViewBag.Pagination = PageHelper.CreateLink(objPage.CurrentPage, objPage.TotalRow, objPage.PageSize, true);
            return View("_DataTableContainerByContNo", resultSearch);
        }

        [NonAction]
        IList<SearchContainerModel> SearchContainerByKodeBlok(string userId, string blok)
        {
            string kdblok = blok.Trim().ToUpper().Replace(" ", "");

            IList<SearchContainerModel> result = new List<SearchContainerModel>();

            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                String xml = stackingService.FillContInOutByBlokForStock(userId, kdblok);
                if (!string.IsNullOrEmpty(xml))
                {
                    DataSet ds = Converter.ConvertXmlToDataSet(xml);
                    DataTable dt = ds.Tables[0];
                    result = dt.ToList<SearchContainerModel>();
                }
            }

            return result;
        }

        [HttpPost]
        public ActionResult DataTableContainerByKodeBlok(SearchContainerModel objSearch)
        {
            DataTableModel objPage = new DataTableModel { CurrentPage = 1, IsDescending = true, PageSize = int.MaxValue, PropertyOrder = "Cont" };
            IList<SearchContainerModel> resultSearch = SearchContainerByKodeBlok(Username, objSearch.Location);
            objPage.TotalRow = resultSearch.Count;
            ViewBag.Pagination = PageHelper.CreateLink(objPage.CurrentPage, objPage.TotalRow, objPage.PageSize, true);
            return View("_DataTableContainerByBlok", resultSearch);
        }
    }
}