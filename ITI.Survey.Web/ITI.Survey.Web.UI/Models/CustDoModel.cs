using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ITI.Survey.Web.UI.Models
{
    public class CustDoModel
    {
        public long CustDoId { get; set; }
        public DateTime DtmDo { get; set; }
        public string DoNumber { get; set; }
        public string CustomerCode { get; set; }
        public string Shipper { get; set; }
        public string VesselVoyage { get; set; }
        public string VesselVoyageName { get; set; }
        public string Destination { get; set; }
        public string DestinationName { get; set; }
        public string Remarks { get; set; }
        public string DefinedCont { get; set; }
        public string Cont20 { get; set; }
        public string Cont40 { get; set; }
        public string Cont45 { get; set; }
        public string Flag { get; set; }
        public string ActOut { get; set; }
        public string KodeKasir { get; set; }
        public string ExBatalRealShipper { get; set; }
        public string Remark2 { get; set; }
        public DateTime DtmStartOut { get; set; }
        public string BusinessUnit { get; set; }
        public int Duration { get; set; }
        public string Region { get; set; }
        public string Commodity { get; set; }
        public long EmklContactPersonId { get; set; }
        public int FreeUseDays { get; set; }
        public string VendorAngkutanOut { get; set; }

        public bool AllowDM
        {
            get
            {
                return Flag.Contains("ALLOWDM");
            }
            set
            {
                
            }
        }

        public CustDoModel()
        {
            CustDoId = 0;
            DtmDo = DateTime.MinValue;
            DoNumber = string.Empty;
            CustomerCode = string.Empty;
            Shipper = string.Empty;
            VesselVoyage = string.Empty;
            VesselVoyageName = string.Empty;
            Destination = string.Empty;
            DestinationName = string.Empty;
            Remarks = string.Empty;
            DefinedCont = string.Empty;
            Cont20 = string.Empty;
            Cont40 = string.Empty;
            Cont45 = string.Empty;
            Flag = string.Empty;
            ActOut = string.Empty;
            KodeKasir = string.Empty;
            ExBatalRealShipper = string.Empty;
            Remark2 = string.Empty;
            DtmStartOut = DateTime.MinValue;
            BusinessUnit = string.Empty;
            Duration = 0;
            Region = string.Empty;
            Commodity = string.Empty;
            EmklContactPersonId = 0;
            FreeUseDays = 0;
            VendorAngkutanOut = string.Empty;
        }
    }
}