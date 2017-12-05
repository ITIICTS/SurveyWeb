using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class ContInOutDAL
    {
        public const string DEFAULT_COLUMN = "{0}continoutid,{0}cont,{0}customercode,{0}size,{0}type,{0}berat,{0}manufacture,{0}cscplate, " +
                                            "{0}rfmachine,{0}tare,{0}dtmin,{0}dtmout,{0}dtmrepaired,{0}dtmpti,{0}washstatus,{0}condition, " +
                                            "{0}actin,{0}actout,{0}location,{0}exvessel,{0}exvesselname,{0}consignee,{0}nomobilin,{0}angkutanin, " +
                                            "{0}payload,{0}nomobilout,{0}angkutanout,{0}donumber,{0}shipper,{0}destination,{0}destinationname, " +
                                            "{0}seal,{0}vesselvoyage,{0}vesselvoyagename,{0}noseriorout,{0}dtmoutdepoin,{0}remarks,{0}ediin,{0}ediout, " +
                                            "{0}ediwash,{0}ediapr,{0}edicom,{0}edipti,{0}edisync,{0}eirin,{0}eirout,{0}dtmeirin,{0}eirincontact,{0}dtmportout, " +
                                            "{0}blnumber,{0}cleaningrefno,{0}cleaningremark,{0}cleaninglastcargo,{0}cleaningcost,{0}cleaningdtmfinish, " +
                                            "{0}bookingassignment,{0}cleaningkode,{0}cleaningdesc,{0}cleaningaction,{0}dtmshortpti,{0}exvesselport, " +
                                            "{0}rfenginecond,{0}rfdtmenginerepaired,{0}rfengineediin,{0}rfengineedicom,{0}rfptitype,{0}rfptidtmapproved, " +
                                            "{0}rfptidtmcompleted,{0}rfptiremark,{0}rfpticost,{0}rfptitemp,{0}rfneedswupdate,{0}rfdtmswupdated,{0}grade, " +
                                            "{0}mddc_remark,{0}commodity,{0}businessunit,{0}ventilation,{0}humidity,{0}vendorangkutanin,{0}rkemin,{0}isfreeuse ";
        public const string DEFAULT_TABLE = "continout";

        private void MappingDataReaderToContCard(NpgsqlDataReader npgsqlDataReader, ContInOut contInOut)
        {
            contInOut.ContInOutId = npgsqlDataReader.GetInt64(0);
            contInOut.Cont = npgsqlDataReader.GetString(1);
            contInOut.CustomerCode = npgsqlDataReader.GetString(2);
            contInOut.Size = npgsqlDataReader.GetString(3);
            contInOut.Type = npgsqlDataReader.GetString(4);
            contInOut.Berat = npgsqlDataReader.GetString(5);
            contInOut.Manufacture = npgsqlDataReader.GetString(6);
            contInOut.CscPlate = npgsqlDataReader.GetString(7);
            contInOut.RfMachine = npgsqlDataReader.GetString(8);
            contInOut.Tare = npgsqlDataReader.GetString(9);

            if (npgsqlDataReader["dtmin"] != DBNull.Value)
            {
                contInOut.DtmIn = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtmin")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            if (npgsqlDataReader["dtmout"] != DBNull.Value)
            {
                contInOut.DtmOut = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtmout")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            if (npgsqlDataReader["dtmrepaired"] != DBNull.Value)
            {
                contInOut.DtmOut = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtmrepaired")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            if (npgsqlDataReader["dtmpti"] != DBNull.Value)
            {
                contInOut.DtmOut = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtmpti")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }

            contInOut.WashStatus = npgsqlDataReader.GetString(14);
            contInOut.Condition = npgsqlDataReader.GetString(15);

            contInOut.ActIn = npgsqlDataReader.GetString(16);
            contInOut.ActOut = npgsqlDataReader.GetString(17);
            contInOut.Location = npgsqlDataReader.GetString(18);

            contInOut.ExVessel = npgsqlDataReader.GetString(19);
            contInOut.ExVesselName = npgsqlDataReader.GetString(20);
            contInOut.Consignee = npgsqlDataReader.GetString(21);
            if (npgsqlDataReader[75] != DBNull.Value)
            {
                contInOut.Commodity = npgsqlDataReader.GetString(75);
            }
            contInOut.NoMobilIn = npgsqlDataReader.GetString(22);
            contInOut.AngkutanIn = npgsqlDataReader.GetString(23);
            contInOut.Payload = npgsqlDataReader.GetString(24);

            contInOut.NoMobilOut = npgsqlDataReader.GetString(25);
            contInOut.AngkutanOut = npgsqlDataReader.GetString(26);
            contInOut.DoNumber = npgsqlDataReader.GetString(27);
            contInOut.Shipper = npgsqlDataReader.GetString(28);
            contInOut.Destination = npgsqlDataReader.GetString(29);
            contInOut.DestinationName = npgsqlDataReader.GetString(30);
            contInOut.Seal = npgsqlDataReader.GetString(31);
            contInOut.VesselVoyage = npgsqlDataReader.GetString(32);
            contInOut.VesselVoyageName = npgsqlDataReader.GetString(33);
            contInOut.NoSeriOrOut = npgsqlDataReader.GetString(34);
            if (npgsqlDataReader["dtmoutdepoin"] != DBNull.Value)
            {
                contInOut.DtmOutDepoIn = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtmoutdepoin")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }

            contInOut.EdiIn = npgsqlDataReader.GetString(37);
            contInOut.EdiOut = npgsqlDataReader.GetString(38);
            contInOut.EdiWash = npgsqlDataReader.GetString(39);
            contInOut.EdiApr = npgsqlDataReader.GetString(40);
            contInOut.EdiCom = npgsqlDataReader.GetString(41);
            contInOut.EdiPti = npgsqlDataReader.GetString(42);
            contInOut.EdiSync = npgsqlDataReader.GetString(43);

            contInOut.EirIn = npgsqlDataReader.GetString(44);
            contInOut.EirOut = npgsqlDataReader.GetString(45);
            if (npgsqlDataReader["dtmeirin"] != DBNull.Value)
            {
                contInOut.DtmEirIn = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtmeirin")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            contInOut.EirInContact = npgsqlDataReader.GetString(47);

            if (npgsqlDataReader["dtmportout"] != DBNull.Value)
            {
                contInOut.DtmPortOut = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtmportout")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            contInOut.BlNumber = npgsqlDataReader.GetString(49);

            contInOut.CleaningRefNo = npgsqlDataReader.GetString(50);
            contInOut.CleaningRemark = npgsqlDataReader.GetString(51);
            contInOut.CleaningLastCargo = npgsqlDataReader.GetString(52);
            contInOut.CleaningCost = npgsqlDataReader.GetDouble(53);
            if (npgsqlDataReader["cleaningdtmfinish"] != DBNull.Value)
            {
                contInOut.CleaningDtmFinish = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("cleaningdtmfinish")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }

            contInOut.BookingAssignment = npgsqlDataReader.GetString(55);
            contInOut.CleaningKode = npgsqlDataReader.GetString(56);
            contInOut.CleaningDesc = npgsqlDataReader.GetString(57);
            contInOut.CleaningAction = npgsqlDataReader.GetString(58);

            if (npgsqlDataReader["dtmshortpti"] != DBNull.Value)
            {
                contInOut.DtmShortPti = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtmshortpti")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }

            contInOut.ExVesselPort = npgsqlDataReader.GetString(60);
            contInOut.RfEngineCond = npgsqlDataReader.GetString(61);

            if (npgsqlDataReader["rfdtmenginerepaired"] != DBNull.Value)
            {
                contInOut.RfDtmEngineRepaired = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("rfdtmenginerepaired")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            contInOut.RfEngineEdiIn = npgsqlDataReader.GetString(63);
            contInOut.RfEngineEdiCom = npgsqlDataReader.GetString(64);
            contInOut.RfPtiType = npgsqlDataReader.GetString(65);
            if (npgsqlDataReader["rfptidtmapproved"] != DBNull.Value)
            {
                contInOut.RfPtiDtmApproved = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("rfptidtmapproved")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            if (npgsqlDataReader["rfptidtmcompleted"] != DBNull.Value)
            {
                contInOut.RfDtmEngineRepaired = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("rfptidtmcompleted")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            contInOut.RfPtiRemark = GetRFPTIRemark(contInOut.Cont, npgsqlDataReader.GetString(68));
            contInOut.RfPtiCost = npgsqlDataReader.GetDouble(69);
            contInOut.RfPtiTemp = npgsqlDataReader.GetString(70);
            contInOut.RfNeedSwUpdate = npgsqlDataReader.GetInt32(71);
            if (npgsqlDataReader["rfdtmswupdated"] != DBNull.Value)
            {
                contInOut.RfDtmSwUpdated = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("rfdtmswupdated")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }

            if (npgsqlDataReader[73] != DBNull.Value)
            {
                contInOut.GradeV2 = npgsqlDataReader.GetString(73);
            }
            if (npgsqlDataReader[74] != DBNull.Value)
            {
                contInOut.MddcRemark = npgsqlDataReader.GetString(74);
            }

            if (npgsqlDataReader["businessunit"] != DBNull.Value)
            {
                contInOut.BusinessUnit = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("businessunit"));
            }

            if (npgsqlDataReader[npgsqlDataReader.GetOrdinal("ventilation")] != DBNull.Value)
            {
                contInOut.Ventilation = npgsqlDataReader.GetInt32(npgsqlDataReader.GetOrdinal("ventilation"));
            }

            if (npgsqlDataReader[npgsqlDataReader.GetOrdinal("humidity")] != DBNull.Value)
            {
                contInOut.Humidity = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("humidity"));
            }

            if (npgsqlDataReader[npgsqlDataReader.GetOrdinal("vendorangkutanin")] != DBNull.Value)
            {
                contInOut.VendorAngkutanIn = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("vendorangkutanin"));
            }

            if (npgsqlDataReader[npgsqlDataReader.GetOrdinal("rkemin")] != DBNull.Value)
            {
                contInOut.RkemIn = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("rkemin"));
            }

            if (npgsqlDataReader[npgsqlDataReader.GetOrdinal("isfreeuse")] != DBNull.Value)
            {
                contInOut.IsFreeUse = npgsqlDataReader.GetBoolean(npgsqlDataReader.GetOrdinal("isfreeuse"));
            }
        }

        private string GetRFPTIRemark(string cont, string remarks)
        {
            if (remarks.Length > 0)
            {
                return remarks;
            }
            else
            {
                if (cont.Substring(0, 4) == "MSFU")
                {
                    return "SFR";
                }
                else
                    if (cont.Substring(0, 4) == "MSVU")
                {
                    return "STV";
                }
                else
                        if (cont.Substring(0, 4) == "MCAU")
                {
                    return "CAT";
                }
                else
                {
                    return remarks;
                }
            }
        }

        /// <summary>
        /// Fill Container Card By contCardId, cardMode
        /// </summary>
        /// <param name="contCardId"></param>
        /// <param name="cardMode"></param>
        /// <returns></returns>
        public ContInOut FillContInOutById(long contInOutId)
        {
            ContInOut contInOut = new ContInOut();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} " +
                                                " FROM {1} " +
                                                "WHERE continoutid=@ContInOutId ",
                                                string.Format(DEFAULT_COLUMN, string.Empty),
                                                DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContInOutId", contInOutId);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                MappingDataReaderToContCard(npgsqlDataReader, contInOut);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contInOut;
        }

        /// <summary>
        /// Fill ContInOut By Container Number
        /// </summary>
        /// <param name="containerNumber"></param>
        /// <returns></returns>
        public ContInOut FillContInOutByContainerNumber(string containerNumber)
        {
            ContInOut contInOut = new ContInOut();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} " +
                                                " FROM {1} " +
                                                " WHERE cont=@ContainerNumber " +
                                                "cORDER BY dtmin DESC",
                                                string.Format(DEFAULT_COLUMN, string.Empty),
                                                DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContainerNumber", containerNumber);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                MappingDataReaderToContCard(npgsqlDataReader, contInOut);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contInOut;
        }

        public List<ContInOut> FillByNoSeriOrOut(string noSeriOrOut, string size, string type, string takeDefined)
        {            
            string definedCondition = string.Empty;
            if (takeDefined.Length > 0)
            {
                string[] conts = takeDefined.Split(",".ToCharArray());
                foreach (string c in conts)
                {
                    if (definedCondition.Length > 0) definedCondition += ",";
                    definedCondition += "'" + c + "'";
                }
                definedCondition = " AND cont NOT IN (" + definedCondition + ")";
            }

            List<ContInOut> listContInOut = new List<ContInOut>();

            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} " +
                                         " FROM continout {1} " +
                                         " WHERE noseriorout=@NoSeriOrOut AND size=@Size AND type=@Type " +
                                         " {2} " +
                                         "ORDER BY cont,dtmin DESC ",
                                         string.Format(DEFAULT_COLUMN, string.Empty),
                                         DEFAULT_TABLE,
                                         definedCondition);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@NoSeriOrOut", noSeriOrOut);
                        npgsqlCommand.Parameters.AddWithValue("@Size", size);
                        npgsqlCommand.Parameters.AddWithValue("@Type", type);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                ContInOut contInOut = new ContInOut();
                                MappingDataReaderToContCard(npgsqlDataReader, contInOut);
                                listContInOut.Add(contInOut);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listContInOut;            
        }

        /// <summary>
        /// Get Container Stock By Container Number
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="isSetMessage"></param>
        /// <returns></returns>
        public List<ContInOut> GetContainerStockByContainerNumber(string cont, bool isSetMessage)
        {
            List<ContInOut> listContInOut = new List<ContInOut>();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} FROM {1} " +
                                                 "  WHERE cont LIKE @Container " +
                                                 "      AND dtmout IS NULL " +
                                                 "  ORDER BY cont,dtmin ", 
                                         string.Format(DEFAULT_COLUMN, string.Empty), 
                                         DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@Container", cont + '%');
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                ContInOut contInOut = new ContInOut();
                                MappingDataReaderToContCard(npgsqlDataReader, contInOut);
                                if (isSetMessage)
                                {
                                    BlackListDAL blackListDAL = new BlackListDAL();
                                    contInOut.Message = blackListDAL.GetMessageByContainerNumber(contInOut.Cont);
                                }
                                listContInOut.Add(contInOut);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listContInOut;
        }

        /// <summary>
        /// Get Container Stock By Blok
        /// </summary>
        /// <param name="blok"></param>
        /// <returns></returns>
        public List<ContInOut> GetContainerStockByBlok(string blok)
        {
            List<ContInOut> listContInOut = new List<ContInOut>();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} " +
                                             " FROM {1} " +
                                             " WHERE location = @Blok AND dtmout IS NULL " + 
                                             " ORDER BY dtmin DESC ",
                                         string.Format(DEFAULT_COLUMN, string.Empty),
                                         DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@Blok", blok);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                ContInOut contInOut = new ContInOut();
                                MappingDataReaderToContCard(npgsqlDataReader, contInOut);
                                listContInOut.Add(contInOut);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listContInOut;
        }

        /// <summary>
        /// Update Location To TMP
        /// </summary>
        /// <param name="containerToBeTMPLeft"></param>
        /// <param name="containerToBeTMPRight"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public int UpdateLocationToTMP(string containerToBeTMPLeft, string containerToBeTMPRight, string location)
        {
            int affectedRow = 0;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = "UPDATE continout " +
                                    "   SET location='TMP' " +
                                    "   WHERE (cont = @ContainerNumberLeft OR cont = @ContainerNumberRight) " +
                                    "           AND location=@Location ";
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContainerNumberLeft", containerToBeTMPLeft);
                        npgsqlCommand.Parameters.AddWithValue("@ContainerNumberRight", containerToBeTMPRight);
                        npgsqlCommand.Parameters.AddWithValue("@Location", location);
                        affectedRow = npgsqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return affectedRow;
        }

        /// <summary>
        /// Update ContInOut
        /// </summary>
        /// <param name="contInOut"></param>
        /// <returns></returns>
        public int UpdateContInOut(ContInOut contInOut)
        {
            int affectedRow = 0;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = "UPDATE continout SET " +
                            "cont=@cont, customercode=@customercode, size=@size, " +
                            "type=@type, berat=@berat, manufacture=@manufacture, " +
                            "cscplate=@cscplate, dtmin=@dtmin, dtmout=@dtmout, " +
                            "dtmrepaired=@dtmrepaired, dtmpti=@dtmpti, washstatus=@washstatus, " +
                            "condition=@condition, actin=@actin, actout=@actout, location=@location, " +
                            "exvessel=@exvessel, consignee=@consignee, nomobilin=@nomobilin, " +
                            "angkutanin=@angkutanin, nomobilout=@nomobilout, " +
                            "angkutanout=@angkutanout, donumber=@donumber, shipper=@shipper, " +
                            "destination=@destination, seal=@seal,payload=@payload, " +
                            "vesselvoyage=@vesselvoyage,exvesselname=@exvesselname, " +
                            "vesselvoyagename=@vesselvoyagename, remarks=@remarks, " +
                            "dtmeirin=@dtmeirin, eirincontact=@eirincontact, noseriorout=@noseriorout, " +
                            "ediin=@ediin,ediout=@ediout,ediwash=@ediwash,ediapr=@ediapr, " +
                            "edicom=@edicom,edipti=@edipti,edisync=@edisync,eirin=@eirin,eirout=@eirout, " +
                            "rfmachine=@rfmachine, destinationname=@destinationname, " +
                            "blnumber=@blnumber,dtmportout=@dtmportout, dtmoutdepoin=@dtmoutdepoin, " +
                            "tare=@tare,cleaningrefno=@cleaningrefno,cleaningremark=@cleaningremark, " +
                            "cleaninglastcargo=@cleaninglastcargo,cleaningdtmfinish=@cleaningdtmfinish, " +
                            "cleaningcost=@cleaningcost, bookingassignment=@bookingassignment, " +
                            "cleaningkode=@cleaningkode,cleaningdesc=@cleaningdesc,cleaningaction=@cleaningaction, " +
                            "dtmshortpti=@dtmshortpti,exvesselport=@exvesselport, rfenginecond=@rfenginecond, " +
                            "rfdtmenginerepaired=@rfdtmenginerepaired,rfengineediin=@rfengineediin,rfengineedicom=@rfengineedicom, " +
                            "rfptitype=@rfptitype,rfptidtmapproved=@rfptidtmapproved,rfptidtmcompleted=@rfptidtmcompleted, " +
                            "rfptiremark=@rfptiremark,rfpticost=@rfpticost,rfptitemp=@rfptitemp, " +
                            "rfneedswupdate=@rfneedswupdate,rfdtmswupdated=@rfdtmswupdated, " +
                            "grade=@grade,commodity=@commodity,mddc_remark=@mddcRemark, " +
                            "businessunit=@businessUnit, vendorangkutanin=@vendorangkutanin, rkemin=@rkemin, isfreeuse=@isfreeuse " +
                            "WHERE continoutid=@continoutid ";
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@continoutid", contInOut.ContInOutId);
                        npgsqlCommand.Parameters.AddWithValue("@cont", contInOut.Cont);
                        npgsqlCommand.Parameters.AddWithValue("@customercode", contInOut.CustomerCode);
                        npgsqlCommand.Parameters.AddWithValue("@size", contInOut.Size);
                        npgsqlCommand.Parameters.AddWithValue("@type", contInOut.Type);
                        npgsqlCommand.Parameters.AddWithValue("@berat", contInOut.Berat);
                        npgsqlCommand.Parameters.AddWithValue("@manufacture", contInOut.Manufacture);
                        npgsqlCommand.Parameters.AddWithValue("@cscplate", contInOut.CscPlate);
                        npgsqlCommand.Parameters.AddWithValue("@dtmin", contInOut.DtmIn);
                        npgsqlCommand.Parameters.AddWithValue("@dtmout", contInOut.DtmOut);
                        npgsqlCommand.Parameters.AddWithValue("@dtmrepaired", contInOut.DtmRepaired);
                        npgsqlCommand.Parameters.AddWithValue("@dtmpti", contInOut.DtmPti);
                        npgsqlCommand.Parameters.AddWithValue("@washstatus", contInOut.WashStatus);
                        npgsqlCommand.Parameters.AddWithValue("@condition", contInOut.Condition);
                        npgsqlCommand.Parameters.AddWithValue("@actin", contInOut.ActIn);
                        npgsqlCommand.Parameters.AddWithValue("@actout", contInOut.ActOut);
                        npgsqlCommand.Parameters.AddWithValue("@location", contInOut.Location);
                        npgsqlCommand.Parameters.AddWithValue("@exvessel", contInOut.ExVessel);
                        npgsqlCommand.Parameters.AddWithValue("@exvesselname", contInOut.ExVesselName);
                        npgsqlCommand.Parameters.AddWithValue("@consignee", contInOut.Consignee);
                        npgsqlCommand.Parameters.AddWithValue("@nomobilin", contInOut.NoMobilIn);
                        npgsqlCommand.Parameters.AddWithValue("@angkutanin", contInOut.AngkutanIn);
                        npgsqlCommand.Parameters.AddWithValue("@nomobilout", contInOut.NoMobilOut);
                        npgsqlCommand.Parameters.AddWithValue("@angkutanout", contInOut.AngkutanOut);
                        npgsqlCommand.Parameters.AddWithValue("@donumber", contInOut.DoNumber);
                        npgsqlCommand.Parameters.AddWithValue("@shipper", contInOut.Shipper);
                        npgsqlCommand.Parameters.AddWithValue("@destination", contInOut.Destination);
                        npgsqlCommand.Parameters.AddWithValue("@seal", contInOut.Seal);
                        npgsqlCommand.Parameters.AddWithValue("@vesselvoyage", contInOut.VesselVoyage);
                        npgsqlCommand.Parameters.AddWithValue("@vesselvoyagename", contInOut.VesselVoyageName);
                        npgsqlCommand.Parameters.AddWithValue("@remarks", contInOut.Remarks);
                        npgsqlCommand.Parameters.AddWithValue("@payload", contInOut.Payload);
                        npgsqlCommand.Parameters.AddWithValue("@dtmeirin", contInOut.DtmEirIn);
                        npgsqlCommand.Parameters.AddWithValue("@eirincontact", contInOut.EirInContact);
                        npgsqlCommand.Parameters.AddWithValue("@noseriorout", contInOut.NoSeriOrOut);
                        npgsqlCommand.Parameters.AddWithValue("@ediin", contInOut.EdiIn);
                        npgsqlCommand.Parameters.AddWithValue("@ediout", contInOut.EdiOut);
                        npgsqlCommand.Parameters.AddWithValue("@ediwash", contInOut.EdiWash);
                        npgsqlCommand.Parameters.AddWithValue("@ediapr", contInOut.EdiApr);
                        npgsqlCommand.Parameters.AddWithValue("@edicom", contInOut.EdiCom);
                        npgsqlCommand.Parameters.AddWithValue("@edipti", contInOut.EdiPti);
                        npgsqlCommand.Parameters.AddWithValue("@edisync", contInOut.EdiSync);
                        npgsqlCommand.Parameters.AddWithValue("@eirin", contInOut.EirIn);
                        npgsqlCommand.Parameters.AddWithValue("@eirout", contInOut.EirOut);
                        npgsqlCommand.Parameters.AddWithValue("@rfmachine", contInOut.RfMachine);
                        npgsqlCommand.Parameters.AddWithValue("@destinationname", contInOut.DestinationName);
                        npgsqlCommand.Parameters.AddWithValue("@blnumber", contInOut.BlNumber);
                        npgsqlCommand.Parameters.AddWithValue("@dtmportout", contInOut.DtmPortOut);
                        npgsqlCommand.Parameters.AddWithValue("@dtmoutdepoin", contInOut.DtmOutDepoIn);
                        npgsqlCommand.Parameters.AddWithValue("@tare", contInOut.Tare);
                        npgsqlCommand.Parameters.AddWithValue("@cleaningrefno", contInOut.CleaningRefNo);
                        npgsqlCommand.Parameters.AddWithValue("@cleaningremark", contInOut.CleaningRemark);
                        npgsqlCommand.Parameters.AddWithValue("@cleaninglastcargo", contInOut.CleaningLastCargo);
                        npgsqlCommand.Parameters.AddWithValue("@cleaningdtmfinish", contInOut.CleaningDtmFinish);
                        npgsqlCommand.Parameters.AddWithValue("@cleaningcost", contInOut.CleaningCost);
                        npgsqlCommand.Parameters.AddWithValue("@bookingassignment", contInOut.BookingAssignment);
                        npgsqlCommand.Parameters.AddWithValue("@cleaningkode", contInOut.CleaningKode);
                        npgsqlCommand.Parameters.AddWithValue("@cleaningdesc", contInOut.CleaningDesc);
                        npgsqlCommand.Parameters.AddWithValue("@cleaningaction", contInOut.CleaningAction);
                        npgsqlCommand.Parameters.AddWithValue("@dtmshortpti", contInOut.DtmShortPti);
                        npgsqlCommand.Parameters.AddWithValue("@exvesselport", contInOut.ExVesselPort);
                        npgsqlCommand.Parameters.AddWithValue("@rfenginecond", contInOut.RfEngineCond);
                        npgsqlCommand.Parameters.AddWithValue("@rfdtmenginerepaired", contInOut.RfDtmEngineRepaired);
                        npgsqlCommand.Parameters.AddWithValue("@rfengineediin", contInOut.RfEngineEdiIn);
                        npgsqlCommand.Parameters.AddWithValue("@rfengineedicom", contInOut.RfEngineEdiCom);
                        npgsqlCommand.Parameters.AddWithValue("@rfptitype", contInOut.RfPtiType);
                        npgsqlCommand.Parameters.AddWithValue("@rfptidtmapproved", contInOut.RfPtiDtmApproved);
                        npgsqlCommand.Parameters.AddWithValue("@rfptidtmcompleted", contInOut.RfPtiDtmCompleted);
                        npgsqlCommand.Parameters.AddWithValue("@rfptiremark", contInOut.RfPtiRemark);
                        npgsqlCommand.Parameters.AddWithValue("@rfpticost", contInOut.RfPtiCost);
                        npgsqlCommand.Parameters.AddWithValue("@rfptitemp", contInOut.RfPtiTemp);
                        npgsqlCommand.Parameters.AddWithValue("@rfneedswupdate", contInOut.RfNeedSwUpdate);
                        npgsqlCommand.Parameters.AddWithValue("@rfdtmswupdated", contInOut.RfDtmSwUpdated);
                        npgsqlCommand.Parameters.AddWithValue("@grade", contInOut.GradeV2);
                        npgsqlCommand.Parameters.AddWithValue("@commodity", contInOut.Commodity);

                        npgsqlCommand.Parameters.AddWithValue("@mddcRemark", contInOut.MddcRemark);

                        npgsqlCommand.Parameters.AddWithValue("@businessUnit", contInOut.BusinessUnit);

                        npgsqlCommand.Parameters.AddWithValue("@vendorangkutanin", contInOut.VendorAngkutanIn);
                        npgsqlCommand.Parameters.AddWithValue("@rkemin", contInOut.RkemIn);

                        npgsqlCommand.Parameters.AddWithValue("@isfreeuse", contInOut.IsFreeUse);
                        affectedRow = npgsqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return affectedRow;
        }
    }
}
