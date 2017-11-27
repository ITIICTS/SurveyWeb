using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ContCard FillContCardByContCardIdAndCardMode(long contCardId, string cardMode)
        {
            ContCard contCard = new ContCard();
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    npgsqlConnection.Open();
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
    }
}
