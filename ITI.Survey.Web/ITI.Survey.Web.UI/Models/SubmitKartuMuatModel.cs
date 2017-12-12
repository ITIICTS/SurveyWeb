namespace ITI.Survey.Web.UI.Models
{
    public class SubmitKartuMuatModel
    {
        public string ActiveUser { get; set; }
        public long InOutRevenueId { get; set; }
        public long ContInOutId { get; set; }
        public long ContInOutId_Reselect { get; set; } // Utk Reselect
        public long CustDoId { get; set; }
        public long ContCardId { get; set; }
        public string Cont_Seal { get; set; }
        public string Cont_NoMobilOut { get; set; }
        public string EqpId { get; set; }
        public string OPID { get; set; }

        public bool IsCombo { get; set; }
    }
}