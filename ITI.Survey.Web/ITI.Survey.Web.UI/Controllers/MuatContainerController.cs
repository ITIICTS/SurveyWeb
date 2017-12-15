using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AGY.Solution.Helper;

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
                using (var stackingService = new StackingWebService.StackingSoapClient())
                {
                    string xmlContCard = stackingService.FillContCardByIdAndCardMode(Username, contCardID, "OUT");
                    if (string.IsNullOrWhiteSpace(xmlContCard))
                    {
                        status = false;
                        message = "No. Kartu Muat salah";
                        errors++;
                    }

                    var dtContCard = Converter.ConvertXmlToDataTable(xmlContCard);
                    contCard = dtContCard.ToList<ContCardModel>().FirstOrDefault();

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

                                    string xmlContainers = stackingService.FillContainerDuration(Username, custDo.CustomerCode, contCard.Size, contCard.Type, "", 1, "");
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

        public string LoadContCard(string txtInfo, ContCardModel cc, CustDoModel cdo, InOutRevenueModel rev)
        {
            txtInfo = "";

            #region Load Container
            DateTime sekarang = DateTime.Now;
            DateTime dtmstartout = Convert.ToDateTime(cdo.DtmStartOut);
            if (sekarang < dtmstartout)
            {
                txtInfo = "Container tdk boleh keluar sebelum " + cdo.DtmStartOut;
                return txtInfo;
            }

            if (rev.IsCanceled == 1)
            {
                txtInfo = "OR sudah dibatalkan";
                return txtInfo;
            }

            if (rev.KasirNote.ToUpper().Contains("#NOOUT"))
            {
                txtInfo = "OR ini tidak boleh keluar";
                return txtInfo;
            }

            bool allowdm = cdo.AllowDM;
            string cdoattribs = allowdm ? " [ALLOWDM]" : "";

            txtInfo = "";

            txtInfo += cc.ContCardID.ToString() + " : " + rev.RefId.ToString() + "<br />";
            txtInfo += "*** " + cc.Cont + " --- " + cc.Size + " " + cc.Type + " ***<br />";
            txtInfo += "SEAL NUMBER: " + cc.Seal + " ***<br />";
            txtInfo += "DO NUMBER: " + cdo.DoNumber + "<br />";
            txtInfo += "CUSTOMER: " + cdo.CustomerCode + "<br />";
            txtInfo += "SHIPPER: " + cdo.Shipper + "<br />";
            txtInfo += "DESTINATION: " + cdo.DestinationName + "<br />";
            txtInfo += "VSLVOY: " + cdo.VesselVoyageName + "<br />";
            txtInfo += "REMARKS: " + cdo.Remarks + "<br />";
            txtInfo += "REMARK2: " + cdo.Remark2 + "<br />";
            txtInfo += "--------------------<br />";
            txtInfo += cdoattribs + "<br />";

            #endregion

            return txtInfo;
        }

        [HttpPost]
        public ActionResult GoLoadContainer(string cont, string cc, string cdo)
        {
            bool status = true;
            string message = string.Empty;

            ContCardModel contCard = JsonConvert.DeserializeObject<ContCardModel>(cc, JSONSetting);
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(cdo, JSONSetting);
            ContInOutModel contInOut = null;
            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                string ContNo = cont.Trim().ToUpper().Replace(" ", "");
                ContNo = ContNo.Substring(0, 4) + " " + ContNo.Substring(4, 3) + " " + ContNo.Substring(7, 3) + " " + ContNo.Substring(10, 1);

                string xmlContInOut = stackingService.FillContInOutByContainerNumber(Username, ContNo);
                if (!string.IsNullOrWhiteSpace(xmlContInOut))
                {
                    if (xmlContInOut.Contains("Error"))
                    {
                        status = false;
                        message = xmlContInOut.Replace("Error: ", "");
                    }
                    else
                    {
                        var dtContInOut = Converter.ConvertXmlToDataTable(xmlContInOut);
                        contInOut = dtContInOut.ToList<ContInOutModel>().FirstOrDefault();

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
                            }
                        }
                    }
                }
            }
            return Json(new { Status = status, Message = message, ContInOut = JsonConvert.SerializeObject(contInOut ?? new ContInOutModel()) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitKartuMuat(string skm, string cc, string cdo, string rev, string cont, UserData userData)
        {
            SubmitKartuMuatModel model = JsonConvert.DeserializeObject<SubmitKartuMuatModel>(skm, JSONSetting);
            ContCardModel contCard = JsonConvert.DeserializeObject<ContCardModel>(cc, JSONSetting);
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(cdo, JSONSetting);
            InOutRevenueModel inOutRevenue = JsonConvert.DeserializeObject<InOutRevenueModel>(rev, JSONSetting);
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
            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                string xml = Converter.ConvertToXML(model);
                message = stackingService.SubmitKartuMuat(xml);
                if (message.Contains("Error"))
                {
                    status = false;
                    message = message.Replace("Error: ", "");
                }
            }

            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GoReselectContainer(string cont, string cc, string cdo)
        {
            bool status = true;
            string message = string.Empty;

            ContCardModel contCard = JsonConvert.DeserializeObject<ContCardModel>(cc, JSONSetting);
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(cdo, JSONSetting);
            ContInOutModel contInOut = null;
            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                string ContNo = cont.Trim().ToUpper().Replace(" ", "");
                ContNo = ContNo.Substring(0, 4) + " " + ContNo.Substring(4, 3) + " " + ContNo.Substring(7, 3) + " " + ContNo.Substring(10, 1);

                string xmlContInOut = stackingService.FillContInOutByContainerNumber(Username, ContNo);
                if (!string.IsNullOrWhiteSpace(xmlContInOut))
                {
                    if (xmlContInOut.Contains("Error"))
                    {
                        status = false;
                        message = xmlContInOut.Replace("Error: ", "");
                    }
                    else
                    {
                        var dtContInOut = Converter.ConvertXmlToDataTable(xmlContInOut);
                        contInOut = dtContInOut.ToList<ContInOutModel>().FirstOrDefault();

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
                            }
                        }
                    }
                }
            }
            return Json(new { Status = status, Message = message, ContInOut = JsonConvert.SerializeObject(contInOut ?? new ContInOutModel()) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReSubmitKartuMuat(string skm, string cc, string cdo, string rev, string cont, UserData userData)
        {
            SubmitKartuMuatModel model = JsonConvert.DeserializeObject<SubmitKartuMuatModel>(skm, JSONSetting);
            ContCardModel contCard = JsonConvert.DeserializeObject<ContCardModel>(cc, JSONSetting);
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(cdo, JSONSetting);
            InOutRevenueModel inOutRevenue = JsonConvert.DeserializeObject<InOutRevenueModel>(rev, JSONSetting);
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
            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                string xml = Converter.ConvertToXML(model);
                message = stackingService.ResubmitKartuMuat(xml);
                if (message.Contains("Error"))
                {
                    status = false;
                    message = message.Replace("Error: ", "");
                }
            }

            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}