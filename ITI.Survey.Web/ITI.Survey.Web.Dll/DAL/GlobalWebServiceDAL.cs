using ITI.Survey.Web.Dll.Helper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class GlobalWebServiceDAL
    {
        public List<string> GetAllOperatorName()
        {
            List<string> listString = new List<string>();
            using (NpgsqlConnection npgsqlConnection = AppConfig.GetLoginConnection())
            {
                if (npgsqlConnection.State == ConnectionState.Closed)
                {
                    npgsqlConnection.Open();
                }
                string query = "SELECT userlogin.userid " +
                                "FROM userlogin " +
                                        "INNER JOIN usergroup on userlogin.userid = usergroup.userid " +
                                        "INNER JOIN groupperm on usergroup.groupname = groupperm.groupname " +
                                                "AND groupperm.action = @Action " +
                                                "AND userlogin.disabled = '0' order by userlogin.userid";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@Action", "Operator Special");
                    using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                    {
                        while (npgsqlDataReader.Read())
                        {
                            listString.Add(npgsqlDataReader.GetString(0));
                        }
                    }
                }
            }
            return listString;
        }

        public List<string> GetAllCustomerCode()
        {
            List<string> listString = new List<string>();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetConnection())
                {
                    npgsqlConnection.Open();
                    string query = "SELECT customercode FROM customer ORDER BY customercode";
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            while (npgsqlDataReader.Read())
                            {
                                listString.Add(npgsqlDataReader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listString = new List<string>();
                throw ex;
            }
            return listString;
        }

        public static DateTime GetServerDtm()
        {
            DateTime serverDateTime = DateTime.Now;
            using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
            {
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand("SELECT now() ", npgsqlConnection);
                serverDateTime = Convert.ToDateTime(npgsqlCommand.ExecuteScalar());
            }
            return serverDateTime;
        }
    }
}
