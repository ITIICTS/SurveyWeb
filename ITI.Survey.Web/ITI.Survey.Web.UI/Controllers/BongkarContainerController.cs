using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Controllers
{
    [Authorize]
    public class BongkarContainerController : BaseController
    {
        public ActionResult BongkarContainerAV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProsesBongkarContainerAV()
        {
            using (var stackingService = new StackingWebService.StackingSoapClient()) 
            {
                //stackingService.
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BongkarContainerDM()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProsesBongkarContainerDM()
        {
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}