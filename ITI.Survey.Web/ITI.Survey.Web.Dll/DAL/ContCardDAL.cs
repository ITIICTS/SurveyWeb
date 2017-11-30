using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class ContCardDAL
    {
        public const string DEFAULT_COLUMN = " {0}contcardid,{0}cardmode,{0}refid,{0}cont,{0}size,{0}type,{0}dtm1,{0}loc1,{0}dtm2,{0}loc2,{0}remark,{0}dtm3,{0}continoutid,{0}userid3,{0}seal1,{0}seal2,{0}seal3,{0}seal4,{0}nomobilout,{0}angkutanout,{0}eiroutno,{0}token,{0}iscombo ";
        public const string DEFAULT_TABLE = "contcard";

        private void MappingDataReaderToContCard(NpgsqlDataReader npgsqlDataReader, ContCard contCard)
        {
            contCard.ContCardID = npgsqlDataReader.GetInt64(npgsqlDataReader.GetOrdinal("contcardid"));
            contCard.CardMode = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("cardmode"));
            contCard.RefID = npgsqlDataReader.GetInt64(npgsqlDataReader.GetOrdinal("refid"));
            contCard.Cont = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("cont"));
            contCard.Size = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("size"));
            contCard.Type = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("type"));
            if (npgsqlDataReader["dtm1"] != DBNull.Value)
            {
                contCard.Dtm1 = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtm1"));
            }
            contCard.Loc1 = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("loc1"));
            if (npgsqlDataReader["dtm2"] != DBNull.Value)
            {
                contCard.Dtm2 = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtm2"));
            }
            contCard.Loc2 = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("loc2"));
            contCard.Remark = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("remark"));
            if (npgsqlDataReader["dtm3"] != DBNull.Value)
            {
                contCard.Dtm3 = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtm3"));
            }
            contCard.ContInOutID = npgsqlDataReader.GetInt64(npgsqlDataReader.GetOrdinal("continoutid"));
            contCard.UserID3 = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("userid3"));
            contCard.Seal1 = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("seal1"));
            contCard.Seal2 = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("seal2"));
            contCard.Seal3 = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("seal3"));
            contCard.Seal4 = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("seal4"));
            contCard.NoMobilOut = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("nomobilout"));
            contCard.AngkutanOut = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("angkutanout"));
            contCard.Token = npgsqlDataReader.GetString(npgsqlDataReader.GetOrdinal("token"));
            contCard.IsCombo = npgsqlDataReader.GetBoolean(npgsqlDataReader.GetOrdinal("iscombo"));
        }

        /// <summary>
        /// Fill Container Card By contCardId
        /// </summary>
        /// <param name="contCardId"></param>
        /// <returns></returns>
        public ContCard FillContCardByContCardId(long contCardId)
        {
            ContCard contCard = new ContCard();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} FROM {1} WHERE contcardid=@ContCardId ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContCardId", contCardId);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                MappingDataReaderToContCard(npgsqlDataReader, contCard);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contCard;
        }

        /// <summary>
        /// Fill Container Card By contCardId, cardMode
        /// </summary>
        /// <param name="contCardId"></param>
        /// <param name="cardMode"></param>
        /// <returns></returns>
        public ContCard FillContCardByContCardIdAndCardMode(long contCardId, string cardMode)
        {
            ContCard contCard = new ContCard();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} FROM {1} WHERE contcardid=@ContCardId AND cardmode=@CardMode ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContCardId", contCardId);
                        npgsqlCommand.Parameters.AddWithValue("@CardMode", cardMode);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                MappingDataReaderToContCard(npgsqlDataReader, contCard);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contCard;
        }

        /// <summary>
        /// Update Container Card
        /// </summary>
        /// <param name="contCard"></param>
        /// <returns></returns>
        public int UpdateContCard(ContCard contCard)
        {
            int rowAffected = 0;
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = "    UPDATE contcard " + 
                                    "   SET  cardmode=@CardMode, refid=@RefID, cont=@Cont, size=@Size, type=@Type, " +
                                    "       dtm1=@Dtm1, loc1=@Loc1, dtm2=@Dtm2, loc2=@Loc2, remark=@Remark, dtm3=@dtm3, " + 
                                    "       continoutid=@continoutid, userid3=@userid3, " +
                                    "       seal1=@seal1, seal2=@seal2, seal3=@seal3, seal4=@seal4, "+ 
                                    "       nomobilout=@nomobilout, angkutanout=@angkutanout, " +
                                    "       eiroutno=@eiroutno, token = @Token, iscombo = @IsCombo " + 
                                    "   WHERE contcardid=@ContCardID ";
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@ContCardID", contCard.ContCardID);
                        npgsqlCommand.Parameters.AddWithValue("@CardMode", contCard.CardMode);
                        npgsqlCommand.Parameters.AddWithValue("@RefID", contCard.RefID);
                        npgsqlCommand.Parameters.AddWithValue("@Cont", contCard.Cont);
                        npgsqlCommand.Parameters.AddWithValue("@Size", contCard.Size);
                        npgsqlCommand.Parameters.AddWithValue("@Type", contCard.Type);
                        npgsqlCommand.Parameters.AddWithValue("@Dtm1", contCard.Dtm1.Value);
                        npgsqlCommand.Parameters.AddWithValue("@Loc1", contCard.Loc1);
                        npgsqlCommand.Parameters.AddWithValue("@Dtm2", contCard.Dtm2.Value);
                        npgsqlCommand.Parameters.AddWithValue("@Loc2", contCard.Loc2);
                        npgsqlCommand.Parameters.AddWithValue("@Remark", contCard.Remark);
                        npgsqlCommand.Parameters.AddWithValue("@dtm3", contCard.Dtm3.Value);
                        npgsqlCommand.Parameters.AddWithValue("@continoutid", contCard.ContInOutID);
                        npgsqlCommand.Parameters.AddWithValue("@userid3", contCard.UserID3);
                        npgsqlCommand.Parameters.AddWithValue("@seal1", contCard.Seal1);
                        npgsqlCommand.Parameters.AddWithValue("@seal2", contCard.Seal2);
                        npgsqlCommand.Parameters.AddWithValue("@seal3", contCard.Seal3);
                        npgsqlCommand.Parameters.AddWithValue("@seal4", contCard.Seal4);
                        npgsqlCommand.Parameters.AddWithValue("@nomobilout", contCard.NoMobilOut);
                        npgsqlCommand.Parameters.AddWithValue("@angkutanout", contCard.AngkutanOut);
                        npgsqlCommand.Parameters.AddWithValue("@eiroutno", contCard.EirOutNo);
                        npgsqlCommand.Parameters.AddWithValue("@Token", contCard.Token);
                        npgsqlCommand.Parameters.AddWithValue("@IsCombo", contCard.IsCombo);
                        rowAffected = npgsqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rowAffected;
        }
    }
}
