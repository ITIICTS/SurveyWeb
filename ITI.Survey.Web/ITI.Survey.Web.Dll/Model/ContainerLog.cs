namespace ITI.Survey.Web.Dll.Model
{
    public class ContainerLog
    {
        #region PROPS
        public long ContainerLogId { get; set; }
        public long ContInOutId { get; set; }
        public string Cont { get; set; }
        public string UserId { get; set; }
        public string Operator { get; set; }
        public string EqpId { get; set; }
        public string FlagAct { get; set; }
        public string Location { get; set; }
        public string Shipper { get; set; }
        public string Dtm { get; set; }
        #endregion

        public ContainerLog()
        {
            ContainerLogId = 0;
            ContInOutId = 0;
            Cont = string.Empty;
            UserId = string.Empty;
            Operator = string.Empty;
            EqpId = string.Empty;
            FlagAct = string.Empty;
            Location = string.Empty;
            Shipper = string.Empty;
            Dtm = string.Empty;
        }
    }
}
