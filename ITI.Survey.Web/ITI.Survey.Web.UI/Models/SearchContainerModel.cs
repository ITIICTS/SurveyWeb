using System.ComponentModel.DataAnnotations;

namespace ITI.Survey.Web.UI.Models
{
    public class SearchContainerModel
    {
        [Display(Name = "Cont No")]
        public string Cont { get; set; }

        [Display(Name = "Kode Blok")]
        public string Location { get; set; }

        [Display(Name = "Customer")]
        public string CustomerCode { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }

        [Display(Name ="Cond")]
        public string Condition { get; set; }

        [Display(Name = "Date In")]
        public string DtmIn { get; set; }

        [Display(Name = "Act In")]
        public string ActIn { get; set; }

        [Display(Name = "Age")]
        public string ContAge { get; set; }
        public string Message { get; set; }

        public SearchContainerModel()
        {
            Cont = string.Empty;
            CustomerCode = string.Empty;
            Size = string.Empty;
            Type = string.Empty;
            Condition = string.Empty;
            DtmIn = string.Empty;
            ActIn = string.Empty;
            Location = string.Empty;
            ContAge = string.Empty;
            Message = string.Empty;

        }
    }
}