using System;
using ITI.Survey.Web.Dll.Helper;
using Npgsql;
using System.Data;
using ITI.Survey.Web.Dll.Model;

namespace ITI.Survey.Web.Dll.DAL
{
    public class CustDoDefinedSealDAL
    {
        public const string DEFAULT_COLUMN = " custdodefinedsealid, custdoid, seal, noserior ";
        public const string DEFAULT_TABLE = "custdodefinedseal";

        public bool IsSealExistInCustDoDefinedSeal(long custDoId, string seal)
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

                    string query = string.Format("SELECT(COUNT(custdodefinedseal.custdoid) > 0 AND " +
                                                  "          SUM(CASE WHEN custdodefinedseal.seal = @Seal THEN 1 ELSE 0 END) > 0) AS isexist " +
                                                  "  FROM " +
                                                  "      custdo " +
                                                  "      INNER JOIN " +
                                                  "      custdodefinedseal ON custdo.custdoid = custdodefinedseal.custdoid " +
                                                  "  WHERE custdo.custDoId = @CustDoId ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@CustDoId", custDoId);
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

        public int GetCountDefinedSealByInOutRevenue(InOutRevenue inOutRevenue)
        {
            int result = 0;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }

                    string query = string.Format("SELECT COUNT(*) " + 
                                                    "FROM {0} " +
                                                    "WHERE custdoid=@CustDoId AND ( noserior=@NoSeri OR noserior='' )",
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@CustDoId", Convert.ToInt64(inOutRevenue.RefId));
                        npgsqlCommand.Parameters.AddWithValue("@NoSeri", inOutRevenue.NoSeri);
                        object count = npgsqlCommand.ExecuteScalar();
                        if (count != null)
                        {
                            result = Convert.ToInt32(count);
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
