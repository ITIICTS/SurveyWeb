using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
using ITI.Survey.Web.UI.StackingWebService;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Controllers
{
    //[Authorize]
    public class InputNoMobilController : BaseController
    {
        // GET: InputNoMobil
        public ActionResult Index()
        {
            InputNoMobilModel model = new InputNoMobilModel();
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ScanBarcode()
        {
            Int64? result = null;
            //var result = string.Empty;
            try
            {
                foreach (string file in Request.Files)
                {
                    var postedFile = Request.Files[file];
                    if (postedFile != null)
                    {
                        using (var bi = new Bitmap(postedFile.InputStream))
                        {
                            ArrayList barcodes = new ArrayList();
                            BarcodeImaging.FullScanPage(ref barcodes, bi, 100);
                            if (barcodes.Count > 0)
                                result = Convert.ToInt64(barcodes[0]);
                        }

                    }
                }
            }
            catch
            {
                result = null;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InputMobil(InputNoMobilModel model)
        {
            model.ActiveUser = Username;
            model.Validate(ModelState);
            if (ModelState.IsValid)
            {
                string resultMessage = string.Empty;
                bool status = false;
                using (var stackingService = new StackingSoapClient())
                {
                    string message = stackingService.SubmitNoMobil(Converter.ConvertToXML(model));
                    if (message.Contains("Error"))
                    {
                        resultMessage = message;
                        status = false;
                    }
                    else
                    {
                        char[] delimiters = new char[] { '\r', '\n' };
                        string[] parts = message.Split(delimiters);
                        foreach (string s in parts)
                        {
                            resultMessage += s + "\r\n";
                        }

                        status = true;
                    }
                    return Json(new { Status = status, Message = resultMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
            }
        }
    }

}
