using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Models
{
    public class InputNoMobilModel
    {

        public string ResultMessage { get; set; }

        [Display(Name = "Scan Card ID")]
        public Int64 ContCardID { get; set; }

        [Display(Name = "Nomor Mobil")]
        public string NoMobil { get; set; }

        [Display(Name = "Flag Act")]

        public string FlagAct { get; set; }


        [Display(Name = "Angkutan")]
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
        public void InputNoMobilSampleValidate(ModelStateDictionary modelState)
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