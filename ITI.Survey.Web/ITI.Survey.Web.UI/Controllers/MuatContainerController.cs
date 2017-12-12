using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                    return Json(new
                    {
                        Status = status,
                        Message = message,
                        ContCard = JsonConvert.SerializeObject(contCard ?? new ContCardModel()),
                        InOutRevenue = JsonConvert.SerializeObject(inOutRevenue ?? new InOutRevenueModel()),
                        CustDo = JsonConvert.SerializeObject(custDo ?? new CustDoModel()),
                        Info = info
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

        [HttpPost]
        public ActionResult GoLoadContainer(string cont, string cc, string cdo)
        {
            bool status = true;
            string message = string.Empty;

            ContCardModel contCard = JsonConvert.DeserializeObject<ContCardModel>(cc, JSONSetting);
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(cdo, JSONSetting);
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
                        ContInOutModel contInOut = dtContInOut.ToList<ContInOutModel>().FirstOrDefault();

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
                                message = contInOut.Cont + "\r\n";
                                message += " " + contInOut.Size + " " + contInOut.Type + " " + contInOut.Condition + " " + contInOut.Payload + " " + contInOut.Manufacture + " " + contInOut.Grade + "\r\n";

                                message += "[ Duration : " + DateTime.Now.Subtract(Convert.ToDateTime(contInOut.DtmIn)).Days + " ]\r\n";
                                message += "DESCRIPTION\r\n";
                                message += "A. SEAL 1      : " + contCard.Seal1 + "\r\n";
                                message += "B. SEAL 2      : " + contCard.Seal2 + "\r\n";
                                message += "C. SEAL 3      : " + contCard.Seal3 + "\r\n";
                                message += "D. SEAL 4      : " + contCard.Seal4 + "\r\n";
                                message += "E. NOPOL TRUCK : " + contCard.NoMobilOut + "\r\n";
                                message += "F. ANGKUTAN    : " + contCard.AngkutanOut + "\r\n";
                            }
                        }
                    }
                }
            }
            return Json(new { Status = status, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GoReselectContainer(string cont, string cdo)
        {
            CustDoModel custDo = JsonConvert.DeserializeObject<CustDoModel>(cdo, JSONSetting);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}