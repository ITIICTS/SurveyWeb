using AGY.Solution.Helper.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Diagnostics;

namespace ITI.Survey.Web.UI.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        public string HE { get; set; }

        [Required]
        public string OPID { get; set; }

        public bool RememberMe { get; set; }

        public LoginModel()
        {
            UserId = string.Empty;
            Password = string.Empty;
            RememberMe = true;
        }

        
    }
}