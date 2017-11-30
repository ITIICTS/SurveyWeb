using ITI.Survey.Web.Dll.DAL;
using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using System;
using System.Collections.Generic;
using System.Data;
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
        private BlokDAL blokDAL = new BlokDAL();
        private ContainerLogDAL containerLogDAL = new ContainerLogDAL();
        private TruckInDepoDAL truckInDepoDAL = new TruckInDepoDAL();

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

        /// <summary>
        /// Old Name Method: Submit_KartuBongkar(string xml_parameter)
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [WebMethod]
        public string SubmitKartuBongkar(string xml)
        {
            string result = string.Empty;
            DataTable dataTable = Converter.ConvertXmlToDataTable(xml);
            DataRow dataRow = dataTable.Rows[0];

            AppPrincipal.LoginForService(dataRow["activeuser"].ToString());

            #region Initialize Objects
            // Initialize ContCard
            ContCard containerCard = contCardDAL.FillContCardByContCardId(Convert.ToInt64(dataRow["contcardid"]));
            if (containerCard.ContCardID <= 0)
            {
                result += "Error: Data kartu bongkar tidak ditemukan\r\n";
            }

            // Initialize ContInOut
            ContInOut contInOut = contInOutDAL.FillContInOutById(Convert.ToInt64(dataRow["continoutid"]));
            if (contInOut.ContInOutId<=0)
            {
                result += "Error: Data container tidak ditemukan\r\n";
            }

            // Initialize Blok
            Blok blok = new Blok();
            if (dataRow["location"].ToString() != "TMP")
            {
                blok = blokDAL.FillBlokByKode(dataRow["location"].ToString());
                if (blok.BlokId <= 0)
                {
                    result += "Error: Blok tidak ditemukan\r\n";
                }
            }
            #endregion

            if (result.Length > 0)
            {
                return result;
            }

            containerCard.ContInOutID = Convert.ToInt64(dataRow["continoutid"]);
            containerCard.UserID3 = dataRow["activeuser"].ToString();
            containerCard.Dtm3 = GlobalWebServiceDAL.GetServerDtm(); // .ToString("yyyy-MM-dd HH:mm:ss")
            try
            {
                contCardDAL.UpdateContCard(containerCard);
                result += "# " + dataRow["flagact"].ToString() + " " + containerCard.Cont + " BERHASIL\r\n";
            }
            catch
            {
                result += "Error: Gagal melakukan pengkinian data untuk kartu bongkar\r\n";
            }

            if (result.Contains("Error"))
            {
                return result;
            }

            //Save to ContLog
            ContainerLog containerLog = new ContainerLog();
            containerLog.ContInOutId = Convert.ToInt64(dataRow["continoutid"]);
            containerLog.Cont = dataRow["cont"].ToString();
            containerLog.UserId = dataRow["activeuser"].ToString();
            containerLog.EqpId= dataRow["eqpid"].ToString();
            containerLog.FlagAct = dataRow["flagact"].ToString();
            containerLog.Location = dataRow["location"].ToString();
            containerLog.Shipper = dataRow["shipper"].ToString();
            containerLog.Operator = dataRow["opid"].ToString();

            contInOut.Location = dataRow["location"].ToString();
            string containerToBeTMPLeft = string.Empty;
            string containerToBeTMPRight = string.Empty;

            if (!string.IsNullOrEmpty(dataRow["cont"].ToString()))
            {
                containerToBeTMPLeft = blok.Cont;
                blok.Cont = dataRow["cont"].ToString();
                if (!contInOut.Size.Equals("20"))
                {
                    containerToBeTMPRight = blok.Cont2;
                }

                if (blok.Cont2.Equals(blok.Cont))
                {
                    blok.Cont2 = string.Empty;
                }
                else
                {
                    ContInOut contInOutToBeTmp = new ContInOut();
                    contInOutToBeTmp = contInOutDAL.FillContInOutByContainerNumber(blok.Cont2);
                    if (!contInOutToBeTmp.Size.Equals("20"))
                    {
                        containerToBeTMPRight = blok.Cont2;
                        blok.Cont2 = string.Empty;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dataRow["cont2"].ToString()))
            {
                containerToBeTMPRight = blok.Cont2;
                blok.Cont2 = dataRow["cont2"].ToString();
                if (!contInOut.Size.Equals("20"))
                {
                    containerToBeTMPLeft = blok.Cont;
                    blok.Cont = blok.Cont2;
                    blok.Cont2 = string.Empty;
                }

                if (blok.Cont2.Equals(blok.Cont))
                {
                    blok.Cont = string.Empty;
                }
                else
                {
                    ContInOut contInOutToBeTmp = new ContInOut();
                    contInOutToBeTmp = contInOutDAL.FillContInOutByContainerNumber(blok.Cont);
                    if (!contInOutToBeTmp.Size.Equals("20"))
                    {
                        containerToBeTMPLeft = blok.Cont;
                        blok.Cont = string.Empty;
                    }
                }
            }

            try
            {
                containerLogDAL.InsertContainerLog(containerLog);
            }
            catch
            {
                result += "Error: Updating Log gagal\r\n";
            }

            // Update Container
            try
            {
                contInOutDAL.UpdateLocationToTMP(containerToBeTMPLeft, containerToBeTMPRight, contInOut.Location);
                contInOutDAL.UpdateContInOut(contInOut);
            }
            catch
            {
                result += "Error: Updating Continout gagal\r\n";
            }

            // Update Blok
            try
            {
                if (dataRow["location"].ToString() != "TMP")
                {
                    blokDAL.UpdateBlok(blok);
                }
            }
            catch
            {
                result += "Error: Updating Blok gagal\r\n";
            }

            result += contInOut.Cont + ": Unload OK on Blok " + contInOut.Location;
            return result;
        }

        /// <summary>
        /// Old Name Method: Submit_NoMobil(string xml_parameter)
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [WebMethod]
        public string SubmitNoMobil(string xml)
        {
            string result = string.Empty;
            DataTable dataTable = Converter.ConvertXmlToDataTable(xml);
            DataRow dataRow = dataTable.Rows[0];

            AppPrincipal.LoginForService(dataRow["activeuser"].ToString());

            #region Initialize Objects

            // Initialize ContCard
            ContCard contCard = new ContCard();
            contCard = contCardDAL.FillContCardByContCardIdAndCardMode(Convert.ToInt64(dataRow["contcardid"]), dataRow["flagact"].ToString());
            if (contCard.ContCardID <= 0)
            {
                result += "Error: Contcard #" + dataRow["contcardid"] + " tidak ditemukan\r\n";
            }
            else if (!contCard.Dtm1.HasValue)
            {
                result += "Error: Belum Scan Gate In\r\n";
            }
            #endregion

            if (result.Length > 0)
            {
                return result;
            }

            // Cek nomor mobil
            string nomorMobil = dataRow["nomobil"].ToString().ToUpper().Replace(" ", string.Empty);
            char[] angka = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int i1 = nomorMobil.IndexOfAny(angka);
            int i2 = nomorMobil.LastIndexOfAny(angka);
            if (i2 >= 0)
            {
                nomorMobil = nomorMobil.Insert(i2 + 1, " ");
            }
            if (i1 >= 0)
            {
                nomorMobil = nomorMobil.Insert(i1, " ");
            }

            if (contCard.Loc2.Length > 0)
            {
                result += "Error: Kartu sudah pernah digunakan.\r\n";
                return result;
            }

            contCard.NoMobilOut = nomorMobil;
            contCard.AngkutanOut = dataRow["angkutan"].ToString();
            contCard.IsCombo = Convert.ToBoolean(dataRow["IsCombo"]);

            try
            {
                contCardDAL.UpdateContCard(contCard);
            }
            catch
            {
                result += "Error: Gagal melakukan update kartu.\r\n";
                return result;
            }

            TruckInDepo truckInDepo = new TruckInDepo();
            truckInDepo.NoMobil = nomorMobil;
            truckInDepo.Angkutan = dataRow["angkutan"].ToString();
            truckInDepo.DtmIn = GlobalWebServiceDAL.GetServerDtm();

            try
            {
                truckInDepoDAL.InsertTruckInDepo(truckInDepo);                
            }
            catch
            {
                result += "Error: Gagal melakukan update pada sebagian data nomor mobil truk.\r\n";
            }

            result += "Nomor Mobil " + nomorMobil + " Angkutan " + dataRow["angkutan"].ToString() + " Update OK.";

            return result;
        }
    }
}
