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
    public class UserLoginDAL
    {
        public const string DEFAULT_COLUMN = "userid, username, password, disabled, randompass, randomtype, dbpass, dbname, dbhost, cashiercode";
        public const string DEFAULT_TABLE = "userlogin";

        private void MappingReaderToUserLogin(NpgsqlDataReader npgsqlDataReader, UserLogin userLogin)
        {
            userLogin.UserId = Convert.ToString(npgsqlDataReader["userid"]);
            userLogin.UserName = Convert.ToString(npgsqlDataReader["username"]);
            userLogin.Password = Convert.ToString(npgsqlDataReader["password"]);
            userLogin.Disabled = Convert.ToInt32(npgsqlDataReader["disabled"]);
            userLogin.RandomPass = Convert.ToInt32(npgsqlDataReader["randompass"]);
            userLogin.RandomType = Convert.ToString(npgsqlDataReader["randomtype"]);
            userLogin.DbHost = Convert.ToString(npgsqlDataReader["dbhost"]);
            userLogin.DbName = Convert.ToString(npgsqlDataReader["dbname"]);
            userLogin.DbPass = Convert.ToString(npgsqlDataReader["dbpass"]);
            userLogin.CashierCode = Convert.ToString(npgsqlDataReader["cashiercode"]);

            userLogin.IsNew = false;
        }

        public UserLogin FillUserLoginByUserId(string userId)
        {
            UserLogin userLogin = new UserLogin();
            FillUserLoginByUserId(userId, userLogin);
            return userLogin;
        }

        public void FillUserLoginByUserId (string userId, UserLogin userLogin)
        {
            try
            {
                using (NpgsqlConnection npgsqlConnection = AppConfig.GetLoginConnection())
                {
                    npgsqlConnection.Open();
                    string query = string.Format("SELECT {0} FROM {1} WHERE userid=@Userid", DEFAULT_COLUMN, DEFAULT_TABLE);
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection))
                    {
                        npgsqlCommand.Parameters.AddWithValue("@UserId", userId);
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (npgsqlDataReader.Read())
                            {
                                MappingReaderToUserLogin(npgsqlDataReader, userLogin);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
