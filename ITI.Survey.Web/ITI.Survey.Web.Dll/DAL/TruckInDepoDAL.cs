using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.DAL
{
    public class TruckInDepoDAL
    {
        public const string DEFAULT_COLUMN = "truckindepoid, nomobil, dtmin, dtmout, remark, refno, muatan, tipe, angkutan, note";
        public const string DEFAULT_TABLE = "truckindepo";

        /// <summary>
        /// Insert Truck In Depo
        /// </summary>
        /// <param name="truckInDepo"></param>
        /// <returns></returns>
        public int InsertTruckInDepo(TruckInDepo truckInDepo)
        {
            int affectedRow = 0;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("INSERT INTO {1}({0}) " +
                                                " VALUES(nextval('truckindepo_truckindepoid_seq'), @NoMobil, @DtmIn, @DtmOut, @Remark, @RefNo, " + 
                                                "       @Muatan, @Ttipe, @Angkutan, @Note)",
                                                DEFAULT_COLUMN,DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@NoMobil", truckInDepo.NoMobil);
                        npgsqlCommand.Parameters.AddWithValue("@DtmIn", truckInDepo.DtmIn);
                        if (truckInDepo.DtmOut.HasValue)
                        {
                            npgsqlCommand.Parameters.AddWithValue("@DtmOut", truckInDepo.DtmOut);
                        }
                        else
                        {
                            npgsqlCommand.Parameters.AddWithValue("@DtmOut", DBNull.Value);
                        }
                        npgsqlCommand.Parameters.AddWithValue("@Remark", truckInDepo.Remark);
                        npgsqlCommand.Parameters.AddWithValue("@RefNo", truckInDepo.RefNo);
                        npgsqlCommand.Parameters.AddWithValue("@Muatan", truckInDepo.Muatan);
                        npgsqlCommand.Parameters.AddWithValue("@Tipe", truckInDepo.Tipe);
                        npgsqlCommand.Parameters.AddWithValue("@Angkutan", truckInDepo.Angkutan);
                        npgsqlCommand.Parameters.AddWithValue("@Note", truckInDepo.Note);
                        affectedRow = npgsqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return affectedRow;
        }
    }
}
