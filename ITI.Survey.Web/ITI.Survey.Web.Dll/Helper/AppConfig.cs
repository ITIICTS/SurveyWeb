using ITI.Survey.Web.Dll.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Helper
{
    public class AppConfig
    {
        public const string ConnectionString = "Server={0};Port={1};UserId={2};Password={3};Database={4};";

        public static NpgsqlConnection GetLoginConnection()
        {
            string loginServer = "localhost";
            string port = "5432";
            string userId = "agys";
            string password = "agys123";
            string database = "ictslogin";
            if (ConfigurationManager.AppSettings["LoginServer"] != null)
            {
                loginServer = ConfigurationManager.AppSettings["LoginServer"];
            }
            if (ConfigurationManager.AppSettings["Port"] != null)
            {
                port = ConfigurationManager.AppSettings["Port"];
            }
            if (ConfigurationManager.AppSettings["UserId"] != null)
            {
                userId = ConfigurationManager.AppSettings["UserId"];
            }
            if (ConfigurationManager.AppSettings["LoginDatabase"] != null)
            {
                database = ConfigurationManager.AppSettings["LoginDatabase"];
            }
            string connectionString = string.Format(ConnectionString,
                loginServer,
                port,
                userId,
                password,
                database
            );
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            return npgsqlConnection;
        }

        public static NpgsqlConnection GetConnection()
        {
            string loginServer = "localhost";
            string port = "5432";
            string userId = "agys";
            string password = "agys123";
            string database = "icts";
            if (ConfigurationManager.AppSettings["LoginServer"] != null)
            {
                loginServer = ConfigurationManager.AppSettings["LoginServer"];
            }
            if (ConfigurationManager.AppSettings["Port"] != null)
            {
                port = ConfigurationManager.AppSettings["Port"];
            }
            if (ConfigurationManager.AppSettings["UserId"] != null)
            {
                userId = ConfigurationManager.AppSettings["UserId"];
            }
            if (ConfigurationManager.AppSettings["Database"] != null)
            {
                database = ConfigurationManager.AppSettings["Database"];
            }
            string connectionString = string.Format(ConnectionString,
                loginServer,
                port,
                userId,
                password,
                database
            );
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            return npgsqlConnection;
        }

        public static NpgsqlConnection GetUserConnection()
        {
            AppIdentity appIdentity = Thread.CurrentPrincipal.Identity as AppIdentity;

            string connectionString = string.Format(ConnectionString + "Pooling = True; MaxPoolSize = 60;",
                                            appIdentity.DbHost,
                                            "5432",
                                            appIdentity.UserId,
                                            appIdentity.DbPass,
                                            appIdentity.DbName);
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            return npgsqlConnection;
        }
    }
}
