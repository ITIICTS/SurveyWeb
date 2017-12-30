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
    [Authorize]
    public class BongkarContainerController : BaseController
    {
        public ActionResult BongkarContainerAV()
        {
            BongkarContainerModel model = new BongkarContainerModel();
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ScanBarcode()
        {
            Int64? result = null;

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
                            //BarcodeImaging.ScanPage(ref barcodes, bi, 100, BarcodeImaging.ScanDirection.Vertical, BarcodeImaging.BarcodeType.Code39);

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
        public ActionResult FillContCard(long contCardID)
        {
            ContCardModel contCard = new ContCardModel();
            using (var stackingService = new StackingSoapClient())
            {
                string strContCard = stackingService.FillContCardByIdAndCardMode(Username, contCardID, "IN");
                if (strContCard.Length != 0)
                {
                    var dataSetContCard = Converter.ConvertXmlToDataSet(strContCard);
                    var listContCard = dataSetContCard.Tables[0].ToList<ContCardModel>();
                    contCard = listContCard.FirstOrDefault();
                }
            }
            return Json(contCard, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProsesBongkarContainerAV(BongkarContainerModel model, UserData userData)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
            }

            string resultMessage = string.Empty;
            bool status = false;
            using (var stackingService = new StackingSoapClient())
            {
                ContCardModel contCard = new ContCardModel();
                string strContCard = stackingService.FillContCardByIdAndCardMode(Username, model.ContCardID, "IN");
                if (strContCard.Length != 0)
                {
                    var dataSetContCard = Converter.ConvertXmlToDataSet(strContCard);
                    var listContCard = dataSetContCard.Tables[0].ToList<ContCardModel>();
                    contCard = listContCard.FirstOrDefault();
                }

                ContInOutModel contInOut = new ContInOutModel();
                string strContInOut = stackingService.FillContInOutById(Username, contCard.RefID);
                if (strContInOut.Length != 0)
                {
                    var dataSetContInOut = Converter.ConvertXmlToDataSet(strContInOut);
                    var listContInOut = dataSetContInOut.Tables[0].ToList<ContInOutModel>();
                    contInOut = listContInOut.FirstOrDefault();
                }

                model.Location = model.Blok.ToUpper() + model.Bay + model.Row + model.Tier;
                model.ContInOutId = contInOut.ContInOutId;
                model.ContCardID = contCard.ContCardID;
                model.FlagAct = "BONGKAR AV";
                model.Cont = contInOut.Cont;
                model.Shipper = contInOut.CustomerCode;
                model.ActiveUser = Username;
                model.EqpId = userData.HEID;
                model.OPID = userData.OPID;

                if (model.Side == "Kiri" || string.IsNullOrEmpty(model.Side))
                {
                    model.Cont = contInOut.Cont;
                    model.Cont2 = string.Empty;
                }
                else
                {
                    model.Cont = string.Empty;
                    model.Cont2 = contInOut.Cont;
                }

                string xml = Converter.ConvertToXML(model);
                string message = stackingService.SubmitKartuBongkar(xml);
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
            }
            return Json(new { Status = status, Message = resultMessage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BongkarContainerDM()
        {
            BongkarContainerModel model = new BongkarContainerModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProsesBongkarContainerDM(BongkarContainerModel model, UserData userData)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
            }

            string resultMessage = string.Empty;
            bool status = false;
            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                ContCardModel contCard = new ContCardModel();
                string strContCard = stackingService.FillContCardByIdAndCardMode(Username, model.ContCardID, "IN");
                if (strContCard.Length != 0)
                {
                    var dataSetContCard = Converter.ConvertXmlToDataSet(strContCard);
                    var listContCard = dataSetContCard.Tables[0].ToList<ContCardModel>();
                    contCard = listContCard.FirstOrDefault();
                }

                ContInOutModel contInOut = new ContInOutModel();
                string strContInOut = stackingService.FillContInOutById(Username, contCard.RefID);
                if (strContInOut.Length != 0)
                {
                    var dataSetContInOut = Converter.ConvertXmlToDataSet(strContInOut);
                    var listContInOut = dataSetContInOut.Tables[0].ToList<ContInOutModel>();
                    contInOut = listContInOut.FirstOrDefault();
                }

                model.Location = model.Blok.ToUpper() + model.Bay + model.Row + model.Tier;
                model.ContInOutId = contInOut.ContInOutId;
                model.ContCardID = contCard.ContCardID;
                model.FlagAct = "BONGKAR DM";
                model.Cont = contInOut.Cont;
                model.Shipper = contInOut.CustomerCode;
                model.ActiveUser = Username;
                model.EqpId = userData.HEID;
                model.OPID = userData.OPID;

                if (model.Side.Equals("Kiri") || string.IsNullOrEmpty(model.Side))
                {
                    model.Cont = contInOut.Cont;
                    model.Cont2 = string.Empty;
                }
                else
                {
                    model.Cont = string.Empty;
                    model.Cont2 = contInOut.Cont;
                }

                string xml = Converter.ConvertToXML(model);
                string message = stackingService.SubmitKartuBongkar(xml);
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
            }
            return Json(new { Status = status, Message = resultMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}