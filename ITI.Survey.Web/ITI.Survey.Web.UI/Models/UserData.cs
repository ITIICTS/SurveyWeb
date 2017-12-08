using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITI.Survey.Web.UI.Models
{
    public class UserData
    {
        public string HEID { get; set; }
        public string OPID { get; set; }
        public UserData()
        {
            this.HEID = string.Empty;
            this.OPID = string.Empty;
        }
    }
}