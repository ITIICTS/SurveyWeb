using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Models
{
    public class BlokSystemModel
    {
        public string ActiveUser { get; set; }
        public string Size { get; set; }

        [Display(Name = "Lokasi")]
        public string ResultMessage { get; set; }

        [Display(Name = "Nomor Container")]
        public string ContNo { get; set; }
        
        public string CustomerCode { get; set; }
      
        [Display(Name = "Blok")]
        [StringLength(1, ErrorMessage = "Blok cannot be longer than 4 characters.")]
        public string Blok { get; set; }

        [StringLength(4, ErrorMessage = "Bay cannot be longer than 4 characters.")]
        [Display(Name = "Bay")]
        public string Bay { get; set; }

        [Display(Name = "Row")]
        [StringLength(4, ErrorMessage = "Row cannot be longer than 4 characters.")]
        public string Row { get; set; }

        [Display(Name = "Tier")]
        [StringLength(4, ErrorMessage = "Tier cannot be longer than 4 characters.")]
        public string Tier { get; set; }

        public string KodeBlok { get; set; }
        public string EqpId { get; set; }
        public string OPID { get; set; }

        public long ContInOutId { get; set; }
        public string FlagAct { get; set; }
        public string SideChoose { get; set; }
        public string Cont { get; set; }
        public string Cont2 { get; set; }
        public string Location { get; set; }
        public string Shipper { get; set; }

        public BlokSystemModel()
        {
            ContNo = string.Empty;
            CustomerCode = string.Empty;
            Blok = string.Empty;
            Bay = string.Empty;
            Row = string.Empty;
            Tier = string.Empty;
            KodeBlok = string.Empty;
            ContInOutId = 0;
            FlagAct = "MOVE";
            SideChoose = string.Empty;
        }

        public void BlokSystemValidate(ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(Blok))
            {
                modelState.AddModelError("Blok", "Blok is Required.");
            }
            if (string.IsNullOrEmpty(Bay))
            {
                modelState.AddModelError("Bay", "Bay is Required.");
            }
            if (string.IsNullOrEmpty(Row))
            {
                modelState.AddModelError("Row", "Row is Required.");
            }
            if (string.IsNullOrEmpty(Tier))
            {
                modelState.AddModelError("Tier", "Tier is Required.");
            }
        }

    }
}