using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Models
{
    public class BlokSystemModel
    {
        [Display(Name = "Lokasi")]

        public string ResultMessage { get; set; }

        [Display(Name = "Nomor Container")]

        public string ContNo { get; set; }

        public List<string> ListSide { get; set; }

        public string CustomerCode { get; set; }

        [Display(Name = "Blok")]
        public string Blok { get; set; }
        [Display(Name = "Bay")]
        public string Bay { get; set; }
        [Display(Name = "Row")]
        public string Row { get; set; }
        [Display(Name = "Tier")]
        public string Tier { get; set; }


        public string KodeBlok { get; set; }

        public long ContInOutId { get; set; }


        public string FlagAct { get; set; }


        public BlokSystemModel()
        {
            this.ContNo = string.Empty;
            this.CustomerCode = string.Empty;
            this.Blok = string.Empty;
            this.Bay = string.Empty;
            this.Row = string.Empty;
            this.Tier = string.Empty;
            this.KodeBlok = string.Empty;
            this.ContInOutId = 0;
            this.FlagAct = "MOVE";
            this.ListSide = new List<string>();
            this.ListSide.Add("KANAN");
            this.ListSide.Add("KIRI");

        }
        public void BlokSystemValidate(ModelStateDictionary modelState)
        {
            if (this.ContInOutId <= 0)
            {
                modelState.AddModelError("ContInOutId", "Container In Out ID is Required.");

            }
            if (string.IsNullOrEmpty(this.Blok))
            {
                modelState.AddModelError("Blok", "Blok is Required.");

            }
            if (string.IsNullOrEmpty(this.Bay))
            {
                modelState.AddModelError("Bay", "Bay is Required.");

            }
            if (string.IsNullOrEmpty(this.Row))
            {
                modelState.AddModelError("Row", "Row is Required.");

            }
            if (string.IsNullOrEmpty(this.Tier))
            {
                modelState.AddModelError("Tier", "Tier is Required.");

            }
        }

        public string Cont { get; set; }

        public string Cont2 { get; set; }

        public string Location { get; set; }

        public string Shipper { get; set; }
    }
}