using ITI.Survey.Web.UI.Models;
using System.Web.Mvc;
using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Filters;
using System;

namespace ITI.Survey.Web.UI.Controllers
{
    [Authorize]
    [SurveyActionFilter]
    public class InputNoMobilController : BaseController
    {
        // GET: InputNoMobil

        public ActionResult Index()
        {
            InputNoMobilModel model = new InputNoMobilModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult InputMobil(InputNoMobilModel model)
        {
            model.ActiveUser = Username;
            model.Validate(ModelState);
            if (ModelState.IsValid)
            {

                string resultMsg = "";
                bool status = false;
                using (var stackingService = new StackingWebService.StackingSoapClient())
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
            }
            else
            {
                return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}
