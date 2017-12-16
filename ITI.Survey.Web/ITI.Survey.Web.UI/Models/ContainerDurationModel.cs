using System.ComponentModel.DataAnnotations;

namespace ITI.Survey.Web.UI.Models
{
    public class ContainerDurationModel
    {
        [Display(Name = "No")]
        public int No { get; set; }
        public long ContInOutId { get; set; }

        [Display(Name = "Cont")]
        public string Cont { get; set; }

        [Display(Name = "Size")]
        public string Size { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Customer")]
        public string CustomerCode { get; set; }

        [Display(Name = "Condition")]
        public string Condition { get; set; }

        [Display(Name = "Date In")]
        public string DtmIn { get; set; }
        public string ActIn { get; set; }
        public string RfEngineCond { get; set; }

        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        public string EorNo { get; set; }
        public string DtmEor { get; set; }
        public string DtmApproved { get; set; }
        public string DtmCompleted { get; set; }
        public string RepairStatus { get; set; }

        [Display(Name = "SPECIAL MESSAGE")]
        public string SpecialMessage { get; set; }
        public string SpecialMessage2 { get; set; }

        public ContainerDurationModel()
        {
            No = 0;
            ContInOutId = 0;
            Cont = string.Empty;
            Size = string.Empty;
            Type = string.Empty;
            CustomerCode = string.Empty;
            Condition = string.Empty;
            DtmIn = string.Empty;
            ActIn = string.Empty;
            RfEngineCond = string.Empty;
            Remarks = string.Empty;
            Duration = 0;
            EorNo = string.Empty;
            DtmEor = string.Empty;
            DtmApproved = string.Empty;
            DtmCompleted = string.Empty;
            RepairStatus = string.Empty;
            SpecialMessage = string.Empty;
            SpecialMessage2 = string.Empty;
        }
    }
}