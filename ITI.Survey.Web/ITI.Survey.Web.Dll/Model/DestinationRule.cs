
using System;

namespace ITI.Survey.Web.Dll.Model
{
    public class DestinationRule
    {
        #region PROPS
        public long DestinationRuleId { get; set; }
        public string CustomerCode { get; set; }
        public string Cont { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public bool Disabled { get; set; }
        public bool RemindInputDo { get; set; }
        public string Remark { get; set; }
        public string KodeAllow { get; set; }
        public string KodeDeny { get; set; }
        public string KodeAllow2 { get; set; }
        public string KodeAllow3 { get; set; }
        public bool WithRange { get; set; }
        public string Range1 { get; set; }
        public string Range2 { get; set; }

        public long lRange1
        {
            get
            {
                string tmp = Range1.Substring(0, 3) + Range1.Substring(4, 3) + Range1.Substring(8, 1);
                if (tmp.Length > 0)
                    return Convert.ToInt64(tmp);
                else
                    return 0;
            }

        }
        public long lRange2
        {
            get
            {
                string tmp = Range2.Substring(0, 3) + Range2.Substring(4, 3) + Range2.Substring(8, 1);
                if (tmp.Length > 0)
                    return Convert.ToInt64(tmp);
                else
                    return 0;
            }

        }
        #endregion

        public DestinationRule()
        {
            DestinationRuleId = 0;
            CustomerCode = string.Empty;
            Cont = string.Empty;
            Size = string.Empty;
            Type = string.Empty;
            Disabled = false;
            RemindInputDo = false;
            Remark = string.Empty;
            KodeAllow = string.Empty;
            KodeDeny = string.Empty;
            KodeAllow2 = string.Empty;
            KodeAllow3 = string.Empty;
            WithRange = false;
            Range1 = string.Empty;
            Range2 = string.Empty;
        }
    }
}
