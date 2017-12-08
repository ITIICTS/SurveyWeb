using ITI.Survey.Web.Dll.Helper;
using Npgsql;
using System;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class SealDestinationDAL
    {
        public const string DEFAULT_COLUMN = " sealdestid, kode, remark ";
        public const string DEFAULT_TABLE = "sealdest";

        public long GetSealDestinationIdByDestinationCode(string destinationCode)
        {
            long sealDestinationId = 0;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }

                    
                    string query = string.Format("SELECT sealdestid  " +
                                                " FROM {1} " +
                                                " WHERE kode=@DestinationCode " + 
                                                " ORDER BY sealdestid DESC ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@DestinationCode", destinationCode);
                        sealDestinationId = Convert.ToInt64(npgsqlCommand.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sealDestinationId;
        }
    }
}
