using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class DurationRuleDAL
    {
        public const string DEFAULT_COLUMN = " durationruleid,customercode,cont,size,type,dtmstart,dtmend,minduration,disabled,remark ";
        public const string DEFAULT_TABLE = "durationrule";

        private void MappingDataReaderToDurationRule(NpgsqlDataReader npgsqlDataReader, DurationRule durationRule)
        {
            durationRule.DurationRuleId = npgsqlDataReader.GetInt64(0);
            durationRule.CustomerCode = npgsqlDataReader.GetString(1);
            durationRule.Cont = npgsqlDataReader.GetString(2);
            durationRule.Size = npgsqlDataReader.GetString(3);
            durationRule.Type = npgsqlDataReader.GetString(4);
            durationRule.DtmStart = npgsqlDataReader.GetDateTime(5);
            durationRule.DtmEnd = npgsqlDataReader.GetDateTime(6);
            durationRule.MinDuration = npgsqlDataReader.GetInt32(7);
            durationRule.Disabled = npgsqlDataReader.GetInt32(8) == 0 ? false : true;
            durationRule.Remark = npgsqlDataReader.GetString(9);            
        }

        /// <summary>
        /// Get Duration Rule For Out
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="container"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<DurationRule> GetDurationRuleForOut(string customerCode, string container, string size, string type)
        {
            List<DurationRule> listDurationRule = new List<DurationRule>();

            string message = string.Empty;
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
                                         " WHERE customercode=@CustomerCode " + 
                                         "  AND dtmstart<= NOW() AND dtmend> NOW() " +
                                         "  AND disabled=0 AND cont=substr(@Container,1,length(cont)) " + 
                                         "  AND size=substr(@Size,1,length(size)) " + 
                                         "  AND type=substr(@Type,1,length(type)) " +
                                         "ORDER BY durationruleid ", 
                                         DEFAULT_COLUMN,
                                         DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@CustomerCode", customerCode);
                        npgsqlCommand.Parameters.AddWithValue("@Container", container);
                        npgsqlCommand.Parameters.AddWithValue("@Size", size);
                        npgsqlCommand.Parameters.AddWithValue("@Type", type);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                DurationRule durationRule = new DurationRule();
                                MappingDataReaderToDurationRule(npgsqlDataReader, durationRule);
                                listDurationRule.Add(durationRule);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listDurationRule;
        }
    }
}
