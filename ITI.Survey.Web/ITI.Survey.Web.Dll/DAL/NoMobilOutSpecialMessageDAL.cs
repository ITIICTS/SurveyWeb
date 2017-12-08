using ITI.Survey.Web.Dll.Helper;
using Npgsql;
using System;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class NoMobilOutSpecialMessageDAL
    {
        public string GetNoMobilOutSpecialMessage(string noMobilOut)
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

                    string query = "SELECT message " +
                           "FROM nomobiloutspmsg " +
                           "WHERE nomobilout = @NoMobilOut AND disabled = 0 " +
                           "ORDER BY nomobiloutspmsgid DESC";
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@NoMobilOut", noMobilOut);
                        object cont = npgsqlCommand.ExecuteScalar();
                        if (cont != null)
                        {
                            result = Convert.ToString(cont);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
