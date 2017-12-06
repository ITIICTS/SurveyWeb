using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
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

        [HttpPost]
        public ActionResult ProsesBongkarContainerAV(BongkarContainerModel model)
        {
            string resultMsg = "";
            bool status = false;
            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                ContCardModel contCard = new ContCardModel();
                string strContCard = stackingService.FillContCardByIdAndCardMode(Username, model.ContCardID, "IN");
                if (strContCard.Length != 0)
                {
                    var dsContCard = Converter.ConvertXmlToDataSet(strContCard);
                    var listContCard = dsContCard.Tables[0].ToList<ContCardModel>();
                    contCard = listContCard.FirstOrDefault();
                }

                ContInOutModel contInOut = new ContInOutModel();
                string strContInOut = stackingService.FillContInOutById(Username, contCard.RefID);
                if (strContInOut.Length != 0)
                {
                    var dsContInOut = Converter.ConvertXmlToDataSet(strContInOut);
                    var listContInOut = dsContInOut.Tables[0].ToList<ContInOutModel>();
                }

                model.Location = model.Blok.ToUpper() + model.Bay + model.Row + model.Tier;

                model.ContInOutId = contInOut.ContInOutId;
                model.ContCardID = contCard.ContCardID;
                model.FlagAct = "BONGKAR AV";
                model.Cont = contInOut.Cont;
                model.Shipper = contInOut.CustomerCode;

                if (model.Side.Equals("Kiri") || string.IsNullOrEmpty(model.Side))
                {
                    model.Cont = contInOut.Cont;
                    model.Cont2 = "";
                }
                else
                {
                    model.Cont = "";
                    model.Cont2 = contInOut.Cont;
                }

                string message = stackingService.SubmitKartuBongkar(Converter.ConvertToXML(model));
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
            }
            return Json(new { Status = status, Message = resultMsg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BongkarContainerDM()
        {
            BongkarContainerModel model = new BongkarContainerModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProsesBongkarContainerDM(BongkarContainerModel model)
        {
            string resultMsg = "";
            bool status = false;
            using (var stackingService = new StackingWebService.StackingSoapClient())
            {
                ContCardModel contCard = new ContCardModel();
                string strContCard = stackingService.FillContCardByIdAndCardMode(Username, model.ContCardID, "IN");
                if (strContCard.Length != 0)
                {
                    var dsContCard = Converter.ConvertXmlToDataSet(strContCard);
                    var listContCard = dsContCard.Tables[0].ToList<ContCardModel>();
                    contCard = listContCard.FirstOrDefault();
                }

                ContInOutModel contInOut = new ContInOutModel();
                string strContInOut = stackingService.FillContInOutById(Username, contCard.RefID);
                if (strContInOut.Length != 0)
                {
                    var dsContInOut = Converter.ConvertXmlToDataSet(strContInOut);
                    var listContInOut = dsContInOut.Tables[0].ToList<ContInOutModel>();
                }

                model.Location = model.Blok.ToUpper() + model.Bay + model.Row + model.Tier;

                model.ContInOutId = contInOut.ContInOutId;
                model.ContCardID = contCard.ContCardID;
                model.FlagAct = "BONGKAR DM";
                model.Cont = contInOut.Cont;
                model.Shipper = contInOut.CustomerCode;

                if (model.Side.Equals("Kiri") || string.IsNullOrEmpty(model.Side))
                {
                    model.Cont = contInOut.Cont;
                    model.Cont2 = "";
                }
                else
                {
                    model.Cont = "";
                    model.Cont2 = contInOut.Cont;
                }

                string message = stackingService.SubmitKartuBongkar(Converter.ConvertToXML(model));
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
            }
            return Json(new { Status = status, Message = resultMsg }, JsonRequestBehavior.AllowGet);
        }
    }
}