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
        private InOutRevenueDAL inOutRevenueDAL = new InOutRevenueDAL();
        private CustDoDAL custDoDAL = new CustDoDAL();
        private DefinedContSizeTypeDAL definedContSizeTypeDAL = new DefinedContSizeTypeDAL();
        private DurationRuleDAL durationRuleDAL = new DurationRuleDAL();
        private DestinationRuleDAL destinationRuleDAL = new DestinationRuleDAL();

        /// <summary>
        /// Previous Name: ContCard_FillByID(string activeuser, long _id, string _cmode)
        /// Fill Container Card By Id and Card Mode
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="cardMode"></param>
        /// <returns></returns>
        [WebMethod]        
        public string FillContCardByIdAndCardMode(string userId, long id, string cardMode)
        {
            if(!AppPrincipal.LoginForService(userId))
            {
                return "Error: Tolong login terlebih dahulu";
            }
            ContCard contCard = new ContCard();
            contCard = contCardDAL.FillContCardByContCardIdAndCardMode(id, cardMode);
            return contCard.ContCardID > 0 ?  Converter.ConvertToXML(contCard) : string.Empty;
        }

        /// <summary>
        /// Previous Name: WebDuration_Fill(string activeuser, string _customercode, string _contsize, string _conttype, string _condition, int _mindur, string _sortby)
        /// Form: Container Duration
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
            if (!AppPrincipal.LoginForService(userId))
            {
                return "Error: Tolong login terlebih dahulu";
            }
            List<ContainerDuration> listContainerDuration = new List<ContainerDuration>();
            listContainerDuration = containerDurationDAL.FillByCriteria(customerCode, size, type, condition, minDuration, sortBy);
            return listContainerDuration.Count > 0 ? Converter.ConvertListToXML(listContainerDuration) : string.Empty;
        }

        /// <summary>
        /// Previous Name: ContInOut_FillByID(string activeuser, long _id)
        /// Fill Container By Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contInOutId"></param>
        /// <returns></returns>
        [WebMethod]
        public string FillContInOutById(string userId, long contInOutId)
        {
            if (!AppPrincipal.LoginForService(userId))
            {
                return "Error: Tolong login terlebih dahulu";
            }
            ContInOut contInOut = new ContInOut();
            contInOut = contInOutDAL.FillContInOutById(contInOutId);
            contInOut.Message = blackListDAL.GetMessageByContainerNumber(contInOut.Cont);
            return contInOut.ContInOutId > 0 ? Converter.ConvertToXML(contInOut) : string.Empty;            
        }

        /// <summary>
        /// Previous Name: ContInOut_FillByCont(string activeuser, string _cont)
        /// Fill ContInOut By Container Number
        /// form : Blok system
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="containerNumber"></param>
        /// <returns></returns>
        [WebMethod]
        public string FillContInOutByContainerNumber(string userId, string containerNumber)
        {
            if (!AppPrincipal.LoginForService(userId))
            {
                return "Error: Tolong login terlebih dahulu";
            }
            ContInOut contInOut = new ContInOut();
            contInOut = contInOutDAL.FillContInOutByContainerNumber(containerNumber);
            contInOut.Message = blackListDAL.GetMessageByContainerNumber(contInOut.Cont);
            return contInOut.ContInOutId > 0 ? Converter.ConvertToXML(contInOut) : string.Empty;
        }

        /// <summary>
        /// Previous Name: Submit_KartuBongkar(string xml_parameter)
        /// Form: 
        /// 1. Bongkar AV
        /// 2. Bongkar DM
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [WebMethod]
        public string SubmitKartuBongkar(string xml)
        {
            string result = string.Empty;
            DataTable dataTable = Converter.ConvertXmlToDataTable(xml);
            DataRow dataRow = dataTable.Rows[0];

            if (!AppPrincipal.LoginForService(dataRow["activeuser"].ToString()))
            {
                return "Error: Tolong login terlebih dahulu";
            }

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
        /// Previous Name: Submit_NoMobil(string xml_parameter)
        /// Form: Input Nomor Mobil
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [WebMethod]
        public string SubmitNoMobil(string xml)
        {
            string result = string.Empty;
            DataTable dataTable = Converter.ConvertXmlToDataTable(xml);
            DataRow dataRow = dataTable.Rows[0];

            if (!AppPrincipal.LoginForService(dataRow["activeuser"].ToString()))
            {
                return "Error: Tolong login terlebih dahulu";
            }

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

        /// <summary>
        /// Previous Name: Submit_BlokMove(string xml_parameter)
        /// Form: Blok System
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [WebMethod]
        public string SubmitBlokMove(string xml)
        {
            string result = string.Empty;
            DataTable dataTable = Converter.ConvertXmlToDataTable(xml);
            DataRow dataRow = dataTable.Rows[0];

            if (!AppPrincipal.LoginForService(dataRow["activeuser"].ToString()))
            {
                return "Error: Tolong login terlebih dahulu";
            }

            #region Initialize Objects
            // Initialize Blok After
            string tempLocation = dataRow["location"].ToString();
            string kodeBlok = tempLocation;

            // Initialize ContInOut
            ContInOut container = new ContInOut();
            string containerNumber = string.Empty;
            if (dataRow["cont"].ToString().Length > 0)
            {
                containerNumber = dataRow["cont"].ToString();
            }
            else if (dataRow["cont2"].ToString().Length > 0)
            {
                containerNumber = dataRow["cont2"].ToString();
            }

            container = contInOutDAL.FillContInOutByContainerNumber(containerNumber);
            if (container.ContInOutId <= 0)
            {
                result += "Error: Continout tidak ditemukan\r\n";
            }

            // Initialize Blok Before
            Blok blok1 = new Blok();
            blok1 = blokDAL.FillBlokByKode(container.Location);
            if (blok1.BlokId > 0)
            {
                blok1.Cont = string.Empty;
                blok1.Cont2 = string.Empty;
            }


            Blok blok2 = new Blok();
            if (kodeBlok != "TMP")
            {
                blok2 = blokDAL.FillBlokByKode(kodeBlok);
                if (blok2.BlokId <= 0)
                {
                    result += "Error: Blok " + kodeBlok + " tidak ditemukan\r\n";
                }
            }

            #endregion

            if (result.Length > 0)
            {
                return result;
            }

            container.Location = kodeBlok;

            string containerToBeTMPLeft = string.Empty;
            string containerToBeTMPRight = string.Empty;

            if (dataRow["cont"].ToString().Length > 0)
            {
                containerToBeTMPLeft = blok2.Cont;
                blok2.Cont = containerNumber;
                if (!container.Size.Equals("20"))
                {
                    containerToBeTMPRight = blok2.Cont2;
                    blok2.Cont2 = string.Empty;
                }
                if (blok2.Cont2.Equals(blok2.Cont))
                {
                    blok2.Cont2 = string.Empty;
                }
                else
                {
                    ContInOut contInOutToBeTmp = new ContInOut();
                    contInOutToBeTmp = contInOutDAL.FillContInOutByContainerNumber(blok2.Cont2);
                    if (!contInOutToBeTmp.Size.Equals("20"))
                    {
                        containerToBeTMPRight = blok2.Cont2;
                    }
                }
            }
            else if (dataRow["cont2"].ToString().Length > 0)
            {
                containerToBeTMPRight = blok2.Cont2;
                blok2.Cont2 = containerNumber;
                if (blok2.Cont.Equals(blok2.Cont2))
                {
                    blok2.Cont = string.Empty;
                }
                else
                {
                    ContInOut contInOutToBeTmp = new ContInOut();
                    contInOutToBeTmp = contInOutDAL.FillContInOutByContainerNumber(blok2.Cont);
                    if (!contInOutToBeTmp.Size.Equals("20"))
                    {
                        containerToBeTMPLeft = blok2.Cont;
                    }
                }
                if (!container.Size.Equals("20"))
                {
                    containerToBeTMPLeft = blok2.Cont;
                    blok2.Cont = containerNumber;
                    blok2.Cont2 = string.Empty;
                }
            }

            try
            {
                contInOutDAL.UpdateLocationToTMP(containerToBeTMPLeft, containerToBeTMPRight, container.Location);
                contInOutDAL.UpdateContInOut(container);
            }
            catch
            {
                result += "Error: Updating Continout gagal\r\n";
            }

            try
            {
                if (blok1.BlokId > 0)
                {
                    blokDAL.UpdateBlok(blok1);
                }

                if (kodeBlok != "TMP")
                {
                    if (blok2.BlokId > 0)
                    {
                        blokDAL.UpdateBlok(blok2);
                    }
                }
            }
            catch
            {
                result += "Error: Updating Blok gagal\r\n";
            }

            //Save to ContLog
            ContainerLog containerLog = new ContainerLog();
            containerLog.ContInOutId = Convert.ToInt64(dataRow["continoutid"]);
            containerLog.Cont = dataRow["cont"].ToString();
            containerLog.UserId = dataRow["activeuser"].ToString();
            containerLog.EqpId = dataRow["eqpid"].ToString();
            containerLog.FlagAct = dataRow["flagact"].ToString();
            containerLog.Location = dataRow["location"].ToString();
            containerLog.Shipper = dataRow["shipper"].ToString();
            containerLog.Operator = dataRow["opid"].ToString();

            try
            {
                containerLogDAL.InsertContainerLog(containerLog);
            }
            catch
            {
                result += "Error: Updating Log gagal\r\n";
            }

            return result;
        }

        /// <summary>
        /// Previous Name: ContInOutList_FillByContStok(string activeuser, string _cont)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="containerNumber"></param>
        /// <returns></returns>
        [WebMethod]
        public string FillContInOutByContainerForStock(string userId, string containerNumber)
        {
            if (!AppPrincipal.LoginForService(userId))
            {
                return "Error: Tolong login terlebih dahulu";
            }
            List<ContInOut> listContInOut = contInOutDAL.GetContainerStockByContainerNumber(containerNumber, true);
            return listContInOut.Count > 0 ? Converter.ConvertListToXML(listContInOut) : string.Empty;
        }

        /// <summary>
        /// Previous Name: ContInOutList_FillByBlokStok(string activeuser, string _blok)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="blok"></param>
        /// <returns></returns>
        [WebMethod]
        public string FillContInOutByBlokForStock(string userId, string blok)
        {
            if (!AppPrincipal.LoginForService(userId))
            {
                return "Error: Tolong login terlebih dahulu";
            }

            List<ContInOut> listContInOut = listContInOut = contInOutDAL.GetContainerStockByBlok(blok);
            return listContInOut.Count > 0 ? Converter.ConvertListToXML(listContInOut) : string.Empty;
        }

        /// <summary>
        /// Previous Name: InOutRevenue_FillByID(string activeuser, long _id)
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="_id"></param>
        /// <returns></returns>
        [WebMethod]
        public string FillInOutRevenueByInOutRevenueId(string userId, long inOutRevenueId)
        {
            if (!AppPrincipal.LoginForService(userId))
            {
                return "Error: Tolong login terlebih dahulu";
            }

            InOutRevenue inOutRevenue = inOutRevenueDAL.FillInOutRevenueByInOutRevenueId(inOutRevenueId);
            return inOutRevenue.InOutRevenueId > 0 ? Converter.ConvertToXML(inOutRevenue) : string.Empty;
        }

        /// <summary>
        /// Previous Name: CustDo_FillByID(string activeuser, long _id)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="custDoId"></param>
        /// <returns></returns>
        [WebMethod]
        public string FillCustDoByCustDoId(string userId, long custDoId)
        {
            if (!AppPrincipal.LoginForService(userId))
            {
                return "Error: Tolong login terlebih dahulu";
            }

            CustDo custDo = custDoDAL.FillCustDoByCustDoId(custDoId);
            return custDo.CustDoId > 0 ? Converter.ConvertToXML(custDo) : string.Empty;
        }

        /// <summary>
        /// Previous Name: Check_DefinedCondition(string activeuser, long _inoutrevenueid, long _continoutid, long _custdoid)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="inOutRevenueId"></param>
        /// <param name="contInOutId"></param>
        /// <param name="custDoId"></param>
        /// <returns></returns>
        [WebMethod]
        public string Check_DefinedCondition(string userId, long inOutRevenueId, long contInOutId, long custDoId)
        {
            string result = string.Empty;
            if (!AppPrincipal.LoginForService(userId))
            {
                return "Error: Tolong login terlebih dahulu";
            }

            InOutRevenue inOutRevenue = inOutRevenueDAL.FillInOutRevenueByInOutRevenueId(inOutRevenueId);
            if (inOutRevenue.InOutRevenueId == 0)
            {
                result += "OR tidak ditemukan\r\n";
            }
            ContInOut contInOut = contInOutDAL.FillContInOutById(contInOutId);
            if (contInOut.ContInOutId == 0)
            {
                result += "Container tidak ditemukan\r\n";
            }

            // Cek Cont Size Type
            DefinedContSizeType def = definedContSizeTypeDAL.FillDefinedContSizeTypeByContainerNumberAndInOutRevenue(contInOut.Cont, inOutRevenue);
            if (def.DefinedContSizeTypeId > 0)
            {
                if (contInOut.Size != def.Size || contInOut.Type != def.Type)
                {
                    result += "Error: Size/Type Container tidak sama dengan OR !\r\n";
                }
            }

            CustDo custDo = custDoDAL.FillCustDoByCustDoId(custDoId);

            string messageDefined = GlobalWebServiceDAL.CheckTakenDefinedContainer(custDo.CustomerCode, inOutRevenue.NoSeri, contInOut.Cont);
            if (messageDefined.Length > 0)
            {
                result += "Error: Container harus dipakai untuk OR/DO Nomor: " + messageDefined + "\r\n";
            }

            // Check Duration
            List<DurationRule> listDurationRule = durationRuleDAL.GetDurationRuleForOut(contInOut.CustomerCode, contInOut.Cont, contInOut.Size, contInOut.Type);
            DurationRule durationRule = null;
            foreach (DurationRule d in listDurationRule)
            {
                if (contInOut.ContAge < d.MinDuration)
                {
                    durationRule = d;
                    break;
                }
            }

            if (durationRule != null)
            {
                result += "Error: Duration rule : " + durationRule.CustomerCode + " " + durationRule.Cont + " " + durationRule.Size + " " + durationRule.Type + " " + durationRule.MinDuration.ToString() + " : " + contInOut.ContAge.ToString() + "\r\n";
            }

            //load black list
            contInOut.ListBlackList = blackListDAL.GetBlackListByContainerNumber(contInOut.Cont);
            List<BlackList> listBlackList2 = blackListDAL.GetBlackList2ByContainerNumber(contInOut.Cont);
            if(listBlackList2.Count>0)
            {
                contInOut.ListBlackList.AddRange(listBlackList2.ToArray());
            }

            //cek blacklist
            contInOut.DoCheckBlockOut = true;
            contInOut.UiBrokenRulesCheck();
            if (!contInOut.BrokenRulesValidate())
            {
                result += "Error: Blacklist : " + contInOut.BrokenRulesString() + "\r\n";
            }

            //cek damage (cont and refeer engine damage)
            if (custDo.AllowDM == false)
            {
                if ((contInOut.Type == "RF" || contInOut.Type == "RH") && contInOut.RfEngineCond == "DM")
                {
                    result += "Error: Reefer Engine Not AV\r\n";
                }

                if (contInOut.Condition != "AV")
                {
                    result += "Error: Container Not AV\r\n";
                }
            }

            //cek apa sudah keluar
            if (contInOut.DtmOut.Length > 0)
            {
                result += "Error: Status Container sudah keluar !\r\n";
            }

            //cek party release (termasuk disini coparn change)
            if (!inOutRevenue.TakeDef.Contains(contInOut.Cont))
            {
                List<ContInOut> listContInOutByOr = contInOutDAL.FillByNoSeriOrOut(inOutRevenue.NoSeri, contInOut.Size, contInOut.Type, inOutRevenue.TakeDef);
                int containerCountByOr = listContInOutByOr.Count;

                ContainerSpecification containerSpecification = new ContainerSpecification();
                switch (contInOut.Size)
                {
                    case "20":
                        containerSpecification.FromString(inOutRevenue.Take20);
                        break;
                    case "40":
                        containerSpecification.FromString(inOutRevenue.Take40);
                        break;
                    case "45":
                        containerSpecification.FromString(inOutRevenue.Take45);
                        break;
                }

                int cnt1 = 0;
                foreach (ContainerSpecification.ContSpecItem contSpecItem in containerSpecification)
                {
                    if (contSpecItem.ContainerType == contInOut.Type)
                    {
                        cnt1 = contSpecItem.ContainerCount;
                    }
                }

                if (cnt1 <= containerCountByOr)
                {
                    result += "Error: Container yang keluar lebih dari DO !\r\n";
                }

            }

            //cek duration rule
            //destrule load
            List<DestinationRule> listDestinationRule = new List<DestinationRule>();
            try
            {
                listDestinationRule = destinationRuleDAL.FillForOut(contInOut.CustomerCode, contInOut.Cont, contInOut.Size, contInOut.Type, custDo.DestinationName);
            }
            catch (Exception ex)
            {
                result += "Error: " + ex.Message + ", System Error\r\n";
            }

            int countDenied = listDestinationRule.FindAll(d => d.KodeDeny.Length > 0).Count();
            int countAllow = listDestinationRule.FindAll(d => d.KodeAllow.Length > 0).Count();
            if (countDenied > 0 || countAllow > 0)
            {
                result += "Error: Diprotek oleh Destination Rule! \r\n";
            }
            return result;
        }

    }
}
