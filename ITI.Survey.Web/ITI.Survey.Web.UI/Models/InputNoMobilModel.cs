using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Models
{
    public class InputNoMobilModel
    {
        public string ActiveUser { get; set; }

        public string ResultMessage { get; set; }

        [Display(Name = "Scan Card ID")]
        public Int64 ContCardID { get; set; }

        [Display(Name = "Nomor Mobil")]
        [StringLength(10)]
        public string NoMobil { get; set; }

        [Display(Name = "Flag Act")]

        public string FlagAct { get; set; }


        [Display(Name = "Angkutan")]
        [StringLength(40)]
        public string Angkutan { get; set; }


        [Display(Name = "Is Combo")]
        public bool IsCombo { get; set; }

        public InputNoMobilModel()
        {
            this.ContCardID = 0;
            this.NoMobil = string.Empty;
            this.Angkutan = string.Empty;
            this.FlagAct = "OUT";
            this.IsCombo = false;
            this.ResultMessage = string.Empty;
        }

        public void Validate(ModelStateDictionary modelState)
        {
            if (this.ContCardID <= 0)
            {
                modelState.AddModelError("ContCardID", "Scan Card ID is Required.");

            }
            if (string.IsNullOrEmpty(this.NoMobil))
            {
                modelState.AddModelError("NoMobil", "Nomor Mobil is Required.");

            }
            if (string.IsNullOrEmpty(this.Angkutan))
            {
                modelState.AddModelError("Angkutan", "Angkutan is Required.");

            }
        }

    }
}