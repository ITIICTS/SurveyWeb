using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ITI.Survey.Web.UI.Models
{
    public class SearchContainerModel
    {
        [Display(Name = "Container No")]

        public string Cont { get; set; }
        [Display(Name = "Kode Blok")]

        public string KodeBlok { get; set; }
        public string CustomerCode { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Condition { get; set; }
        public DateTime? DtmIn { get; set; }
        public string ActIn { get; set; }
        public string Location { get; set; }
        public string ContAge { get; set; }
        public string Message { get; set; }

        public SearchContainerModel()
        {
            this.Cont = string.Empty;
            this.CustomerCode = string.Empty;
            this.Size = string.Empty;
            this.Type = string.Empty;
            this.Condition = string.Empty;
            this.DtmIn = null;
            this.ActIn = string.Empty;
            this.Location = string.Empty;
            this.ContAge = string.Empty;
            this.Message = string.Empty;

        }
    }
}