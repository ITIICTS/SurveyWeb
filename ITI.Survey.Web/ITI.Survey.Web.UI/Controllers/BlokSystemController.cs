using ITI.Survey.Web.UI.Models;
using System.Linq;
using System.Web.Mvc;
using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.StackingWebService;

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
        public ActionResult GoByContainerNumber(string contNo)
        {
            string resultMessage = string.Empty;
            ContInOutModel contInOutModel = new ContInOutModel();
            BlokSystemModel blokSystemmodel = new BlokSystemModel();
            using (var stackingService = new StackingSoapClient())
            {
                string contNoText = contNo.Trim();
                contNoText = contNoText.Replace(" ", "");
                if (contNoText.Length <= 0)
                {
                    return View(contNo);
                }
                blokSystemmodel.ContNo = contNoText.Substring(0, 4) + " " +
                    contNoText.Substring(4, 3) + " " + contNoText.Substring(7, 3) + " " + contNoText.Substring(10, 1);

                string contIntOutXML = string.Empty;
                contIntOutXML = stackingService.FillContInOutByContainerNumber(Username, blokSystemmodel.ContNo.ToUpper());
                if (!string.IsNullOrEmpty(contIntOutXML))
                {
                    var dsContInOut = Converter.ConvertXmlToDataSet(contIntOutXML);
                    var listContIntOut = dsContInOut.Tables[0].ToList<ContInOutModel>();
                    contInOutModel = listContIntOut.FirstOrDefault();

                }
                else
                {
                    blokSystemmodel.ResultMessage = blokSystemmodel.ContNo + " tidak ditemukan.";
                    return View(blokSystemmodel);
                }
                blokSystemmodel.ResultMessage = contInOutModel.Cont + "\r\n";
                blokSystemmodel.Size = contInOutModel.Size;
                blokSystemmodel.ContInOutId = contInOutModel.ContInOutId;
                blokSystemmodel.Cont = contInOutModel.Cont;
                blokSystemmodel.Shipper = contInOutModel.CustomerCode;
                if (contInOutModel.Location.Length == 0 || contInOutModel.Location.Contains("TMP"))
                {
                    blokSystemmodel.ResultMessage += "Lokasi : Belum di set\r\n";
                }
                else
                {
                    blokSystemmodel.ResultMessage += "Lokasi : " + contInOutModel.Location.Substring(0, 1) + "  " +
                                                  contInOutModel.Location.Substring(1, 2) + "  " +
                                                  contInOutModel.Location.Substring(3, 2) + "  " +
                                                  contInOutModel.Location.Substring(5, 1) + "\r\n";
                }
                blokSystemmodel.ResultMessage += "Silakan input lokasi baru.";

            }

            return Json(blokSystemmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Process(BlokSystemModel model, UserData userData)
         {
            model.BlokSystemValidate(ModelState);
            if (!ModelState.IsValid)
            {
                return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
            }

            string _kodeBlok = model.Blok.ToUpper() + model.Bay + model.Row + model.Tier;

            var status = false;
            try
            {
                using (var stackingService = new StackingSoapClient())
                {
                    var contIntOutXML = stackingService.FillContInOutByContainerNumber(Username, model.ContNo);
                    var contInOutModel = new ContInOutModel();
                    if (!string.IsNullOrEmpty(contIntOutXML))
                    {
                        var dsContInOut = Converter.ConvertXmlToDataSet(contIntOutXML);
                        var listContIntOut = dsContInOut.Tables[0].ToList<ContInOutModel>();
                        contInOutModel = listContIntOut.FirstOrDefault();

                    }
                    else
                    {
                        return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
                    }
                    if (_kodeBlok.Length < 6)
                    {
                        status = false;
                        model.ResultMessage = "Lokasi kurang dari 6 digit";
                        return Json(new { Status = status, Message = model.ResultMessage }, JsonRequestBehavior.AllowGet);

                    }

                    model.ContInOutId = contInOutModel.ContInOutId;
                    model.Cont = contInOutModel.Cont;
                    model.Shipper = contInOutModel.CustomerCode;
                    model.Size = contInOutModel.Size;
                    model.ResultMessage = contInOutModel.Message;
                    model.ActiveUser = Username;
                    model.Location = _kodeBlok;
                    model.KodeBlok = _kodeBlok;

                    // User data from login
                    model.EqpId = userData.HEID;
                    model.OPID = userData.OPID;

                    var kodeBlok = model.Blok.ToUpper() + model.Bay + model.Row + model.Tier;
                    model.FlagAct = "MOVE";
                    // Logical Process
                    if (model.SideChoose.ToUpper() == "KIRI" || model.SideChoose == string.Empty)
                    {
                        model.Cont = model.Cont;
                        model.Cont2 = "";
                    }
                    else
                    {
                        model.Cont2 = model.Cont;
                        model.Cont = string.Empty;
                    }

                    model.ResultMessage = stackingService.SubmitBlokMove(Converter.ConvertToXML(model));
                    if (model.ResultMessage.Contains("Error"))
                    {
                        status = false;
                    }
                    else
                    {
                        status = true;
                        model.ResultMessage = model.ResultMessage = model.ContNo + "\r\nDipindahkan ke : " + model.Location;
                        model.Cont = string.Empty;
                        model.Blok = string.Empty;
                        model.Bay = string.Empty;
                        model.Row = string.Empty;
                        model.Bay = string.Empty;
                    }
                    return Json(new { Status = status, Message = model.ResultMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { Status = status, Message = model.ResultMessage }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}