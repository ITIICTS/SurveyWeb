using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AGY.Solution.Helper;
using ITI.Survey.Web.UI.StackingWebService;

namespace ITI.Survey.Web.UI.Controllers
{
    [Authorize]
    public class MuatContainerController : BaseController
    {
        // GET: MuatContainer
        public ActionResult Index()
        {
            return View();
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
                        string[] data = Spire.Barcode.BarcodeScanner.Scan(postedFile.InputStream);
                        result = Convert.ToInt64(data[0]);
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
        public ActionResult ProsesMuatScan(long contCardID)
        {
            string message = "Proses Muat Scan berhasil";
            bool status = true;
            int errors = 0;
            try
            {
                if (contCardID <= 0)
                {
                    status = false;
                    message = "Silakan masukkan nomor kartu muat";
                    errors++;
                }

                ContCardModel contCard = null;
                string xmlInOutRevenue = string.Empty;
                InOutRevenueModel inOutRevenue = null;
                string xmlCustDo = string.Empty;
                CustDoModel custDo = null;
                string info = string.Empty;
                IList<ContainerDurationModel> muatContainer = new List<ContainerDurationModel>();
                using (var stackingService = new StackingSoapClient())
                {
                    string xmlContCard = stackingService.FillContCardByIdAndCardMode(Username, contCardID, "OUT");
                    if (string.IsNullOrWhiteSpace(xmlContCard) || xmlContCard.Contains("Error"))
                    {
                        status = false;
                        message = ErrorMessageFromService(xmlContCard, "No. Kartu Muat salah");
                        errors++;
                    }
                    else
                    {
                        var dataTableContCard = Converter.ConvertXmlToDataTable(xmlContCard);
                        contCard = dataTableContCard.ToList<ContCardModel>().FirstOrDefault();

                        if (contCard.EirOutNo.Length > 0)
                        {
                            status = false;
                            message = "Surat jalan sudah tercetak";
                            errors++;
                        }

                        if (contCard.Loc1.Length < 2)
                        {
                            status = false;
                            message = "Kartu Muat belum scan di Gate-In";
                            errors++;
                        }

                        // Cek nomor mobil
                        if (string.IsNullOrEmpty(contCard.NoMobilOut.Trim()))
                        {
                            status = false;
                            message = "Nomor mobil belum diinput";
                            errors++;
                        }

                        // InOutRevenue
                        xmlInOutRevenue = stackingService.FillInOutRevenueByInOutRevenueId(Username, contCard.RefID);
                        if (string.IsNullOrWhiteSpace(xmlInOutRevenue))
                        {
                            status = false;
                            info = "Record pembayaran tidak ditemukan. Hubungi inventori";
                        }
                        else
                        {
                            if (xmlInOutRevenue.Contains("Error"))
                            {
                                info = xmlInOutRevenue.Replace("Error: ", "");
                            }
                            else
                            {
                                var dtInOutRevenue = Converter.ConvertXmlToDataTable(xmlInOutRevenue);
                                inOutRevenue = dtInOutRevenue.ToList<InOutRevenueModel>().FirstOrDefault();

                                // CustDo
                                xmlCustDo = stackingService.FillCustDoByCustDoId(Username, inOutRevenue.RefId);
                                if (string.IsNullOrWhiteSpace(xmlCustDo))
                                {
                                    status = false;
                                    info = "Nomor DO tidak ditemukan. Hubungi inventori";
                                }
                                else
                                {
                                    if (xmlCustDo.Contains("Error"))
                                    {
                                        info = xmlCustDo.Replace("Error: ", "");
                                    }
                                    else
                                    {
                                        var dtCustDo = Converter.ConvertXmlToDataTable(xmlCustDo);
                                        custDo = dtCustDo.ToList<CustDoModel>().FirstOrDefault();

                                        string xmlContainers = stackingService.FillContainerDuration(Username, custDo.CustomerCode, contCard.Size, contCard.Type, custDo.AllowDM ? "" : "AV", 1, "");
                                        if (!xmlContainers.Contains("Error") || !string.IsNullOrWhiteSpace(xmlContainers))
                                        {
                                            var dsContainers = Converter.ConvertXmlToDataSet(xmlContainers);
                                            muatContainer = dsContainers.Tables[0].ToList<ContainerDurationModel>();
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                if (errors > 0)
                {
                    goto ENDOFCODE;
                }
                else
                {
                    info = LoadContCard(info, contCard ?? new ContCardModel(), custDo ?? new CustDoModel(), inOutRevenue ?? new InOutRevenueModel());
                    return Json(new
                    {
                        Status = status,
                        Message = message,
                        ContCard = JsonConvert.SerializeObject(contCard ?? new ContCardModel()),
                        InOutRevenue = JsonConvert.SerializeObject(inOutRevenue ?? new InOutRevenueModel()),
                        CustDo = JsonConvert.SerializeObject(custDo ?? new CustDoModel()),
                        Info = info,
                        MuatContainers = this.RenderPartialViewToString("_DataTableMuatContainer", muatContainer)
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ProsesMuatScan -> Message: {0}", ex.Message);
                log.ErrorFormat("ProsesMuatScan -> StackTrace: {0}", ex.StackTrace);
                status = false;
                message = "Proses Muat Scan error on technical issue..";
            }


        ENDOFCODE:
            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }

        public string LoadContCard(string txtInfo, ContCardModel contCard, CustDoModel custDo, InOutRevenueModel inOutRevenue)
        {
            txtInfo = string.Empty;

            #region Load Container
            DateTime sekarang = DateTime.Now;
            DateTime dtmStartOut = Convert.ToDateTime(custDo.DtmStartOut);
            if (sekarang < dtmStartOut)
            {
                txtInfo = "Container tdk boleh keluar sebelum " + custDo.DtmStartOut;
                return txtInfo;
            }

            if (inOutRevenue.IsCanceled == 1)
            {
                txtInfo = "OR sudah dibatalkan";
                return txtInfo;
            }

            if (inOutRevenue.KasirNote.ToUpper().Contains("#NOOUT"))
            {
                txtInfo = "OR ini tidak boleh keluar";
                return txtInfo;
            }

            bool allowDm = custDo.AllowDM;
            string custDoAttributes = allowDm ? " [ALLOWDM]" : string.Empty;

            txtInfo = string.Empty;
            txtInfo += contCard.ContCardID.ToString() + " : " + inOutRevenue.RefId.ToString() + "<br />";
            txtInfo += "*** " + contCard.Cont + " --- " + contCard.Size + " " + contCard.Type + " ***<br />";
            txtInfo += "SEAL NUMBER: " + contCard.Seal + " ***<br />";
            txtInfo += "DO NUMBER: " + custDo.DoNumber + "<br />";
            txtInfo += "CUSTOMER: " + custDo.CustomerCode + "<br />";
            txtInfo += "SHIPPER: " + custDo.Shipper + "<br />";
            txtInfo += "DESTINATION: " + custDo.DestinationName + "<br />";
            txtInfo += "VSLVOY: " + custDo.VesselVoyageName + "<br />";
            txtInfo += "REMARKS: " + custDo.Remarks + "<br />";
            txtInfo += "REMARK2: " + custDo.Remark2 + "<br />";
            txtInfo += "--------------------<br />";
            txtInfo += custDoAttributes + "<br />";
            #endregion

            return txtInfo;
        }

        [HttpPost]
        public ActionResult GoLoadContainer(string cont, string stringContCard, string stringCustDo)
        {
            bool status = true;
            string message = string.Empty;
            string stringOldCont = string.Empty;

            ContCardModel contCard = JsonConvert.DeserializeObject<ContCardModel>(stringContCard, JSONSetting);
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(stringCustDo, JSONSetting);
            ContInOutModel contInOut = null;
            ContInOutModel contInOutExists = null;
            using (var stackingService = new StackingSoapClient())
            {
                string ContNo = cont.Trim().ToUpper().Replace(" ", string.Empty);
                ContNo = ContNo.Substring(0, 4) + " " + ContNo.Substring(4, 3) + " " + ContNo.Substring(7, 3) + " " + ContNo.Substring(10, 1);

                string xmlContInOut = stackingService.FillContInOutByContainerNumber(Username, ContNo);
                if (!string.IsNullOrWhiteSpace(xmlContInOut))
                {
                    if (xmlContInOut.Contains("Error"))
                    {
                        status = false;
                        message = xmlContInOut.Replace("Error: ", string.Empty);
                    }
                    else
                    {
                        var dataTableContInOut = Converter.ConvertXmlToDataTable(xmlContInOut);
                        contInOut = dataTableContInOut.ToList<ContInOutModel>().FirstOrDefault();

                        if (contInOut.CustomerCode != custDo.CustomerCode)
                        {
                            status = false;
                            message = "Container " + contInOut.Cont + " owned by " + contInOut.CustomerCode + "\r\n DO must be for " + custDo.CustomerCode;
                        }
                        else
                        {
                            if (contInOut.IsFreeUse)
                            {
                                if (DateTime.Today.Subtract(custDo.DtmDo).Days > custDo.FreeUseDays - 1)
                                {
                                    status = false;
                                    message = stackingService.PreventGateOut(Username, contCardId: contCard.ContCardID);
                                }
                            }
                            else
                            {
                                message = contInOut.Cont + "<br />";
                                message += " " + contInOut.Size + " " + contInOut.Type + " " + contInOut.Condition + " " + contInOut.Payload + " " + contInOut.Manufacture + " " + contInOut.Grade + "<br />";
                                message += "[ Duration : " + DateTime.Now.Subtract(Convert.ToDateTime(contInOut.DtmIn)).Days + " ]<br />";
                                message += "DESCRIPTION<br />";
                                message += "A. SEAL 1      : " + contCard.Seal1 + "<br />";
                                message += "B. SEAL 2      : " + contCard.Seal2 + "<br />";
                                message += "C. SEAL 3      : " + contCard.Seal3 + "<br />";
                                message += "D. SEAL 4      : " + contCard.Seal4 + "<br />";
                                message += "E. NOPOL TRUCK : " + contCard.NoMobilOut + "<br />";
                                message += "F. ANGKUTAN    : " + contCard.AngkutanOut + "<br />";

                                if (contCard.ContInOutID > 0)
                                {
                                    string xmlContInOutExists = stackingService.FillContInOutById(Username, contCard.ContInOutID);
                                    if (!xmlContInOutExists.Contains("Error") || !string.IsNullOrWhiteSpace(xmlContInOutExists))
                                    {
                                        contInOutExists = Converter.ConvertXmlToDataTable(xmlContInOutExists).ToList<ContInOutModel>().FirstOrDefault();
                                        stringOldCont = contInOutExists.Cont + "<br />";
                                        stringOldCont += "Seal No : " + contInOutExists.Seal + "<br />";
                                        stringOldCont += "Blok : " + contInOutExists.Location + "<br />";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Json(new
            {
                Status = status,
                Message = message,
                ContOld = stringOldCont,
                ContIntOutExists = contInOutExists,
                ContInOut = JsonConvert.SerializeObject(contInOut ?? new ContInOutModel())
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitKartuMuat(string stringKartuMuat, string stringContCard, string stringCustDo, string stringInOutRevenue, string cont, UserData userData)
        {
            SubmitKartuMuatModel model = JsonConvert.DeserializeObject<SubmitKartuMuatModel>(stringKartuMuat, JSONSetting);
            ContCardModel contCard = JsonConvert.DeserializeObject<ContCardModel>(stringContCard, JSONSetting);
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(stringCustDo, JSONSetting);
            InOutRevenueModel inOutRevenue = JsonConvert.DeserializeObject<InOutRevenueModel>(stringInOutRevenue, JSONSetting);
            ContInOutModel contInOut = JsonConvert.DeserializeObject<ContInOutModel>(cont, JSONSetting);

            model.ContInOutId = contInOut.ContInOutId;
            model.ContInOutId_Reselect = 0;
            model.ContCardId = contCard.ContCardID;
            model.CustDoId = custDo.CustDoId;
            model.InOutRevenueId = inOutRevenue.InOutRevenueId;
            model.Cont_NoMobilOut = contCard.NoMobilOut;
            model.ActiveUser = Username;
            model.EqpId = userData.HEID;
            model.OPID = userData.OPID;

            bool status = true;
            string message = string.Empty;
            using (var stackingService = new StackingSoapClient())
            {
                string xml = Converter.ConvertToXML(model);
                message = stackingService.SubmitKartuMuat(xml);
                if (message.Contains("Error"))
                {
                    status = false;
                    message = message.Replace("Error: ", string.Empty);
                }
            }
            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GoReselectContainer(string cont, string stringContCard, string stringCustDo)
        {
            bool status = true;
            string message = string.Empty;

            ContCardModel contCard = JsonConvert.DeserializeObject<ContCardModel>(stringContCard, JSONSetting);
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(stringCustDo, JSONSetting);
            ContInOutModel contInOut = null;
            using (var stackingService = new StackingSoapClient())
            {
                string contNo = cont.Trim().ToUpper().Replace(" ", string.Empty);
                contNo = contNo.Substring(0, 4) + " " + contNo.Substring(4, 3) + " " + contNo.Substring(7, 3) + " " + contNo.Substring(10, 1);

                string xmlContInOut = stackingService.FillContInOutByContainerNumber(Username, contNo);
                if (!string.IsNullOrWhiteSpace(xmlContInOut))
                {
                    if (xmlContInOut.Contains("Error"))
                    {
                        status = false;
                        message = xmlContInOut.Replace("Error: ", string.Empty);
                    }
                    else
                    {
                        var dataTableContInOut = Converter.ConvertXmlToDataTable(xmlContInOut);
                        contInOut = dataTableContInOut.ToList<ContInOutModel>().FirstOrDefault();

                        if (contInOut.CustomerCode != custDo.CustomerCode)
                        {
                            status = false;
                            message = "Container " + contInOut.Cont + " owned by " + contInOut.CustomerCode + "\r\n DO must be for " + custDo.CustomerCode;
                        }
                        else
                        {
                            if (contInOut.IsFreeUse)
                            {
                                if (DateTime.Today.Subtract(custDo.DtmDo).Days > custDo.FreeUseDays - 1)
                                {
                                    status = false;
                                    message = stackingService.PreventGateOut(Username, contCardId: contCard.ContCardID);
                                }
                            }
                            else
                            {
                                message = contInOut.Cont + "<br />";
                                message += " " + contInOut.Size + " " + contInOut.Type + " " + contInOut.Condition + " " + contInOut.Payload + " " + contInOut.Manufacture + " " + contInOut.Grade + "<br />";
                                message += "[ Duration : " + DateTime.Now.Subtract(Convert.ToDateTime(contInOut.DtmIn)).Days + " ]<br />";
                                message += "DESCRIPTION<br />";
                                message += "A. SEAL 1      : " + contCard.Seal1 + "<br />";
                                message += "B. SEAL 2      : " + contCard.Seal2 + "<br />";
                                message += "C. NOPOL TRUCK : " + contCard.NoMobilOut + "<br />";
                                message += "D. ANGKUTAN    : " + contCard.AngkutanOut + "<br />";
                            }
                        }
                    }
                }
            }
            return Json(new { Status = status, Message = message, ContInOut = JsonConvert.SerializeObject(contInOut ?? new ContInOutModel()) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReSubmitKartuMuat(string stringKartuMuat, string stringContCard, string stringCustDo, string stringInOutRevenue, string cont, UserData userData)
        {
            SubmitKartuMuatModel model = JsonConvert.DeserializeObject<SubmitKartuMuatModel>(stringKartuMuat, JSONSetting);
            ContCardModel contCard = JsonConvert.DeserializeObject<ContCardModel>(stringContCard, JSONSetting);
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(stringCustDo, JSONSetting);
            InOutRevenueModel inOutRevenue = JsonConvert.DeserializeObject<InOutRevenueModel>(stringInOutRevenue, JSONSetting);
            ContInOutModel contInOut = JsonConvert.DeserializeObject<ContInOutModel>(cont, JSONSetting);

            model.ContInOutId = 0;
            model.ContInOutId_Reselect = contInOut.ContInOutId;
            model.ContCardId = contCard.ContCardID;
            model.CustDoId = custDo.CustDoId;
            model.InOutRevenueId = inOutRevenue.InOutRevenueId;
            model.Cont_NoMobilOut = contCard.NoMobilOut;
            model.ActiveUser = Username;
            model.EqpId = userData.HEID;
            model.OPID = userData.OPID;

            bool status = true;
            string message = string.Empty;
            using (var stackingService = new StackingSoapClient())
            {
                string xml = Converter.ConvertToXML(model);
                message = stackingService.ResubmitKartuMuat(xml);
                if (message.Contains("Error"))
                {
                    status = false;
                    message = message.Replace("Error: ", string.Empty);
                }
            }
            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PindahkanBlokContainer(BlokSystemModel model, UserData userData)
        {
            model.BlokSystemValidate(ModelState);
            if (!ModelState.IsValid)
            {
                return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
            }

            string _kodeblok = model.Blok.ToUpper() + model.Bay + model.Row + model.Tier;

            bool status = false;
            using (var stackingService = new StackingSoapClient())
            {
                var contIntOutXML = stackingService.FillContInOutByContainerNumber(Username, model.ContNo);
                var contModel = new ContInOutModel();
                if (!string.IsNullOrEmpty(contIntOutXML))
                {
                    var dataSetContInOut = Converter.ConvertXmlToDataSet(contIntOutXML);
                    var listContIntOut = dataSetContInOut.Tables[0].ToList<ContInOutModel>();
                    contModel = listContIntOut.FirstOrDefault();

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

                model.ContInOutId = contModel.ContInOutId;
                model.Cont = contModel.Cont;
                model.Shipper = contModel.CustomerCode;
                model.Size = contModel.Size;
                model.ResultMessage = contModel.Message;
                model.ActiveUser = Username;
                model.Location = _kodeblok;
                model.KodeBlok = _kodeblok;
                // User Data from login
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
    }
}