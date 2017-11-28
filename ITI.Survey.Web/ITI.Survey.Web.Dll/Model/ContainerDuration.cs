using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Model
{
    public class ContainerDuration
    {
        public int No { get; set; }
        public long ContInOutId { get; set; }
        public string Cont { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string CustomerCode { get; set; }
        public string Condition { get; set; }
        public string DtmIn { get; set; }
        public string ActIn { get; set; }
        public string RfEngineCond { get; set; }
        public string Remarks { get; set; }
        public int Duration { get; set; }
        public string EorNo { get; set; }
        public string DtmEor { get; set; }
        public string DtmApproved { get; set; }
        public string DtmCompleted { get; set; }
        public string RepairStatus { get; set; }
        public string SpecialMessage { get; set; }
        public string SpecialMessage2 { get; set; }

        public ContainerDuration()
        {
            No = 0;
            ContInOutId = 0;
            Cont = string.Empty;
            Size= string.Empty;
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
