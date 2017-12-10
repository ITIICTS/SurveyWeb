using ITI.Survey.Web.Dll.DAL;
using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
        #region Fields
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        private NoMobilOutSpecialMessageDAL noMobilOutSpecialMessageDAL = new NoMobilOutSpecialMessageDAL();
        #endregion

        #region Private Method
        private string CheckSeal(ContInOut contInOut)
        {
            string message = string.Empty;
            try
            {
                // Check Seal
                string[] splits = contInOut.Seal.Split(",".ToCharArray());
                SealRegisterDAL sealRegisterDAL = new SealRegisterDAL();
                SealDestinationDAL sealDestinationDAL = new SealDestinationDAL();
                SealDestinationItemDAL sealDestinationItemDAL = new SealDestinationItemDAL();
                CustDoDefinedSealDAL custDoDefinedSealDAL = new CustDoDefinedSealDAL();
                ContOutSealDAL contOutSealDAL = new ContOutSealDAL();
                foreach (string seal in splits)
                {
                    if (seal.EndsWith("."))
                    {
                        continue;
                    }
                    string sealDestinationCode = sealRegisterDAL.GetSealDestinationCodeBySealAndCustomerCode(seal, contInOut.CustomerCode);
                    if (string.IsNullOrEmpty(sealDestinationCode))
                    {
                        message = seal + " : seal not registered!";
                        return message;
                    }
                    else
                    {
                        // Seal Destination
                        if (sealDestinationCode.Length > 0)
                        {
                            long sealDestinationId = sealDestinationDAL.GetSealDestinationIdByDestinationCode(sealDestinationCode);
                            if (sealDestinationId > 0)
                            {
                                int countSealDestinationItem = sealDestinationItemDAL.CountSealDestinationItemByDestinationCodeAndSealDestinationId(contInOut.DestinationName, sealDestinationId);
                                if (countSealDestinationItem == 0)
                                {
                                    message = seal + " : destination not allowed !";
                                    return message;
                                }
                            }
                        }

                        // Cek defined seal
                        if (!custDoDefinedSealDAL.IsSealExistInCustDoDefinedSeal(contInOut.ContInOutId, seal))
                        {
                            message = seal + " : not listed in defined seals !";
                            return message;
                        }
                    }

                    // Check Seal Used
                    string usedSealContainer = contOutSealDAL.CheckSealUsedByOtherContainer(seal, contInOut.CustomerCode);
                    if (usedSealContainer.Length > 0 && !usedSealContainer.Equals(contInOut.Cont))
                    {
                        message = string.Format("{0} used by {1}!!", seal, usedSealContainer);
                        return message;
                    }
                }
            }
            catch
            {
                message = "Seal check error";
            }
            return message;
        }

        private string CheckSeal(List<ContCard> listContCard, string seal, ContCard contCard)
        {
            string message = string.Empty;
            foreach (ContCard card in listContCard)
            {
                if (card.ContCardID == contCard.ContCardID)
                {
                    continue;
                }
                if ((card.Seal1.Length > 0 && contCard.Seal.Contains(card.Seal1))
                    || (card.Seal2.Length > 0 && contCard.Seal.Contains(card.Seal2))
                    || (card.Seal3.Length > 0 && contCard.Seal.Contains(card.Seal3))
                    || (card.Seal4.Length > 0 && contCard.Seal.Contains(card.Seal4)))
                {
                    message += "Seal " + seal + " is used by load card " + card.ContCardID;
                    break;
                }
            }
            return message;
        }
        #endregion

        #region Web Method
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
            try
            {
                if (!AppPrincipal.LoginForService(userId))
                {
                    return "Error: Tolong login terlebih dahulu";
                }
                ContCard contCard = new ContCard();
                contCard = contCardDAL.FillContCardByContCardIdAndCardMode(id, cardMode);
                return contCard.ContCardID > 0 ? Converter.ConvertToXML(contCard) : string.Empty;

            }
            catch (Exception ex)
            {
                log.ErrorFormat("FillContCardByIdAndCardMode -> Parameter: userid={0},id={1},cardMode={2}", userId, id, cardMode);
                log.ErrorFormat("FillContCardByIdAndCardMode -> Message: {0}", ex.Message);
                log.ErrorFormat("FillContCardByIdAndCardMode -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat memproses kartu: " + id;
            }

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
            try
            {
                if (!AppPrincipal.LoginForService(userId))
                {
                    return "Error: Tolong login terlebih dahulu";
                }
                List<ContainerDuration> listContainerDuration = new List<ContainerDuration>();
                listContainerDuration = containerDurationDAL.FillByCriteria(customerCode, size, type, condition, minDuration, sortBy);
                return listContainerDuration.Count > 0 ? Converter.ConvertListToXML(listContainerDuration) : string.Empty;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("FillContainerDuration -> Parameter: userid={0},customerCode={1},size={2},type={3},condition={4},minDuration={5},sortBy={6}",
                                                            userId, customerCode, size, type, condition, minDuration, sortBy);
                log.ErrorFormat("FillContainerDuration -> Message: {0}", ex.Message);
                log.ErrorFormat("FillContainerDuration -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat menampilkan data container";
            }
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
            try
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
            catch (Exception ex)
            {
                log.ErrorFormat("FillContInOutById -> Parameter: userId={0},contInOutId={1}", userId, contInOutId);
                log.ErrorFormat("FillContInOutById -> Message: {0}", ex.Message);
                log.ErrorFormat("FillContInOutById -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat menampilkan data container";
            }
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
            try
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
            catch (Exception ex)
            {
                log.ErrorFormat("FillContInOutByContainerNumber -> Parameter: userId={0},containerNumber={1}", userId, containerNumber);
                log.ErrorFormat("FillContInOutByContainerNumber -> Message: {0}", ex.Message);
                log.ErrorFormat("FillContInOutByContainerNumber -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat menampilkan data container: " + containerNumber;
            }

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
            try
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
                if (contInOut.ContInOutId <= 0)
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

                #region Update Container Card
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
                catch (Exception ex)
                {
                    log.ErrorFormat("SubmitKartuBongkar - Update Container Card -> Message: {0}", ex.Message);
                    log.ErrorFormat("SubmitKartuBongkar - Update Container Card -> StackTrace: {0}", ex.StackTrace);
                    result += "Error: Gagal melakukan pengkinian data untuk kartu bongkar\r\n";
                }

                if (result.Contains("Error"))
                {
                    return result;
                }
                #endregion

                #region Insert Log
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
                containerLog.Dtm = GlobalWebServiceDAL.GetServerDtm().ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
                try
                {
                    containerLogDAL.InsertContainerLog(containerLog);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("SubmitKartuBongkar - Update Container Log-> Message: {0}", ex.Message);
                    log.ErrorFormat("SubmitKartuBongkar - Update Container Log -> StackTrace: {0}", ex.StackTrace);
                    result += "Error: Updating Log gagal\r\n";
                }
                #endregion

                #region Update Container and Blok
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
                    contInOutDAL.UpdateLocationToTMP(containerToBeTMPLeft, containerToBeTMPRight, contInOut.Location);
                    contInOutDAL.UpdateContInOut(contInOut);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("SubmitKartuBongkar - Update ContInOut -> Message: {0}", ex.Message);
                    log.ErrorFormat("SubmitKartuBongkar - Update ContInOut -> StackTrace: {0}", ex.StackTrace);
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
                catch (Exception ex)
                {
                    log.ErrorFormat("SubmitKartuBongkar - Update Blok -> Message: {0}", ex.Message);
                    log.ErrorFormat("SubmitKartuBongkar - Update Blok -> StackTrace: {0}", ex.StackTrace);
                    result += "Error: Updating Blok gagal\r\n";
                }
                #endregion

                result += contInOut.Cont + ": Unload OK on Blok " + contInOut.Location;
                return result;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("SubmitKartuBongkar -> Parameter: xml={0}", xml);
                log.ErrorFormat("SubmitKartuBongkar -> Message: {0}", ex.Message);
                log.ErrorFormat("SubmitKartuBongkar -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat melakukan submit kartu bongkar";
            }
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
            try
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

                #region Checking Nomor Mobil
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
                #endregion

                #region Update Container Card
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
                catch (Exception ex)
                {
                    log.ErrorFormat("SubmitNoMobil - Update Cont Card -> Message: {0}", ex.Message);
                    log.ErrorFormat("SubmitNoMobil - Update Cont Card -> StackTrace: {0}", ex.StackTrace);
                    result += "Error: Gagal melakukan update kartu.\r\n";
                    return result;
                }
                #endregion

                #region Update Truck In Depo
                TruckInDepo truckInDepo = new TruckInDepo();
                truckInDepo.NoMobil = nomorMobil;
                truckInDepo.Angkutan = dataRow["angkutan"].ToString();
                truckInDepo.DtmIn = GlobalWebServiceDAL.GetServerDtm();

                try
                {
                    truckInDepoDAL.InsertTruckInDepo(truckInDepo);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("SubmitNoMobil - Update Truck In Depo -> Message: {0}", ex.Message);
                    log.ErrorFormat("SubmitNoMobil - Update Truck In Depo -> StackTrace: {0}", ex.StackTrace);
                    result += "Error: Gagal melakukan update pada sebagian data nomor mobil truk.\r\n";
                }
                #endregion

                result += "Nomor Mobil " + nomorMobil + " Angkutan " + dataRow["angkutan"].ToString() + " Update OK.";

                return result;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("SubmitNoMobil -> Parameter: xml={0}", xml);
                log.ErrorFormat("SubmitNoMobil -> Message: {0}", ex.Message);
                log.ErrorFormat("SubmitNoMobil -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat melakukan submit nomor mobil";
            }
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
            try
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

                #region Update Blok
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
                catch (Exception ex)
                {
                    log.ErrorFormat("SubmitBlokMove - Update ContInOut -> Message: {0}", ex.Message);
                    log.ErrorFormat("SubmitBlokMove - Update ContInOut -> StackTrace: {0}", ex.StackTrace);
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
                catch (Exception ex)
                {
                    log.ErrorFormat("SubmitBlokMove - Update Blok -> Message: {0}", ex.Message);
                    log.ErrorFormat("SubmitBlokMove - Update Blok -> StackTrace: {0}", ex.StackTrace);
                    result += "Error: Updating Blok gagal\r\n";
                }
                #endregion

                #region Insert Log
                ContainerLog containerLog = new ContainerLog();
                containerLog.ContInOutId = Convert.ToInt64(dataRow["continoutid"]);
                containerLog.Cont = dataRow["cont"].ToString();
                containerLog.UserId = dataRow["activeuser"].ToString();
                //containerLog.EqpId = dataRow["eqpid"].ToString();
                containerLog.FlagAct = dataRow["flagact"].ToString();
                containerLog.Location = dataRow["location"].ToString();
                containerLog.Shipper = dataRow["shipper"].ToString();
                //containerLog.Operator = dataRow["opid"].ToString();
                try
                {
                    containerLogDAL.InsertContainerLog(containerLog);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("SubmitBlokMove - Update ContainerLog -> Message: {0}", ex.Message);
                    log.ErrorFormat("SubmitBlokMove - Update ContainerLog -> StackTrace: {0}", ex.StackTrace);
                    result += "Error: Updating Log gagal\r\n";
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("SubmitBlokMove -> Parameter: xml={0}", xml);
                log.ErrorFormat("SubmitBlokMove -> Message: {0}", ex.Message);
                log.ErrorFormat("SubmitBlokMove -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat melakukan submit blok";
            }
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
            try
            {
                if (!AppPrincipal.LoginForService(userId))
                {
                    return "Error: Tolong login terlebih dahulu";
                }
                List<ContInOut> listContInOut = contInOutDAL.GetContainerStockByContainerNumber(containerNumber, true);
                return listContInOut.Count > 0 ? Converter.ConvertListToXML(listContInOut) : string.Empty;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("FillContInOutByContainerForStock -> Parameter: userId={0},containerNumber={1}", userId, containerNumber);
                log.ErrorFormat("FillContInOutByContainerForStock -> Message: {0}", ex.Message);
                log.ErrorFormat("FillContInOutByContainerForStock -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat menampilkan data container berdasarkan nomor container";
            }
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
            try
            {
                if (!AppPrincipal.LoginForService(userId))
                {
                    return "Error: Tolong login terlebih dahulu";
                }

                List<ContInOut> listContInOut = listContInOut = contInOutDAL.GetContainerStockByBlok(blok);
                return listContInOut.Count > 0 ? Converter.ConvertListToXML(listContInOut) : string.Empty;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("FillContInOutByBlokForStock -> Parameter: userId={0},blok={1}", userId, blok);
                log.ErrorFormat("FillContInOutByBlokForStock -> Message: {0}", ex.Message);
                log.ErrorFormat("FillContInOutByBlokForStock -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat menampilkan data container berdasarkan blok";
            }
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
            try
            {
                if (!AppPrincipal.LoginForService(userId))
                {
                    return "Error: Tolong login terlebih dahulu";
                }

                InOutRevenue inOutRevenue = inOutRevenueDAL.FillInOutRevenueByInOutRevenueId(inOutRevenueId);
                return inOutRevenue.InOutRevenueId > 0 ? Converter.ConvertToXML(inOutRevenue) : string.Empty;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("FillInOutRevenueByInOutRevenueId -> Parameter: userId={0},inOutRevenueId={1}", userId, inOutRevenueId);
                log.ErrorFormat("FillInOutRevenueByInOutRevenueId -> Message: {0}", ex.Message);
                log.ErrorFormat("FillInOutRevenueByInOutRevenueId -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat menampilkan data inoutrevenue";
            }
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
            try
            {
                if (!AppPrincipal.LoginForService(userId))
                {
                    return "Error: Tolong login terlebih dahulu";
                }

                CustDo custDo = custDoDAL.FillCustDoByCustDoId(custDoId);
                return custDo.CustDoId > 0 ? Converter.ConvertToXML(custDo) : string.Empty;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("FillCustDoByCustDoId -> Parameter: userId={0},custDoId={1}", userId, custDoId);
                log.ErrorFormat("FillCustDoByCustDoId -> Message: {0}", ex.Message);
                log.ErrorFormat("FillCustDoByCustDoId -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat menampilkan data customer DO";
            }
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
        public string CheckDefinedCondition(string userId, long inOutRevenueId, long contInOutId, long custDoId)
        {
            try
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
                if (listBlackList2.Count > 0)
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
                    log.ErrorFormat("CheckDefinedCondition - Duration Rule -> Message: {0}", ex.Message);
                    log.ErrorFormat("CheckDefinedCondition - Duration Rule -> StackTrace: {0}", ex.StackTrace);
                    result += "Error: Terjadi kesalahan pada saat pengecekan Destination Rule, System Error! \r\n";
                }

                int countDenied = listDestinationRule.FindAll(d => d.KodeDeny.Length > 0).Count();
                int countAllow = listDestinationRule.FindAll(d => d.KodeAllow.Length > 0).Count();
                if (countDenied > 0 || countAllow > 0)
                {
                    result += "Error: Diprotek oleh Destination Rule! \r\n";
                }
                return result;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("CheckDefinedCondition -> Parameter: userId={0},inOutRevenueId={1},contInOutId={2},custDoId={3}", 
                                userId, inOutRevenueId, contInOutId, custDoId);
                log.ErrorFormat("CheckDefinedCondition -> Message: {0}", ex.Message);
                log.ErrorFormat("CheckDefinedCondition -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat melakukan pengecekan kondisi";
            }
        }

        /// <summary>
        /// Previous Name: PreventGateOut(string activeUser, long contCardId)
        /// Prevent Gate Out
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contCardId"></param>
        /// <returns></returns>
        [WebMethod]
        public string PreventGateOut(string userId, long contCardId)
        {
            try
            {
                if (!AppPrincipal.LoginForService(userId))
                {
                    return "Error: Tolong login terlebih dahulu";
                }
                ContCard contCard = contCardDAL.FillContCardByContCardId(contCardId);
                if (contCard.ContCardID > 0)
                {
                    InOutRevenue inOutRevenue = inOutRevenueDAL.FillInOutRevenueByContCard(contCard);
                    if (!inOutRevenue.KasirNote.ToUpper().Contains(GlobalConstant.FLAG_NO_OUT))
                    {
                        inOutRevenueDAL.SetPreventGateOut(inOutRevenue.InOutRevenueId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("PreventGateOut -> Parameter: userId={0},contCardId={1}",userId, contCardId);
                log.ErrorFormat("PreventGateOut -> Message: {0}", ex.Message);
                log.ErrorFormat("PreventGateOut -> StackTrace: {0}", ex.StackTrace);
                return "Failed";
            }
            return "Successed";
        }

        /// <summary>
        /// Previous Name: Submit_KartuMuat(string xml_parameter)
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [WebMethod]
        public string SubmitKartuMuat(string xml)
        {
            try
            {
                //return xml_parameter;
                string result = string.Empty;
                DataTable dataTable = Converter.ConvertXmlToDataTable(xml);
                DataRow dataRow = dataTable.Rows[0];

                if (!AppPrincipal.LoginForService(dataRow["activeuser"].ToString()))
                {
                    return "Error: Tolong login terlebih dahulu";
                }

                #region Initialize Objects

                // Initialize InOutRevenue
                InOutRevenue inOutRevenue = inOutRevenueDAL.FillInOutRevenueByInOutRevenueId(Convert.ToInt64(dataRow["inoutrevenueid"]));
                if (inOutRevenue.InOutRevenueId <= 0)
                {
                    result += "Error: OR tidak ditemukan\r\n";
                }

                // Initialize ContInOut
                ContInOut contInOut = contInOutDAL.FillContInOutById(Convert.ToInt64(dataRow["continoutid"]));
                if (contInOut.ContInOutId <= 0)
                {
                    result += "Error: Cont tidak ditemukan\r\n";
                }

                // Initialize CustDo
                CustDo custDo = custDoDAL.FillCustDoByCustDoId(Convert.ToInt64(dataRow["custdoid"]));
                if (custDo.CustDoId <= 0)
                {
                    result += "Error: DO tidak ditemukan\r\n";
                }

                // Initialize ContCard
                ContCard contCard = contCardDAL.FillContCardByContCardId(Convert.ToInt64(dataRow["contcardid"]));
                if (contCard.ContCardID <= 0)
                {
                    result += "Error: Contcard tidak ditemukan\r\n";
                }

                ContCard contCardByContainer = contCardDAL.FillContCardByContInOutIdAndCardMode(contInOut.ContInOutId, "OUT");
                if (contCard.ContInOutID > 0 && contCardByContainer.ContInOutID > 0
                    && contCard.ContInOutID == contCardByContainer.ContInOutID
                    && contCard.ContCardID != contCardByContainer.ContCardID)
                {
                    result += "Error: Container = " + contInOut.Cont + " sudah dimuat di nomor contcard " + contCardByContainer.ContCardID + ".\r\n";
                }
                #endregion

                if (result.Length > 0)
                {
                    return result;
                }

                #region Cek Seal and Get Seal error message
                contInOut.Seal = dataRow["cont_Seal"].ToString();
                // Check Seal
                string sealMessage = CheckSeal(contInOut);
                if (sealMessage.Length > 0)
                {
                    result += string.Format("Error: Seal={0} \r\n", sealMessage);
                }
                #endregion

                #region Check Double Definition for Seal in other cards
                List<ContCard> listContCard = contCardDAL.FillContCardByRefIdAndCardMode(inOutRevenue.InOutRevenueId, "OUT");
                sealMessage = CheckSeal(listContCard, dataRow["cont_Seal"].ToString(), contCard);
                if (sealMessage.Length > 0)
                {
                    result += string.Format("Error: Seal={0} \r\n", sealMessage);
                }
                #endregion

                #region Check No Mobil
                string tnm = dataRow["cont_NoMobilOut"].ToString().ToUpper().Replace(" ", string.Empty);
                char[] angka = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                int i1 = tnm.IndexOfAny(angka);
                int i2 = tnm.LastIndexOfAny(angka);
                if (i2 >= 0) tnm = tnm.Insert(i2 + 1, " ");
                if (i1 >= 0) tnm = tnm.Insert(i1, " ");

                contInOut.NoMobilOut = tnm;
                if (contInOut.NoMobilOut.Length == 0)
                {
                    result += "Error: Nomor Truck Salah. -" + tnm + "-\r\n";
                }
                else
                {
                    string noMobilOutSpecialMessage = noMobilOutSpecialMessageDAL.GetNoMobilOutSpecialMessage(contInOut.NoMobilOut);
                    if (noMobilOutSpecialMessage.Length > 0)
                    {
                        result += string.Format("Error: Blocked= {0}, {1}. \r\n", contInOut.NoMobilOut, noMobilOutSpecialMessage);
                    }
                }

                #endregion

                #region Try to set dtmout
                if (contInOut.DtmOut.Length == 0)
                {
                    contInOut.DtmOut = GlobalWebServiceDAL.GetServerDtm().AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                // If still empty set error
                if (contInOut.DtmOut.Length == 0)
                {
                    result += "Error: DtmOut Conflict.\r\n";
                }
                #endregion

                #region Update card

                if (result.Length == 0)
                {
                    contCard.ContInOutID = contInOut.ContInOutId;
                    contCard.UserID3 = dataRow["activeuser"].ToString();
                    contCard.Dtm3 = GlobalWebServiceDAL.GetServerDtm();

                    string containerSeal = dataRow["cont_Seal"].ToString();
                    string[] seals = containerSeal.Split(",".ToCharArray());
                    if (seals.Length > 0)
                    {
                        if (seals.Length > 0 && seals[0].Trim().Length > 0)
                        {
                            contCard.Seal1 = seals[0].Trim();
                        }
                        if (seals.Length > 1 && seals[1].Trim().Length > 0)
                        {
                            contCard.Seal2 = seals[1].Trim();
                        }
                        if (seals.Length > 2 && seals[2].Trim().Length > 0)
                        {
                            contCard.Seal3 = seals[2].Trim();
                        }
                        if (seals.Length > 3 && seals[3].Trim().Length > 0)
                        {
                            contCard.Seal4 = seals[3].Trim();
                        }
                    }

                    try
                    {
                        contCardDAL.UpdateContCard(contCard);
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("SubmitKartuMuat - Update ContCard -> Message: {0}", ex.Message);
                        log.ErrorFormat("SubmitKartuMuat - Update ContCard -> StackTrace: {0}", ex.StackTrace);
                        result += "Error: Updating Contcard gagal\r\n";
                    }

                    #region Insert Log
                    ContainerLog containerLog = new ContainerLog();
                    containerLog.ContInOutId = contInOut.ContInOutId;
                    containerLog.Cont = contInOut.Cont;
                    containerLog.UserId = dataRow["activeuser"].ToString();
                    containerLog.EqpId = dataRow["eqpid"].ToString();
                    containerLog.FlagAct = "MUAT";
                    containerLog.Shipper = contInOut.CustomerCode;
                    containerLog.Operator = dataRow["opid"].ToString();
                    try
                    {
                        containerLogDAL.InsertContainerLog(containerLog);
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("SubmitKartuMuat - Update ContainerLog -> Message: {0}", ex.Message);
                        log.ErrorFormat("SubmitKartuMuat - Update ContainerLog -> StackTrace: {0}", ex.StackTrace);
                        result += "Error: Updating Log gagal\r\n";
                    }
                    #endregion

                    #region Update Blok
                    string oldLocation = contInOut.Location;
                    contInOut.Location = string.Empty;

                    Blok oldBlok = blokDAL.FillBlokByKode(oldLocation);
                    if (oldBlok.Cont == contInOut.Cont)
                    {
                        oldBlok.Cont = string.Empty;
                    }
                    if (oldBlok.Cont2 == contInOut.Cont)
                    {
                        oldBlok.Cont2 = string.Empty;
                    }
                    try
                    {
                        blokDAL.UpdateBlok(oldBlok);
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("SubmitKartuMuat - Update Blok -> Message: {0}", ex.Message);
                        log.ErrorFormat("SubmitKartuMuat - Update Blok -> StackTrace: {0}", ex.StackTrace);
                        result += "Error: Updating Blok gagal\r\n";
                    }
                    #endregion

                    result += "# Load Container " + contCard.Cont + " BERHASIL\r\n";
                }
                #endregion
                return result;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("SubmitKartuMuat -> Parameter: xml={0}", xml);
                log.ErrorFormat("SubmitKartuMuat -> Message: {0}", ex.Message);
                log.ErrorFormat("SubmitKartuMuat -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat melakukan submit kartu muat";
            }
        }

        /// <summary>
        /// Previous Name: ReSubmit_KartuMuat(string xml_parameter)
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [WebMethod]
        public string ResubmitKartuMuat(string xml)
        {
            try
            {
                //return xml_parameter;
                string result = string.Empty;
                DataTable dataTable = Converter.ConvertXmlToDataTable(xml);
                DataRow dataRow = dataTable.Rows[0];

                if (!AppPrincipal.LoginForService(dataRow["activeuser"].ToString()))
                {
                    return "Error: Tolong login terlebih dahulu";
                }

                #region Initialize Objects

                // Initialize InOutRevenue
                InOutRevenue inOutRevenue = inOutRevenueDAL.FillInOutRevenueByInOutRevenueId(Convert.ToInt64(dataRow["inoutrevenueid"]));
                if (inOutRevenue.InOutRevenueId <= 0)
                {
                    result += "Error: OR tidak ditemukan\r\n";
                }

                // Initialize ContInOut
                ContInOut contInOut = contInOutDAL.FillContInOutById(Convert.ToInt64(dataRow["continoutid"]));
                if (contInOut.ContInOutId <= 0)
                {
                    result += "Error: Cont tidak ditemukan\r\n";
                }

                // Initialize ContInOut_Reselect
                ContInOut contInOutReselect = contInOutDAL.FillContInOutById(Convert.ToInt64(dataRow["continoutid_reselect"]));
                if (contInOutReselect.ContInOutId <= 0)
                {
                    result += "Error: Cont Re-Select tidak ditemukan\r\n";
                }

                // Initialize CustDo
                CustDo custDo = custDoDAL.FillCustDoByCustDoId(Convert.ToInt64(dataRow["custdoid"]));
                if (custDo.CustDoId <= 0)
                {
                    result += "Error: DO tidak ditemukan\r\n";
                }

                // Initialize ContCard
                ContCard contCard = contCardDAL.FillContCardByContCardId(Convert.ToInt64(dataRow["contcardid"]));
                if (contCard.ContCardID <= 0)
                {
                    result += "Error: Contcard tidak ditemukan\r\n";
                }

                #endregion

                if (result.Length > 0)
                {
                    return result;
                }

                #region Canceling Seal
                contCard.ContInOutID = 0;
                contCard.UserID3 = dataRow["activeuser"].ToString();
                contCard.Dtm3 = null;
                contCard.Seal1 = string.Empty;
                try
                {
                    contCardDAL.UpdateContCard(contCard);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("ResubmitKartuMuat - Update ContCard -> Message: {0}", ex.Message);
                    log.ErrorFormat("ResubmitKartuMuat - Update ContCard -> StackTrace: {0}", ex.StackTrace);
                    result += "Error: Canceling Contcard gagal\r\n";
                }
                #endregion

                #region Check Seal and Get Seal error message
                contInOutReselect.Seal = dataRow["cont_Seal"].ToString();
                string sealMessage = CheckSeal(contInOutReselect);
                if (sealMessage.Length > 0)
                {
                    result += string.Format("Error: Seal={0} \r\n", sealMessage);
                }
                #endregion

                #region Check double definition for seal in other card
                List<ContCard> listContCard = contCardDAL.FillContCardByRefIdAndCardMode(inOutRevenue.InOutRevenueId, "OUT");
                sealMessage = CheckSeal(listContCard, dataRow["cont_Seal"].ToString(), contCard);
                if (sealMessage.Length > 0)
                {
                    result += string.Format("Error: Seal={0} \r\n", sealMessage);
                }
                #endregion

                #region Check Nomor Mobil
                string tnm = dataRow["cont_NoMobilOut"].ToString().ToUpper().Replace(" ", string.Empty);
                char[] angka = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                int i1 = tnm.IndexOfAny(angka);
                int i2 = tnm.LastIndexOfAny(angka);
                if (i2 >= 0) tnm = tnm.Insert(i2 + 1, " ");
                if (i1 >= 0) tnm = tnm.Insert(i1, " ");

                contInOutReselect.NoMobilOut = tnm;
                if (contInOutReselect.NoMobilOut.Length == 0)
                {
                    result += "Error: Nomor Truck Salah. -" + tnm + "-\r\n";
                }
                else
                {
                    string noMobilOutSpecialMessage = noMobilOutSpecialMessageDAL.GetNoMobilOutSpecialMessage(contInOutReselect.NoMobilOut);
                    if (noMobilOutSpecialMessage.Length > 0)
                    {
                        result += string.Format("Error: Blocked= {0}, {1}. \r\n", contInOutReselect.NoMobilOut, noMobilOutSpecialMessage);
                    }
                }
                #endregion

                #region Set DtmOut

                if (contInOutReselect.DtmOut.Length == 0)
                {
                    contInOutReselect.DtmOut = GlobalWebServiceDAL.GetServerDtm().AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                // if still empty set error
                if (contInOutReselect.DtmOut.Length == 0)
                {
                    result += "Error: DtmOut Conflict.\r\n";
                }

                #endregion

                #region save to card if result is empty string

                if (result.Length == 0)
                {
                    contCard.ContInOutID = contInOutReselect.ContInOutId;
                    contCard.UserID3 = dataRow["activeuser"].ToString();
                    contCard.Dtm3 = GlobalWebServiceDAL.GetServerDtm();
                    contCard.Seal1 = dataRow["cont_Seal"].ToString();
                    try
                    {
                        contCardDAL.UpdateContCard(contCard);
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("ResubmitKartuMuat - Update ContCard -> Message: {0}", ex.Message);
                        log.ErrorFormat("ResubmitKartuMuat - Update ContCard -> StackTrace: {0}", ex.StackTrace);
                        result += "Error: Updating Contcard gagal\r\n";
                    }

                    #region Insert Log
                    ContainerLog containerLog = new ContainerLog();
                    containerLog.ContInOutId = contInOutReselect.ContInOutId;
                    containerLog.Cont = contInOutReselect.Cont;
                    containerLog.UserId = dataRow["activeuser"].ToString();
                    containerLog.EqpId = dataRow["eqpid"].ToString();
                    containerLog.FlagAct = "MUAT";
                    containerLog.Shipper = contInOutReselect.CustomerCode;
                    containerLog.Operator = dataRow["opid"].ToString();
                    try
                    {
                        containerLogDAL.InsertContainerLog(containerLog);
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("ResubmitKartuMuat - Update ContCard -> Message: {0}", ex.Message);
                        log.ErrorFormat("ResubmitKartuMuat - Update ContCard -> StackTrace: {0}", ex.StackTrace);
                        result += "Error: Updating Log gagal\r\n";
                    }
                    #endregion

                    //Update blok
                    string OldLocation = contInOutReselect.Location;
                    contInOutReselect.Location = string.Empty;
                    //cont.Update(null,null);

                    #region Update Blok
                    string oldLocation = contInOutReselect.Location;
                    contInOutReselect.Location = string.Empty;

                    Blok oldBlok = blokDAL.FillBlokByKode(oldLocation);
                    if (oldBlok.Cont == contInOutReselect.Cont)
                    {
                        oldBlok.Cont = string.Empty;
                    }
                    if (oldBlok.Cont2 == contInOutReselect.Cont)
                    {
                        oldBlok.Cont2 = string.Empty;
                    }
                    try
                    {
                        blokDAL.UpdateBlok(oldBlok);
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("ResubmitKartuMuat - Update Blok -> Message: {0}", ex.Message);
                        log.ErrorFormat("ResubmitKartuMuat - Update Blok -> StackTrace: {0}", ex.StackTrace);
                        result += "Error: Updating Blok gagal\r\n";
                    }
                    #endregion
                    result += "# Ganti Container " + contCard.Cont + " BERHASIL\r\n";
                }

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("ResubmitKartuMuat -> Parameter: xml={0}", xml);
                log.ErrorFormat("ResubmitKartuMuat -> Message: {0}", ex.Message);
                log.ErrorFormat("ResubmitKartuMuat -> StackTrace: {0}", ex.StackTrace);
                return "Error: Tidak dapat melakukan resubmit kartu muat";
            }
        }
        #endregion
    }
}
