using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;


namespace ITI.Survey.Web.Dll.DAL
{
    public class DestinationRuleDAL
    {
        public const string DEFAULT_COLUMN = " destruleid,customercode,cont,size,type,disabled,remark,kodeallow,kodedeny,remindinputdo,kodeallow2,kodeallow3,withrange,range1,range2 ";
        public const string DEFAULT_TABLE = "destrule";

        private void MappingDataReaderToDestinationRule(NpgsqlDataReader npgsqlDataReader, DestinationRule destinationRule)
        {
            destinationRule.DestinationRuleId = npgsqlDataReader.GetInt64(0);
            destinationRule.CustomerCode = npgsqlDataReader.GetString(1);
            destinationRule.Cont = npgsqlDataReader.GetString(2);
            destinationRule.Size = npgsqlDataReader.GetString(3);
            destinationRule.Type = npgsqlDataReader.GetString(4);
            destinationRule.Disabled = npgsqlDataReader.GetInt32(5) == 0 ? false : true;
            destinationRule.Remark = npgsqlDataReader.GetString(6);
            destinationRule.KodeAllow = npgsqlDataReader.GetString(7);
            destinationRule.KodeDeny = npgsqlDataReader.GetString(8);
            destinationRule.RemindInputDo = npgsqlDataReader.GetInt32(9) == 0 ? false : true;
            destinationRule.KodeAllow2 = npgsqlDataReader.GetString(10);
            destinationRule.KodeAllow3 = npgsqlDataReader.GetString(11);
            destinationRule.WithRange = npgsqlDataReader.GetInt32(12) == 0 ? false : true;
            destinationRule.Range1 = npgsqlDataReader.GetString(13);
            destinationRule.Range2 = npgsqlDataReader.GetString(14);
        }

        /// <summary>
        /// Fill Destination For Out Activity
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="containerNumber"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<DestinationRule> FillForOut(string customerCode, string containerNumber, string size, string type)
        {
            List<DestinationRule> listDestinationRule = new List<DestinationRule>();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} " +
                                         "  ,cast(substring(@cont,6,3)||substring(@cont,10,3)||substring(@cont,14,1) as int)as reg " +
                                         "  ,case when range1 != '' then (cast(substring(range1,1,3)||substring(range1,5,3)||substring(range1,9,1) as int)) else 0 end as v1 " +
                                         "  ,case when range2 != '' then (cast(substring(range2,1,3)||substring(range2,5,3)||substring(range2,9,1) as int)) else 0 end as v2 " +
                                         " FROM {1} " +
                                         " WHERE customercode=@CustomerCode AND remindinputdo=0 AND disabled=0 " +
                                         "      AND cont=substr(@ContainerNumber,1,length(cont)) " +
                                         "      AND size=substr(@Size,1,length(size)) " +
                                         "      AND type=substr(@Type,1,length(type)) " +
                                         " ORDER BY destruleid ", DEFAULT_COLUMN, DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@CustomerCode", customerCode);
                        npgsqlCommand.Parameters.AddWithValue("@ContainerNumber", containerNumber);
                        npgsqlCommand.Parameters.AddWithValue("@Size", size);
                        npgsqlCommand.Parameters.AddWithValue("@Type", type);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                // Cek Deny
                                if (npgsqlDataReader.GetString(8).Length > 0)
                                {
                                    // Cek WithRange
                                    if (npgsqlDataReader.GetInt32(12) == 0)
                                    {
                                        DestinationRule destinationRule = new DestinationRule();
                                        MappingDataReaderToDestinationRule(npgsqlDataReader, destinationRule);
                                        listDestinationRule.Add(destinationRule);
                                    }
                                    else
                                    {
                                        if ((npgsqlDataReader.GetInt32(15) >= npgsqlDataReader.GetInt32(16)) && (npgsqlDataReader.GetInt32(15) <= npgsqlDataReader.GetInt32(17)))
                                        {
                                            DestinationRule destinationRule = new DestinationRule();
                                            MappingDataReaderToDestinationRule(npgsqlDataReader, destinationRule);
                                            listDestinationRule.Add(destinationRule);
                                        }
                                    }
                                }
                                else // Cek Allow/Deny
                                    if (npgsqlDataReader.GetString(7).Length > 0)
                                {
                                    if ((npgsqlDataReader.GetInt32(15) < npgsqlDataReader.GetInt32(16)) || (npgsqlDataReader.GetInt32(15) > npgsqlDataReader.GetInt32(17)))
                                    {
                                        DestinationRule destinationRule = new DestinationRule();
                                        MappingDataReaderToDestinationRule(npgsqlDataReader, destinationRule);
                                        listDestinationRule.Add(destinationRule);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listDestinationRule;
        }

        /// <summary>
        /// List For Out
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="containerNumber"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <param name="destinationName"></param>
        /// <returns></returns>
        public List<DestinationRule> FillForOut(string customerCode, string containerNumber, string size, string type, string destinationName)
        {
            List<DestinationRule> listDestinationRule = new List<DestinationRule>();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string querySelect = "SELECT DISTINCT destruleid, customercode, cont, size, type, disabled, destrule.remark,kodeallow,kodedeny,remindinputdo,kodeallow2,kodeallow3,withrange,range1,range2, " +
                                    "   cast(substring(@cont,6,3)||substring(@cont,10,3)||substring(@cont,14,1) as int)as reg, " +
                                    "   case when range1 != '' then(cast(substring(range1, 1, 3) || substring(range1, 5, 3) || substring(range1, 9, 1) as int)) else 0 end as v1, " +
	                                "   case when range2 != '' then(cast(substring(range2, 1, 3) || substring(range2, 5, 3) || substring(range2, 9, 1) as int)) else 0 end as v2";
                    string condition = "WHERE   customercode=@CustomerCode AND remindinputdo=0 AND disabled=0 " +
                                         "      AND cont=substr(@ContainerNumber,1,length(cont)) " +
                                         "      AND size=substr(@Size,1,length(size)) " +
                                         "      AND type=substr(@Type,1,length(type)) " +
                                         "      AND sealdestitem.destination = @DestinationName ";

                    string query = string.Format("SELECT {0} " +
                                                " FROM destrule " +
                                                "       INNER JOIN " +
                                                "       sealdest ON kode IN(kodeallow, kodeallow2, kodeallow3) " +
                                                "       INNER JOIN " +
                                                "       sealdestitem ON sealdest.sealdestid = sealdestitem.sealdestid " +
                                                " WHERE {1} " +
                                                " UNION ALL " + 
                                                "SELECT {0} " +
                                                " FROM destrule " +
                                                "       INNER JOIN " +
                                                "      sealdest ON kode IN (kodedeny) " +
                                                "       INNER JOIN " +
                                                "       sealdestitem ON sealdest.sealdestid = sealdestitem.sealdestid " +
                                                " WHERE {1} " + 
                                         " ORDER BY destruleid ", querySelect, condition);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@CustomerCode", customerCode);
                        npgsqlCommand.Parameters.AddWithValue("@ContainerNumber", containerNumber);
                        npgsqlCommand.Parameters.AddWithValue("@Size", size);
                        npgsqlCommand.Parameters.AddWithValue("@Type", type);
                        npgsqlCommand.Parameters.AddWithValue("@DestinationName", destinationName);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                // Cek Deny
                                if (npgsqlDataReader.GetString(8).Length > 0)
                                {
                                    // Cek WithRange
                                    if (npgsqlDataReader.GetInt32(12) == 0)
                                    {
                                        DestinationRule destinationRule = new DestinationRule();
                                        MappingDataReaderToDestinationRule(npgsqlDataReader, destinationRule);
                                        listDestinationRule.Add(destinationRule);
                                    }
                                    else
                                    {
                                        if ((npgsqlDataReader.GetInt32(15) >= npgsqlDataReader.GetInt32(16)) && (npgsqlDataReader.GetInt32(15) <= npgsqlDataReader.GetInt32(17)))
                                        {
                                            DestinationRule destinationRule = new DestinationRule();
                                            MappingDataReaderToDestinationRule(npgsqlDataReader, destinationRule);
                                            listDestinationRule.Add(destinationRule);
                                        }
                                    }
                                }
                                else // Cek Allow/Deny
                                    if (npgsqlDataReader.GetString(7).Length > 0)
                                {
                                    if ((npgsqlDataReader.GetInt32(15) < npgsqlDataReader.GetInt32(16)) || (npgsqlDataReader.GetInt32(15) > npgsqlDataReader.GetInt32(17)))
                                    {
                                        DestinationRule destinationRule = new DestinationRule();
                                        MappingDataReaderToDestinationRule(npgsqlDataReader, destinationRule);
                                        listDestinationRule.Add(destinationRule);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listDestinationRule;
        }
    }
}
