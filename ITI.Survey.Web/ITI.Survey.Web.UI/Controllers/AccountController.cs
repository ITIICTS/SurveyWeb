using AGY.Solution.DataAccess;
using AGY.Solution.Helper.Common;
using ITI.Survey.Web.UI.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ITI.Survey.Web.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private string GetReturnUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }
            else
            {
                return Url.Action("Index", "Home");
            }
        }

        [NonAction]
        public void GetConfigXML()
        {
            using (var globalService = new GlobalWebService.GlobalSoapClient())
            {
                var ds = Converter.ConvertXmlToDataSet(globalService.GetConfigXML("agys"));
                ViewBag.HeavyEquipmentList = GetDropListByDataTable(ds.Tables["dt1"], "PILIH HEID");
                ViewBag.ContainerSize = GetDropListByDataTable(ds.Tables["dt2"]);
                ViewBag.ContainerType = GetDropListByDataTable(ds.Tables["dt3"]);
                ViewBag.Customers = GetDropListByDataTable(ds.Tables["dt4"]);
                ViewBag.Operators = GetDropListByDataTable(ds.Tables["dt5"], "PILIH OPID");
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            GetConfigXML();

            LoginModel model = new LoginModel();
            return View(model);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetDropListByDataTable(DataTable dt, string optionLabel = "")
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new SelectListItem
                {
                    Value = ConvertData.ToString(dr, "Key"),
                    Text = string.IsNullOrWhiteSpace(ConvertData.ToString(dr, "Key")) ? optionLabel : ConvertData.ToString(dr, "Key")
                });
            }

            return result.OrderBy(x => x.Value);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            model.Validate(ModelState);
            if (!ModelState.IsValid)
            {
                return Json(new { errorList = GetErrorList(ModelState) }, JsonRequestBehavior.AllowGet);
            }

            bool statusLogin = false;
            ViewBag.Errors = "Your username or password is not valid.";

            using (GlobalWebService.GlobalSoapClient GlobalService = new GlobalWebService.GlobalSoapClient())
            {
                if (GlobalService.Login(model.UserId, model.Password))
                {
                    UserData userData = new UserData()
                    {
                        HEID = model.HE,
                        OPID = model.OPID
                    };

                    Response.SetAuthCookie(model.UserId, model.RememberMe, userData);

                    statusLogin = true;
                }
            }

            if (statusLogin)
            {
                return Json(new { Status = true, ReturnUrl = GetReturnUrl(returnUrl) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Status = false, ErrorMessage = ViewBag.Errors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            string ipAddress = HttpContext.Request.UserHostAddress;
            string userLogin = HttpContext.User.Identity.Name;

            //Session.Clear();
            //System.Web.HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();

            return Json(Url.Action("Index", "Home"), JsonRequestBehavior.AllowGet);
        }

        public static IEnumerable<object> GetErrorList(ModelStateDictionary modelState)
        {
            var listError =
                modelState.Select(c => new { c, firstOrDefault = c.Value.Errors.FirstOrDefault() }).Where(
                    @t => @t.firstOrDefault != null).
                    Select(@t => new
                    {
                        @t.c.Key,
                        @t.firstOrDefault.ErrorMessage
                    });

            return listError;
        }
    }
}