using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Model
{
    public class UserLogin
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Disabled { get; set; }
        public int RandomPass { get; set; }
        public string RandomType { get; set; }
        public string DbPass { get; set; }
        public string DbName { get; set; }
        public string DbHost { get; set; }
        public string CashierCode { get; set; }
        public bool IsNew { get; set; }

        public UserLogin()
        {
            UserId = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            Disabled = 0;
            RandomPass = 0;
            RandomType = string.Empty;
            DbPass = string.Empty;
            DbName = string.Empty;
            DbHost = string.Empty;
            CashierCode = string.Empty;
            IsNew = true;
        }
    }
}
