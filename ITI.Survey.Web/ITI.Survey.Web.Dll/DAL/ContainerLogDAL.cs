using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class ContainerLogDAL
    {
        public const string DEFAULT_COLUMN = "contlogid, continoutid, cont, userid, eqpid, flag_act, location, dtm, shipper,operator";
        public const string DEFAULT_TABLE = "contlog";

        /// <summary>
        /// Insert Container Log
        /// </summary>
        /// <param name="containerLog"></param>
        /// <returns></returns>
        public int InsertContainerLog(ContainerLog containerLog)
        {
            int affectedRows = 0;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = "INSERT INTO contlog(contlogid,continoutid,cont,userid,eqpid,flag_act,location,dtm,shipper,operator) " +
                           "VALUES(nextval('contlog_contlogid_seq'),@continoutid,@cont,@userid,@eqpid,@flag_act,@location,@dtm,@shipper,@operator) ";
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@contlogid", containerLog.ContainerLogId);
                        npgsqlCommand.Parameters.AddWithValue("@continoutid", containerLog.ContInOutId);
                        npgsqlCommand.Parameters.AddWithValue("@cont", containerLog.Cont);
                        npgsqlCommand.Parameters.AddWithValue("@userid", containerLog.UserId);
                        npgsqlCommand.Parameters.AddWithValue("@eqpid", containerLog.EqpId);
                        npgsqlCommand.Parameters.AddWithValue("@flag_act", containerLog.FlagAct);
                        npgsqlCommand.Parameters.AddWithValue("@location", containerLog.Location);
                        if (string.IsNullOrWhiteSpace(containerLog.Dtm))
                            npgsqlCommand.Parameters.AddWithValue("@dtm", DBNull.Value);
                        else
                            npgsqlCommand.Parameters.AddWithValue("@dtm", Convert.ToDateTime(containerLog.Dtm));

                        npgsqlCommand.Parameters.AddWithValue("@shipper", containerLog.Shipper);
                        npgsqlCommand.Parameters.AddWithValue("@operator", containerLog.Operator);
                        affectedRows = npgsqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return affectedRows;
        }
    }
}
