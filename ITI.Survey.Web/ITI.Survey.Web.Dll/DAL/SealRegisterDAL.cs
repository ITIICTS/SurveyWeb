using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class SealRegisterDAL
    {
        public const string DEFAULT_COLUMN = " sealregid,name,customercode,sealfrom,sealto,dtmcreate,sealdestkode ";
        public const string DEFAULT_TABLE = "sealreg";

        public bool IsSealRegistered(string seal, string customerCode)
        {
            bool isRegistered = false;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }

                    string query = string.Format("SELECT CASE WHEN COUNT (*) > 0 THEN true ELSE false END  " +
                                                " FROM {1} " +
                                                " WHERE customercode=@CustomerCode AND sealfrom<=@Seal AND sealto>=@Seal AND length(sealfrom)=length(@Seal) AND length(sealto)=length(@Seal) ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@Seal", seal);
                        npgsqlCommand.Parameters.AddWithValue("@CustomerCode", customerCode);
                        isRegistered = Convert.ToBoolean(npgsqlCommand.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isRegistered;
        }

        public string GetSealDestinationCodeBySealAndCustomerCode(string seal, string customerCode)
        {
            string sealDestinationCode = string.Empty;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }

                    string query = string.Format("SELECT sealdestkode  " +
                                                " FROM {1} " +
                                                " WHERE customercode=@CustomerCode AND sealfrom<=@Seal AND sealto>=@Seal AND length(sealfrom)=length(@Seal) AND length(sealto)=length(@Seal) ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@Seal", seal);
                        npgsqlCommand.Parameters.AddWithValue("@CustomerCode", customerCode);
                        object sealDestKode = npgsqlCommand.ExecuteScalar();
                        if(sealDestinationCode != null)
                        {
                            sealDestinationCode = Convert.ToString(sealDestinationCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sealDestinationCode;
        }
    }
}
