using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
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

            //datetime data
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

            //data condition, washing
            contInOut.WashStatus = npgsqlDataReader.GetString(14);
            contInOut.Condition = npgsqlDataReader.GetString(15);

            //act in out
            contInOut.ActIn = npgsqlDataReader.GetString(16);
            contInOut.ActOut = npgsqlDataReader.GetString(17);

            //lokasi
            contInOut.Location = npgsqlDataReader.GetString(18);

            //data in
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

            //data out
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

            // remark
            //contInOut.Rem = new RemarkList(npgsqlDataReader.GetString(36));

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

            //cleaning data
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

            //ocl request
            contInOut.ExVesselPort = npgsqlDataReader.GetString(60);

            //reefer new implementation
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
    }
}
