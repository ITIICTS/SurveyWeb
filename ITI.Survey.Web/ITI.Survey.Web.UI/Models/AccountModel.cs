﻿using AGY.Solution.Helper.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Models
{
    public class AccountModel
    {
    }

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
            this.UserId = string.Empty;
            this.Password = string.Empty;
            this.RememberMe = true;
        }

        public void Validate(ModelStateDictionary modelState)
        {
            string machineName = Environment.MachineName;
            string ipaddress = string.Empty;
            foreach (var item in Dns.GetHostAddresses(Environment.MachineName))
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipaddress = item.MapToIPv4().ToString();
                }
            }

            try
            {
                string keys = ConfigurationManager.AppSettings["Keys"];
                string[] arrayKey = keys.Split('|');

                if (machineName != Decryptor.DecryptString(arrayKey[0])
                    || ipaddress != Decryptor.DecryptString(arrayKey[1])
                    || DateTime.Now > Convert.ToDateTime(Decryptor.DecryptString(arrayKey[2])))
                {
                    modelState.AddModelError("global", "Failed secure application process.");
                }
            }
            catch (Exception ex)
            {
                modelState.AddModelError("global", ex.Message);
            }
        }
    }
}