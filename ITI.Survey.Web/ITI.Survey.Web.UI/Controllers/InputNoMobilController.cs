using ITI.Survey.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AGY.Solution.Helper.Common;
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
            InputNoMobilModel model = new InputNoMobilModel();
            return View(model);
        }

        // POST: Sample/Create
        [HttpPost]
        public ActionResult Create(InputNoMobilModel model)
        {
            try
            {

                string resultMsg = "";
                bool status = false;
                using (var stackingService = new StackingWebService.StackingSoapClient())
                {
                    model.InputNoMobilSampleValidate(ModelState);
                    if (ModelState.IsValid)
                    {
                        string message = stackingService.SubmitNoMobil(Converter.ConvertToXML(model));
                        if (message.Contains("Error"))
                        {
                            resultMsg = message;
                            status = false;
                        }
                        else
                        {
                            char[] delimiters = new char[] { '\r', '\n' };
                            string[] parts = message.Split(delimiters);

                            foreach (string s in parts)
                            {
                                resultMsg += s + "\r\n";
                            }

                            status = true;
                        }
                        return Json(new { Status = status, Message = resultMsg }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
                    }
                }
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