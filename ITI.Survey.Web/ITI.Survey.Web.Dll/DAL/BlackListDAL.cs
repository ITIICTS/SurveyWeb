using ITI.Survey.Web.Dll.Helper;
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

        public string GetMessageByContNumber(string containerNumber)
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
    }
}
