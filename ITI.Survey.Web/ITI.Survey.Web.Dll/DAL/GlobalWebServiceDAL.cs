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

        /// <summary>
        /// Check Taken Defined Container
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="noSeri"></param>
        /// <param name="containerNumber"></param>
        /// <returns></returns>
        public static string CheckTakenDefinedContainer(string customerCode, string noSeri, string containerNumber)
        {
            string result = string.Empty;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = "SELECT r.noseri,d.donumber,r.takedef,d.customercode " + 
                                    "FROM inoutrevenue r " +
                                    "   JOIN custdo d ON d.custdoid=r.refid " +
                                    "WHERE r.dtmkasir >= (now() - interval '10 day') " +
                                    "   AND r.movact='OUT' AND r.iscanceled=0 AND r.takedef LIKE @cont " +
                                    "   AND d.customercode=@cus AND r.noseri!=@noseri AND r.textflag LIKE '%PRIMOROUT%' " +
                                    "ORDER BY r.dtmkasir DESC ";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@cus", customerCode);
                        npgsqlCommand.Parameters.AddWithValue("@noseri", noSeri);
                        npgsqlCommand.Parameters.AddWithValue("@cont", "%" + containerNumber + "%");
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                string noseri1 = npgsqlDataReader.GetString(0);
                                string dono1 = npgsqlDataReader.GetString(1);
                                npgsqlDataReader.Close();

                                string query2 = "SELECT cont " + 
                                                " FROM continout " + 
                                                " WHERE cont=@cont AND (NOT dtmout IS NULL) AND customercode=@cus AND noseriorout=@noseri ";
                                using (NpgsqlCommand npgsqlCommand2 = new Npgsql.NpgsqlCommand(query2, npgsqlConnection))
                                {
                                    npgsqlCommand2.Parameters.AddWithValue("@cus", customerCode);
                                    npgsqlCommand2.Parameters.AddWithValue("@noseri", noseri1);
                                    npgsqlCommand2.Parameters.AddWithValue("@cont", containerNumber);
                                    using (NpgsqlDataReader npgsqlDataReader2 = npgsqlCommand2.ExecuteReader())
                                    {
                                        if (npgsqlDataReader2.Read())
                                        {
                                            result = string.Empty;
                                        }
                                        else
                                        {
                                            result = string.Format("{0} {1}", noseri1, dono1);
                                        }
                                        npgsqlDataReader2.Close();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
    }
}
