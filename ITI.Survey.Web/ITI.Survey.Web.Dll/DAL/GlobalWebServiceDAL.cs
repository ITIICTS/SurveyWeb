﻿using ITI.Survey.Web.Dll.Helper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.DAL
{
    public class GlobalWebServiceDAL
    {
        public List<string> GetAllOperatorName()
        {
            List<string> listString = new List<string>();
            using (NpgsqlConnection npgsqlConnection = AppConfig.GetLoginConnection())
            {
                npgsqlConnection.Open();
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

    }
}
