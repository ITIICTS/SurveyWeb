using System;
using ITI.Survey.Web.Dll.Helper;
using Npgsql;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class SealDestinationItemDAL
    {
        //SELECT sealdestitemid,sealdestid,destination FROM sealdestitem WHERE sealdestid=@id AND destination=@dest ORDER BY destination 

        public const string DEFAULT_COLUMN = " sealdestitemid,sealdestid,destination ";
        public const string DEFAULT_TABLE = "sealdestitem";

        public int CountSealDestinationItemByDestinationCodeAndSealDestinationId(string destinationCode, long sealDestinationId)
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


                    string query = string.Format("SELECT COUNT(*)  " +
                                                " FROM {1} " +
                                                "  WHERE sealdestid=@SealDestinationId AND destination=@DestinationCode " +
                                                " ORDER BY destination ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@DestinationCode", destinationCode);
                        npgsqlCommand.Parameters.AddWithValue("@SealDestinationId", sealDestinationId);
                        result = Convert.ToInt32(npgsqlCommand.ExecuteScalar());
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
