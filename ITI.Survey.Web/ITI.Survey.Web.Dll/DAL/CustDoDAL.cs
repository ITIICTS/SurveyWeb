using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class CustDoDAL
    {
        public const string DEFAULT_COLUMN = "custdoid,dtmdo,donumber,customercode,shipper,vesselvoyage,vesselvoyagename,destination,destinationname,remarks,definedcont, " +
                                                "cont20,cont40,cont45, textflag,actout,kodekasir,exbatalrealshipper,remark2,dtmstartout,businessunit,duration,region,commodity, " +
                                                " \"EmklContactPersonId\",\"FreeUseDays\",vendorangkutanout ";
        public const string DEFAULT_TABLE = "custdo";

        private void MappingDataReaderToCustDo(NpgsqlDataReader npgsqlDataReader, CustDo custDo)
        {
            custDo.CustDoId = npgsqlDataReader.GetInt64(0);
            custDo.DtmDo = npgsqlDataReader.GetDateTime(1);
            custDo.DoNumber = npgsqlDataReader.GetString(2);
            custDo.CustomerCode = npgsqlDataReader.GetString(3);
            custDo.Shipper = npgsqlDataReader.GetString(4);
            custDo.VesselVoyage = npgsqlDataReader.GetString(5);
            custDo.VesselVoyageName = npgsqlDataReader.GetString(6);
            custDo.Destination = npgsqlDataReader.GetString(7);
            custDo.DestinationName = npgsqlDataReader.GetString(8);
            custDo.Remarks = npgsqlDataReader.GetString(9);
            custDo.DefinedCont = new DefinedContainer(npgsqlDataReader.GetString(10)).ToString();
            custDo.Cont20 = new ContainerSpecification(npgsqlDataReader.GetString(11)).ToString();
            custDo.Cont40 = new ContainerSpecification(npgsqlDataReader.GetString(12)).ToString();
            custDo.Cont45 = new ContainerSpecification(npgsqlDataReader.GetString(13)).ToString() ;
            custDo.Flag = npgsqlDataReader.GetString(14);
            custDo.ActOut = npgsqlDataReader.GetString(15);
            custDo.KodeKasir = npgsqlDataReader.GetString(16);
            custDo.ExBatalRealShipper = npgsqlDataReader.GetString(17);
            custDo.Remark2 = npgsqlDataReader.GetString(18);
            custDo.DtmStartOut = npgsqlDataReader.GetDateTime(19);
            custDo.BusinessUnit = npgsqlDataReader.GetString(20);
            custDo.Duration = npgsqlDataReader.GetInt32(21);
            custDo.Region = npgsqlDataReader.GetString(22);
            custDo.Commodity = npgsqlDataReader.GetString(23);

            custDo.EmklContactPersonId = npgsqlDataReader.GetInt64(24);

            custDo.FreeUseDays = npgsqlDataReader.GetInt32(25);

            custDo.VendorAngkutanOut = npgsqlDataReader.GetString(26);
        }

        /// <summary>
        /// Fill Customer DO By Customer DO ID
        /// </summary>
        /// <param name="custDoId"></param>
        /// <returns></returns>
        public CustDo FillCustDoByCustDoId(long custDoId)
        {
            CustDo custDo = new CustDo();
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
                                                " WHERE custdoid=@CustDoId ",
                                                DEFAULT_COLUMN,
                                                DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@CustDoId", custDoId);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                MappingDataReaderToCustDo(npgsqlDataReader, custDo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return custDo;
        }
    }
}
