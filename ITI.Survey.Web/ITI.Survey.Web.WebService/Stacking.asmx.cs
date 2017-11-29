using ITI.Survey.Web.Dll.DAL;
using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ITI.Survey.Web.WebService
{
    /// <summary>
    /// Summary description for Stacking
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Stacking : System.Web.Services.WebService
    {
        private ContCardDAL contCardDAL = new ContCardDAL();
        private ContainerDurationDAL containerDurationDAL = new ContainerDurationDAL();
        private ContInOutDAL contInOutDAL = new ContInOutDAL();
        private BlackListDAL blackListDAL = new BlackListDAL();

        /// <summary>
        /// Old Name Method: ContCard_FillByID(string activeuser, long _id, string _cmode)
        /// Fill Container Card By Id and Card Mode
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="cardMode"></param>
        /// <returns></returns>
        [WebMethod]        
        public string FillContCardByIdAndCardMode(string userId, long id, string cardMode)
        {
            AppPrincipal.LoginForService(userId);
            ContCard contCard = new ContCard();
            contCard = contCardDAL.FillContCardByContCardIdAndCardMode(id, cardMode);
            return contCard.ContCardID > 0 ?  Converter.ConvertToXML(contCard) : string.Empty;
        }

        /// <summary>
        /// Old Name Method: WebDuration_Fill(string activeuser, string _customercode, string _contsize, string _conttype, string _condition, int _mindur, string _sortby)
        /// Fill Container Duration by Customer Code, Size, Type, Condition, Minimum Duration, Sort By
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="customerCode"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <param name="condition"></param>
        /// <param name="minDuration"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        [WebMethod]        
        public string FillContainerDuration(string userId, string customerCode, string size, string type, string condition, int minDuration, string sortBy)
        {
            AppPrincipal.LoginForService(userId);
            List<ContainerDuration> listContainerDuration = new List<ContainerDuration>();
            listContainerDuration = containerDurationDAL.FillByCriteria(customerCode, size, type, condition, minDuration, sortBy);
            return listContainerDuration.Count > 0 ? Converter.ConvertListToXML(listContainerDuration) : string.Empty;
        }

        /// <summary>
        /// Old Name Method: ContInOut_FillByID(string activeuser, long _id)
        /// Fill Container By Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contInOutId"></param>
        /// <returns></returns>
        [WebMethod]
        public string FillContInOutById(string userId, long contInOutId)
        {
            AppPrincipal.LoginForService(userId);
            ContInOut contInOut = new ContInOut();
            contInOut = contInOutDAL.FillContInOutById(contInOutId);
            contInOut.Message = blackListDAL.GetMessageByContNumber(contInOut.Cont);
            return contInOut.ContInOutId > 0 ? Converter.ConvertToXML(contInOut) : string.Empty;            
        }
    }
}
