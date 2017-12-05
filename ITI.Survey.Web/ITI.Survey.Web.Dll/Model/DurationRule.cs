using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Model
{
    public class DurationRule
    {
        #region PROPS
        public long DurationRuleId { get; set; }
        public string CustomerCode { get; set; }
        public string Cont { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public DateTime DtmStart { get; set; }
        public DateTime DtmEnd { get; set; }
        public int MinDuration { get; set; }
        public bool Disabled { get; set; }
        public string Remark { get; set; }
        #endregion

        public DurationRule()
        {
            DurationRuleId = 0;
            CustomerCode = string.Empty;
            Cont = string.Empty;
            Size = string.Empty;
            Type = string.Empty;
            DtmStart = DateTime.MinValue;
            DtmEnd = DateTime.MinValue;
            MinDuration = 0;
            Disabled = false;
            Remark = string.Empty;

        }
    }
}
