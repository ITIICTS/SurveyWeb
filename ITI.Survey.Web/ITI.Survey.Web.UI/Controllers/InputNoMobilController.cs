using ITI.Survey.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ITI.Survey.Web.UI.Controllers
{
    public class InputNoMobilController : BaseController
    {
        // GET: InputNoMobil
        public ActionResult Index()
        {
            InputNoMobilModel model = new InputNoMobilModel();
            return View(model);
        }
        public ActionResult Create()
        {
            SampleModel model = new SampleModel();
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

    }
}