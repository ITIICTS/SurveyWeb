using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class DefinedContSizeTypeDAL
    {
        public const string DEFAULT_COLUMN = "definedcontsizetypeid,inoutrevenueid,cont,size,type";
        public const string DEFAULT_TABLE = "definedcontsizetype";

        private void MappingDataReaderToDefinedContSizeType(NpgsqlDataReader npgsqlDataReader, DefinedContSizeType definedContSizeType)
        {
            definedContSizeType.DefinedContSizeTypeId = npgsqlDataReader.GetInt64(0);
            definedContSizeType.InOutRevenueId = npgsqlDataReader.GetInt64(1);
            definedContSizeType.Cont = npgsqlDataReader.GetString(2);
            definedContSizeType.Size = npgsqlDataReader.GetString(3);
            definedContSizeType.Type = npgsqlDataReader.GetString(4);
        }

        /// <summary>
        /// Fill Customer DO By Customer DO ID
        /// </summary>
        /// <param name="definedContSizeTypeId"></param>
        /// <returns></returns>
        public DefinedContSizeType FillDefinedContSizeTypeByContainerNumberAndInOutRevenue(string containerNumber, InOutRevenue inOutRevenue)
        {
            DefinedContSizeType definedContSizeType = new DefinedContSizeType(inOutRevenue);
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if (npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = string.Format("SELECT {0} " +
                                                " FROM {1} " +
                                                " WHERE definedcontsizetypeid=@DefinedContSizeTypeId AND cont=@ContainerNumber ",
                                                DEFAULT_COLUMN,
                                                DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@DefinedContSizeTypeId", inOutRevenue.InOutRevenueId);
                        npgsqlCommand.Parameters.AddWithValue("@ContainerNumber", containerNumber);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                MappingDataReaderToDefinedContSizeType(npgsqlDataReader, definedContSizeType);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return definedContSizeType;
        }
    }
}
