using AGY.Solution.Helper;
using ITI.Survey.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace ITI.Survey.Web.UI.Controllers
{

    [Authorize]
    public class BlokSystemController : BaseController
    {
        // GET: BlokSystem
        public ActionResult Index()
        {
            BlokSystemModel model = new BlokSystemModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult GoByContainerNumber(string containerNumber)
        {
            BlokSystemModel model = new BlokSystemModel();
            model.ResultMessage = ""; //lokasi setelah klik process GO search by container number
            return View(model);
        }
        [HttpPost]
        public ActionResult Process(BlokSystemModel model)
        {
            try
            {
                // TODO: Add insert logic here
                model.BlokSystemValidate(ModelState);
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