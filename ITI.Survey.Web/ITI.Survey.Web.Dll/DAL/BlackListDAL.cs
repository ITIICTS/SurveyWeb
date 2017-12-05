using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.DAL
{
    public class BlackListDAL
    {
        public const string DEFAULT_COLUMN = " blacklistid,dtmcreate,cont,message,disabled,lastreleaseinfo,textflag,disableduntil ";
        public const string DEFAULT_TABLE = "blacklist";

        private void MappingDataReaderToBlackList(NpgsqlDataReader npgsqlDataReader, BlackList blackList)
        {
            blackList.BlackListId = npgsqlDataReader.GetInt64(0);
            blackList.DtmCreate = npgsqlDataReader.GetDateTime(1);
            blackList.ContainerNumber = npgsqlDataReader.GetString(2);
            blackList.Message = npgsqlDataReader.GetString(3);
            blackList.Disabled = npgsqlDataReader.GetInt32(4) == 0 ? false : true;
            blackList.LastReleaseInfo = npgsqlDataReader.GetString(5);
            blackList.TextFlag = npgsqlDataReader.GetString(6);
            blackList.DisabledUntil = npgsqlDataReader.GetDateTime(7);
        }

        private void MappingDataReaderFromBlackList2ToBlackList(NpgsqlDataReader npgsqlDataReader, BlackList blackList)
        {
            blackList.BlackListId = npgsqlDataReader.GetInt64(0);
            blackList.DtmCreate = npgsqlDataReader.GetDateTime(1);
            blackList.ContainerNumber = npgsqlDataReader.GetString(2) + " - " + npgsqlDataReader.GetString(3);
            blackList.Message = npgsqlDataReader.GetString(4);
            blackList.Disabled = npgsqlDataReader.GetInt32(5) == 0 ? false : true;
            blackList.LastReleaseInfo = npgsqlDataReader.GetString(6);
            blackList.TextFlag = npgsqlDataReader.GetString(7);
            blackList.DisabledUntil = npgsqlDataReader.GetDateTime(8);
        }

        /// <summary>
        /// Get Message by Container Number
        /// </summary>
        /// <param name="containerNumber"></param>
        /// <returns></returns>
        public string GetMessageByContainerNumber(string containerNumber)
        {
            if (string.IsNullOrEmpty(containerNumber))
            {
                return string.Empty;
            }
            string message = string.Empty;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT message FROM {0} WHERE cont = @ContainerNumber and disableduntil <= now()", DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new Npgsql.NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContainerNumber", containerNumber);
                        object result = npgsqlCommand.ExecuteScalar();
                        if (result != null)
                        {
                            message = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }

        /// <summary>
        /// Get Black List by Container Number
        /// </summary>
        /// <param name="containerNumber"></param>
        /// <returns></returns>
        public List<BlackList> GetBlackListByContainerNumber(string containerNumber)
        {
            List<BlackList> listBlackList = new List<BlackList>();
            if (string.IsNullOrEmpty(containerNumber))
            {
                return listBlackList;
            }
            string message = string.Empty;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} FROM {1} WHERE cont = @ContainerNumber and disableduntil <= now()", DEFAULT_COLUMN, DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContainerNumber", containerNumber);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                BlackList blackList = new BlackList();
                                MappingDataReaderToBlackList(npgsqlDataReader, blackList);
                                listBlackList.Add(blackList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listBlackList;
        }

        /// <summary>
        /// Get Black List 2 By Container Number
        /// </summary>
        /// <param name="containerNumber"></param>
        /// <returns></returns>
        public List<BlackList> GetBlackList2ByContainerNumber(string containerNumber)
        {
            containerNumber = containerNumber.Trim();


            List<BlackList> listBlackList = new List<BlackList>();
            if (string.IsNullOrEmpty(containerNumber))
            {
                return listBlackList;
            }
            string message = string.Empty;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT blacklist2id,dtmcreate,contfrom,contto,message,disabled,lastreleaseinfo,textflag,disableduntil " + 
                                                "   FROM blacklist2 " +
                                                "   WHERE contfrom<=@ContainerNumber AND contto>=@ContainerNumber " + 
                                                "       AND length(@ContainerNumber)=length(contfrom) AND length(@ContainerNumber)=length(contto) " + 
                                                "   ORDER BY dtmcreate", DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContainerNumber", containerNumber);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                BlackList blackList = new BlackList();
                                MappingDataReaderToBlackList(npgsqlDataReader, blackList);

                                listBlackList.Add(blackList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listBlackList;
        }
    }
}
