using ITI.Survey.Web.Dll.Helper;
using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Data;

namespace ITI.Survey.Web.Dll.DAL
{
    public class InOutRevenueDAL
    {
        public const string DEFAULT_COLUMN = " {0}inoutrevenueid, {0}refid, {0}movact, {0}revenuetype, {0}billto, {0}liftoff, {0}liftoffinfo, {0}lifton, " +
                                         "{0}liftoninfo, {0}wash, {0}washinfo, {0}admin, {0}admininfo, {0}storage, {0}storageinfo, {0}textflag, {0}noseri, " +
                                         "{0}qty, {0}iscanceled, {0}contactpersonout, {0}liftoffsplit, {0}liftonsplit, {0}washsplit, {0}kodekasir, {0}invby, " +
                                         "{0}kasirby, {0}dtminv, {0}dtmkasir, {0}takedef, {0}take20, {0}take40, {0}take45, {0}kasirnote, {0}principaladmin, " +
                                         "{0}principalliftoff, {0}principallifton, {0}principalwash, {0}principalstorage,  {0}spnote1, {0}ppn, " +
                                         "{0}seal, {0}preparation, {0}token, {0}pph23, {0}noserifaktur ";
        public const string DEFAULT_TABLE = "inoutrevenue";

        private void MappingDataReaderToInOutRevenue(NpgsqlDataReader npgsqlDataReader, InOutRevenue inOutRevenue)
        {
            inOutRevenue.InOutRevenueId = npgsqlDataReader.GetInt64(0);
            inOutRevenue.RefId = npgsqlDataReader.GetInt64(1);
            inOutRevenue.MovAct = npgsqlDataReader.GetString(2);
            inOutRevenue.RevenueType = npgsqlDataReader.GetString(3);
            inOutRevenue.BillTo = npgsqlDataReader.GetString(4);
            inOutRevenue.LiftOff = npgsqlDataReader.GetDouble(5);
            inOutRevenue.LiftOffInfo = npgsqlDataReader.GetString(6);
            inOutRevenue.LiftOn = npgsqlDataReader.GetDouble(7);
            inOutRevenue.LiftOnInfo = npgsqlDataReader.GetString(8);
            inOutRevenue.Wash = npgsqlDataReader.GetDouble(9);
            inOutRevenue.WashInfo = npgsqlDataReader.GetString(10);
            inOutRevenue.Admin = npgsqlDataReader.GetDouble(11);
            inOutRevenue.AdminInfo = npgsqlDataReader.GetString(12);
            inOutRevenue.Storage = npgsqlDataReader.GetDouble(13);
            inOutRevenue.StorageInfo = npgsqlDataReader.GetString(14);
            inOutRevenue.TextFlag = npgsqlDataReader.GetString(15);
            inOutRevenue.NoSeri = npgsqlDataReader.GetString(16);
            inOutRevenue.Qty = npgsqlDataReader.GetInt32(17);
            inOutRevenue.IsCanceled = npgsqlDataReader.GetInt32(18);
            inOutRevenue.ContactPersonOut = npgsqlDataReader.GetString(19);
            inOutRevenue.LiftOffSplit = npgsqlDataReader.GetDouble(20);
            inOutRevenue.LiftOnSplit = npgsqlDataReader.GetDouble(21);
            inOutRevenue.WashSplit = npgsqlDataReader.GetDouble(22);
            inOutRevenue.KodeKasir = npgsqlDataReader.GetString(23);
            inOutRevenue.InvBy = npgsqlDataReader.GetString(24);
            inOutRevenue.KasirBy = npgsqlDataReader.GetString(25);
            if (npgsqlDataReader["dtminv"] != DBNull.Value)
            {
                inOutRevenue.DtmInv = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtminv")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            if (npgsqlDataReader["dtmkasir"] != DBNull.Value)
            {
                inOutRevenue.DtmInv = npgsqlDataReader.GetDateTime(npgsqlDataReader.GetOrdinal("dtmkasir")).ToString(GlobalConstant.DATE_YMDHMS_LONG_FORMAT);
            }
            inOutRevenue.TakeDef = new DefinedContainer(npgsqlDataReader.GetString(28)).ToString();
            inOutRevenue.Take20 = new ContainerSpecification(npgsqlDataReader.GetString(29)).ToString();
            inOutRevenue.Take40 = new ContainerSpecification(npgsqlDataReader.GetString(30)).ToString();
            inOutRevenue.Take45 = new ContainerSpecification(npgsqlDataReader.GetString(31)).ToString();

            inOutRevenue.KasirNote = npgsqlDataReader.GetString(32);
            inOutRevenue.PrincipalAdmin = npgsqlDataReader.GetDouble(33);
            inOutRevenue.PrincipalLiftOff = npgsqlDataReader.GetDouble(34);
            inOutRevenue.PrincipalLiftOn = npgsqlDataReader.GetDouble(35);
            inOutRevenue.PrincipalWash = npgsqlDataReader.GetDouble(36);
            inOutRevenue.PrincipalStorage = npgsqlDataReader.GetDouble(37);
            inOutRevenue.SpNote1 = npgsqlDataReader.GetString(38);            
        }

        public InOutRevenue FillInOutRevenueByInOutRevenueId(long inOutRevenueId)
        {
            InOutRevenue inOutRevenue = new InOutRevenue();
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
                                                " WHERE inoutrevenueid=@InOutRevenueId ",
                                        string.Format(DEFAULT_COLUMN, string.Empty),
                                        DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@InOutRevenueId", inOutRevenueId);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                MappingDataReaderToInOutRevenue(npgsqlDataReader, inOutRevenue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return inOutRevenue;
        }
    }
}
