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
    public class BlokDAL
    {
        public const string DEFAULT_COLUMN = "blokid,kode,disabled,maxtier,vpad,remark,size,xpos,ypos,cont,cont2";
        public const string DEFAULT_TABLE = "blok";

        private void MappingDataReaderToBlok(NpgsqlDataReader npgsqlDataReader, Blok blok)
        {
            blok.BlokId = npgsqlDataReader.GetInt64(0);
            blok.Kode = npgsqlDataReader.GetString(1);
            blok.Disabled = npgsqlDataReader.GetInt32(2) == 0 ? false : true;
            blok.MaxTier = npgsqlDataReader.GetInt32(3);
            blok.VPad = npgsqlDataReader.GetInt32(4);
            blok.Remark = npgsqlDataReader.GetString(5);
            blok.Size = npgsqlDataReader.GetString(6);
            blok.XPos = npgsqlDataReader.GetInt32(7);
            blok.YPos = npgsqlDataReader.GetInt32(8);
            blok.Cont = npgsqlDataReader.GetString(9);
            blok.Cont2 = npgsqlDataReader.GetString(10);
        }

        /// <summary>
        /// Fill Blok By Kode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Blok FillBlokByKode(string code)
        {
            Blok blok = new Blok();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} FROM {1} WHERE kode=@Code ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@Code", code);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                MappingDataReaderToBlok(npgsqlDataReader, blok);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return blok;
        }

        /// <summary>
        /// Update Blok
        /// </summary>
        /// <param name="blok"></param>
        /// <returns></returns>
        public int UpdateBlok(Blok blok)
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
                    string query = "UPDATE blok " +
                                    "   SET kode=@Kode,disabled=@Disabled,maxtier=@MaxTier,remark=@Remark," +
                                    "       size=@Size,vpad=@VPad,xpos=@XPos,ypos=@YPos,cont=@Cont,cont2=@Cont2 " +
                                    "   WHERE blokid=@BlokId ";
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@BlokId", blok.BlokId);
                        npgsqlCommand.Parameters.AddWithValue("@Kode", blok.Kode);
                        npgsqlCommand.Parameters.AddWithValue("@Disabled", blok.Disabled);
                        npgsqlCommand.Parameters.AddWithValue("@MaxTier", blok.MaxTier);
                        npgsqlCommand.Parameters.AddWithValue("@Remark", blok.Remark);
                        npgsqlCommand.Parameters.AddWithValue("@Size", blok.Size);
                        npgsqlCommand.Parameters.AddWithValue("@VPad", blok.VPad);
                        npgsqlCommand.Parameters.AddWithValue("@XPos", blok.XPos);
                        npgsqlCommand.Parameters.AddWithValue("@YPos", blok.YPos);
                        npgsqlCommand.Parameters.AddWithValue("@Cont", blok.Cont);
                        npgsqlCommand.Parameters.AddWithValue("@Cont2", blok.Cont2);
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
