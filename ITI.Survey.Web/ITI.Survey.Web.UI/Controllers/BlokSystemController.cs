using ITI.Survey.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AGY.Solution.Helper.Common;



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
        public ActionResult GoByContainerNumber(string ContNo)
        {
            string resultMessage = string.Empty;
            bool status = false;
            ContInOutModel ContModel = new ContInOutModel();
            BlokSystemModel Blokmodel = new BlokSystemModel();
            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                string ContNoText = ContNo.Trim();
                ContNoText = ContNoText.Replace(" ", "");
                if (ContNoText.Length <= 0)
                {
                    return View(ContNo);
                }
                Blokmodel.ContNo = ContNoText.Substring(0, 4) + " " +
                    ContNoText.Substring(4, 3) + " " + ContNoText.Substring(7, 3) + " " + ContNoText.Substring(10, 1);
                Blokmodel.ContNo.ToUpper();

                string ContIntOutXML = string.Empty;
                ContIntOutXML = stackingService.FillContInOutByContainerNumber(Username, Blokmodel.ContNo);
                if (!string.IsNullOrEmpty(ContIntOutXML))
                {
                    var dsContInOut = Converter.ConvertXmlToDataSet(ContIntOutXML);
                    var listContIntOut = dsContInOut.Tables[0].ToList<ContInOutModel>();
                    ContModel = listContIntOut.FirstOrDefault();

                }
                else
                {
                    Blokmodel.ResultMessage = Blokmodel.ContNo + " tidak ditemukan.";
                    return View(Blokmodel);
                }
                Blokmodel.ResultMessage = ContModel.Cont + "\r\n";
                Blokmodel.Size = ContModel.Size;
                Blokmodel.ContInOutId = ContModel.ContInOutId;
                Blokmodel.Cont = ContModel.Cont;
                Blokmodel.Shipper = ContModel.CustomerCode;
                if (ContModel.Location.Length == 0 || ContModel.Location.Contains("TMP"))
                {
                    Blokmodel.ResultMessage += "Lokasi : Belum di set\r\n";
                }
                else
                {
                    Blokmodel.ResultMessage += "Lokasi : " + ContModel.Location.Substring(0, 1) + "  " +
                                                  ContModel.Location.Substring(1, 2) + "  " +
                                                  ContModel.Location.Substring(3, 2) + "  " +
                                                  ContModel.Location.Substring(5, 1) + "\r\n";
                }
                Blokmodel.ResultMessage += "Silakan input lokasi baru.";

            }

            return Json(Blokmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Process(BlokSystemModel model, UserData userData)
        {
            string _kodeblok = model.Blok.ToUpper() + model.Bay + model.Row + model.Tier;

            var status = false;
            try
            {
                using (var stackingService = new StackingWebService.StackingSoapClient())
                {
                    var ContIntOutXML = stackingService.FillContInOutByContainerNumber(Username, model.ContNo);
                    var ContModel = new ContInOutModel();
                    if (!string.IsNullOrEmpty(ContIntOutXML))
                    {
                        var dsContInOut = Converter.ConvertXmlToDataSet(ContIntOutXML);
                        var listContIntOut = dsContInOut.Tables[0].ToList<ContInOutModel>();
                        ContModel = listContIntOut.FirstOrDefault();

                    }
                    else
                    {
                        return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
                    }
                    if (_kodeblok.Length < 6)
                    {
                        //skm.location = "TMP";
                        status = false;
                        model.ResultMessage = "Lokasi kurang dari 6 digit";
                        return Json(new { Status = status, Message = model.ResultMessage }, JsonRequestBehavior.AllowGet);

                    }
                    // TODO: Add insert logic here

                    model.ContInOutId = ContModel.ContInOutId;
                    model.Cont = ContModel.Cont;
                    model.Shipper = ContModel.CustomerCode;
                    model.Size = ContModel.Size;
                    model.ResultMessage = ContModel.Message;
                    model.activeuser = Username;
                    model.Location = _kodeblok;
                    model.KodeBlok = _kodeblok;

                    // User Data from Login, look the step carefully.
                    model.EqpId = userData.HEID;
                    model.OPID = userData.OPID;

                    //model.BlokSystemValidate(ModelState);
                    //if (ModelState.IsValid)
                    //{

                    var kodeBlok = model.Blok.ToUpper() + model.Bay + model.Row + model.Tier;
                    model.FlagAct = "MOVE";
                    // Do Logical Process
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
                        return View(model);
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
                    //}
                    //else
                    //{
                    //    return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
                    //}
                }
                //return RedirectToAction("Index");
            }
            catch
            {

                return Json(new { Status = status, Message = model.ResultMessage }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}