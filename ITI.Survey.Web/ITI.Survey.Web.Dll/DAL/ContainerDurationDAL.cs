using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class ContainerDurationDAL
    {
        long id = 0;

        /// <summary>
        /// Mapping Data Reader To Container Card
        /// </summary>
        /// <param name="npgsqlDataReader"></param>
        /// <param name="containerDuration"></param>
        private void MappingDataReaderToContainerDuration(NpgsqlDataReader npgsqlDataReader, ContainerDuration containerDuration)
        {
            containerDuration.ContInOutId = npgsqlDataReader.GetInt64(0);
            id = containerDuration.ContInOutId;
            containerDuration.Cont = npgsqlDataReader.GetString(1);
            containerDuration.Size = npgsqlDataReader.GetString(2);
            containerDuration.Type = npgsqlDataReader.GetString(3);
            containerDuration.CustomerCode = npgsqlDataReader.GetString(4);
            containerDuration.Condition = npgsqlDataReader.GetString(5);
            containerDuration.DtmIn = npgsqlDataReader.GetDateTime(6).ToString("yyyy-MM-dd HH:mm:ss");
            containerDuration.RfEngineCond = npgsqlDataReader.GetString(7);
            containerDuration.Remarks = npgsqlDataReader.GetString(14) + " " 
                                        + npgsqlDataReader.GetString(15) + " " 
                                        + npgsqlDataReader.GetString(8).Replace("<DSW>", " ");
            containerDuration.Duration = (int)npgsqlDataReader.GetDouble(9);
            containerDuration.EorNo = npgsqlDataReader.GetString(10);
            containerDuration.ActIn = npgsqlDataReader.GetString(16);
            if (containerDuration.EorNo.Length > 0)
            {
                containerDuration.RepairStatus = "WA";
                containerDuration.DtmEor = npgsqlDataReader.GetDateTime(11).ToString("yyyy-MM-dd HH:mm:ss");
                if (npgsqlDataReader.GetValue(12) != DBNull.Value)
                {
                    containerDuration.DtmApproved = npgsqlDataReader.GetDateTime(12).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (npgsqlDataReader.GetValue(13) != DBNull.Value)
                {
                    containerDuration.DtmCompleted = npgsqlDataReader.GetDateTime(13).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (containerDuration.DtmApproved.Length > 0) containerDuration.RepairStatus = "AP";
                if (containerDuration.DtmCompleted.Length > 0) containerDuration.RepairStatus = "CR";
            }
        }

        /// <summary>
        /// Fill Container Duration By Criteria
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <param name="condition"></param>
        /// <param name="minDuration"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public List<ContainerDuration> FillByCriteria(string customerCode, string size, string type, string condition, int minDuration, string sortBy)
        {
            List<ContainerDuration> listContainerDuration = new List<ContainerDuration>();
            if (sortBy.Length == 0)
            {
                sortBy = "c.continoutid";
            }
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetUserConnection())
                {
                    if(npgsqlConnection.State == ConnectionState.Closed)
                    {
                        npgsqlConnection.Open();
                    }
                    string query = "SELECT c.continoutid,c.cont,c.size,c.type,c.customercode,c.condition,c.dtmin,c.rfenginecond,c.remarks, " +
                                    "   (extract( day from date_trunc('day',now())-date_trunc('day',c.dtmin) ) + 1) as dur, " +
                                    "   COALESCE(e.eorno,'') AS eorno, e.dtmeor, e.dtmapproved, e.dtmcompleted, c.payload, c.manufacture, c.actin " +
                                    "FROM continout c " + 
                                    "       LEFT JOIN eor e ON e.continoutid=c.continoutid " +
                                    "WHERE c.dtmout IS NULL AND c.customercode=@CustomerCode AND c.size=@Size AND c.type=@Type AND c.condition LIKE @Condition " +
                                    "   AND (extract( day from date_trunc('day',now())-date_trunc('day',c.dtmin) ) + 1) >= @MinDuration " +
                                    "ORDER BY " + sortBy;
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@CustomerCode", customerCode);
                        npgsqlCommand.Parameters.AddWithValue("@Size", size);
                        npgsqlCommand.Parameters.AddWithValue("@Type", type);
                        npgsqlCommand.Parameters.AddWithValue("@Condition", condition + "%");
                        npgsqlCommand.Parameters.AddWithValue("@MinDuration", minDuration);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {                            
                            while (npgsqlDataReader.Read())
                            {
                                ContainerDuration containerDuration = new ContainerDuration();
                                MappingDataReaderToContainerDuration(npgsqlDataReader, containerDuration);
                                listContainerDuration.Add(containerDuration);
                            }
                        }
                    }
                }
            }
            catch(NpgsqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listContainerDuration;
        }
    }
}
