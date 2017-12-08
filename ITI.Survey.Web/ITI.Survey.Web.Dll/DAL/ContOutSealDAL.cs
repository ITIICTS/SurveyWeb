using System;
using ITI.Survey.Web.Dll.Helper;
using Npgsql;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class ContOutSealDAL
    {
        public const string DEFAULT_COLUMN = " contoutsealid, continoutid, seal ";
        public const string DEFAULT_TABLE = "contoutseal";

        public string CheckSealUsedByOtherContainer(string seal, string customerCode)
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

                    string query = "SELECT cont " +
                                    "FROM continout c " +
                                    "   JOIN contoutseal ON contoutseal.continoutid=continout.continoutid " +
                                    "WHERE customercode=@CustomerCode AND contoutseal.seal=@Seal " +
                                    "       AND dtmout + interval '60 day' >= now() ";
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@CustomerCode", customerCode);
                        npgsqlCommand.Parameters.AddWithValue("@Seal", seal);
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
