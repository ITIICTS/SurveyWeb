using System;
using ITI.Survey.Web.Dll.Helper;
using Npgsql;
using System.Data;


namespace ITI.Survey.Web.Dll.DAL
{
    public class CustDoDefinedSealDAL
    {
        public const string DEFAULT_COLUMN = " custdodefinedsealid, custdoid, seal, noserior ";
        public const string DEFAULT_TABLE = "custdodefinedseal";

        public bool IsSealExistInCustDoDefinedSeal(long contInOutId, string seal)
        {
            bool result = false;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }

                    string query = string.Format("SELECT(COUNT(continoutid) > 0 AND " +
                                                  "          SUM(CASE WHEN custdodefinedseal.seal = @Seal THEN 1 ELSE 0 END) > 0) AS isexist " +
                                                  "  FROM " +
                                                  "      custdo " +
                                                  "      INNER JOIN " +
                                                  "      custdodefinedseal ON custdo.custdoid = custdodefinedseal.custdoid " +
                                                  "      INNER JOIN " +
                                                  "      continout ON custdo.donumber = continout.donumber " +
                                                  "  WHERE continoutid = @ContInOutId ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContInOutId", contInOutId);
                        npgsqlCommand.Parameters.AddWithValue("@Seal", seal);
                        result = Convert.ToBoolean(npgsqlCommand.ExecuteScalar());
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
