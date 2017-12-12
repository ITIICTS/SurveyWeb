using AGY.Solution.Helper;
using ITI.Survey.Web.UI.Filters;
using ITI.Survey.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Controllers
{
    [Authorize]
    public class SampleController : BaseController
    {

        // GET: Sample
        public ActionResult Index()
        {
            SampleModel model = new SampleModel();
            ViewBag.DropList = DropList();
            return View(model);
        }

        List<SampleModel> DataSamples()
        {
            List<SampleModel> data = new List<SampleModel>();

            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(1), SampleDescription = "Desc 1", SampleDropList = "Drop List 1", SampleNumber = 1 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(2), SampleDescription = "Desc 2", SampleDropList = "Drop List 2", SampleNumber = 2 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(3), SampleDescription = "Desc 3", SampleDropList = "Drop List 3", SampleNumber = 3 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(4), SampleDescription = "Desc 4", SampleDropList = "Drop List 4", SampleNumber = 4 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(5), SampleDescription = "Desc 5", SampleDropList = "Drop List 5", SampleNumber = 5 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(6), SampleDescription = "Desc 6", SampleDropList = "Drop List 6", SampleNumber = 6 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(7), SampleDescription = "Desc 7", SampleDropList = "Drop List 7", SampleNumber = 7 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(8), SampleDescription = "Desc 8", SampleDropList = "Drop List 8", SampleNumber = 8 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(9), SampleDescription = "Desc 9", SampleDropList = "Drop List 9", SampleNumber = 9 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(10), SampleDescription = "Desc 10", SampleDropList = "Drop List 10", SampleNumber = 10 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(11), SampleDescription = "Desc 11", SampleDropList = "Drop List 11", SampleNumber = 11 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(12), SampleDescription = "Desc 12", SampleDropList = "Drop List 12", SampleNumber = 12 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(13), SampleDescription = "Desc 13", SampleDropList = "Drop List 13", SampleNumber = 13 });
            data.Add(new SampleModel { SampleID = Guid.NewGuid(), SampleCurrency = 100000, SampleDate = DateTime.Now.AddDays(14), SampleDescription = "Desc 14", SampleDropList = "Drop List 14", SampleNumber = 14 });

            return data;
        }

        IEnumerable<SelectListItem> DropList()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            for (int i = 1; i < 6; i++)
            {
                result.Add(new SelectListItem
                {
                    Value = string.Format("Drop List {0}", i),
                    Text = string.Format("Drop List {0}", i)
                });
            }


            return result;
        }

        [HttpPost]
        public ActionResult DataTableSample(SampleModel objSearch)
        {
            DataTableModel objPage = new DataTableModel { CurrentPage = 1, IsDescending = true, PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PageSize"]), PropertyOrder = "SampleDescription" };
            List<SampleModel> resultSearch = DataSamples();
            objPage.TotalRow = resultSearch.Count;
            ViewBag.Pagination = PageHelper.CreateLink(objPage.CurrentPage, objPage.TotalRow, objPage.PageSize, true);
            return View("_DataTableSample", resultSearch);
        }

        [HttpPost]
        public ActionResult DataTableSampleJson(string stringSearch, int page = 1, int pageSize = 5, string propertyOrder = "CreatedDate", bool isDescending = true)
        {
            SampleModel objSearch = Newtonsoft.Json.JsonConvert.DeserializeObject<SampleModel>(stringSearch, JSONSetting);
            DataTableModel objPage = new DataTableModel { CurrentPage = page, IsDescending = isDescending, PageSize = pageSize, PropertyOrder = propertyOrder };
            List<SampleModel> resultSearch = DataSamples();
            objPage.TotalRow = resultSearch.Count;
            string pagination = PageHelper.CreateLink(objPage.CurrentPage, objPage.TotalRow, objPage.PageSize, true);
            var jsonResult = new
            {
                DataTable = resultSearch,
                TotalRow = objPage.TotalRow,
                Pagination = pagination
            };
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        // GET: Sample/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult LookupSample()
        {
            return View("_LookupSample");
        }

        [HttpPost]
        public ActionResult DataTableLookupSample(SampleModel objSearch)
        {
            DataTableModel objPage = new DataTableModel { CurrentPage = 1, IsDescending = true, PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PageSize"]), PropertyOrder = "SampleDescription" };
            List<SampleModel> resultSearch = DataSamples();
            objPage.TotalRow = resultSearch.Count;
            ViewBag.Pagination = PageHelper.CreateLink(objPage.CurrentPage, objPage.TotalRow, objPage.PageSize, true);
            return View("_DataTableLookupSample", resultSearch);
        }

        [HttpPost]
        public ActionResult DataTableLookupSampleJson(string stringSearch, int page = 1, int pageSize = 5, string propertyOrder = "CreatedDate", bool isDescending = true)
        {
            SampleModel objSearch = Newtonsoft.Json.JsonConvert.DeserializeObject<SampleModel>(stringSearch, JSONSetting);
            DataTableModel objPage = new DataTableModel { CurrentPage = page, IsDescending = isDescending, PageSize = pageSize, PropertyOrder = propertyOrder };
            List<SampleModel> resultSearch = DataSamples();
            objPage.TotalRow = resultSearch.Count;
            string pagination = PageHelper.CreateLink(objPage.CurrentPage, objPage.TotalRow, objPage.PageSize, true);
            var jsonResult = new
            {
                DataTable = resultSearch,
                TotalRow = objPage.TotalRow,
                Pagination = pagination
            };
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        // GET: Sample/Create
        public ActionResult Create()
        {
            SampleModel model = new SampleModel();
            ViewBag.DropList = DropList();
            return View(model);
        }

        // POST: Sample/Create
        [HttpPost]
        public ActionResult Create(SampleModel model)
        {
            try
            {
                // TODO: Add insert logic here
                model.SampleValidate(ModelState);
                if (ModelState.IsValid)
                {
                    // Do Logical Process

                    // Return Status Logical Process
                    return Json(new
                    {
                        Status = true,
                        Href = Url.Action("Index", "Sample")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
                }

                //return RedirectToAction("Index");
            }
            catch
            {
                return Json(new
                {
                    Status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateHeaderDetail()
        {
            SampleModel model = new SampleModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateHeaderDetail(SampleModel model)
        {
            try
            {
                model.SampleList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SampleModel>>(model.SampleListJson);

                // TODO: Add insert logic here
                model.SampleValidate(ModelState);
                if (ModelState.IsValid)
                {
                    // Do Logical Process

                    // Return Status Logical Process
                    return Json(new
                    {
                        Status = true,
                        Href = Url.Action("Index", "Sample")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
                }

                //return RedirectToAction("Index");
            }
            catch
            {
                return Json(new
                {
                    Status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Sample/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sample/Edit/5
        [HttpPost]
        public ActionResult Edit(SampleModel model)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Sample/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sample/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
