using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Model
{
    public class BlackList
    {
        public long BlackListId { get; set; }
        public DateTime DtmCreate { get; set; }
        public string ContainerNumber { get; set; }
        public string LastReleaseInfo { get; set; }
        public string TextFlag { get; set; }
        public bool Disabled { get; set; }
        public DateTime DisabledUntil { get; set; }
        public string Message { get; set; }

        public bool BlockOut
        {
            get
            {
                return TextFlag.Contains("BLOCKOUT");
            }
        }

        public bool BlockByMnr
        {
            get
            {
                return TextFlag.Contains("BLOCKBYMNR");
            }
        }



        public BlackList()
        {
            BlackListId = 0;
            DtmCreate = DateTime.MinValue;
            ContainerNumber = string.Empty;
            LastReleaseInfo = string.Empty;
            TextFlag = string.Empty;
            Disabled = false;
            DisabledUntil = DateTime.MinValue;            
            Message = string.Empty;
        }
    }
}
