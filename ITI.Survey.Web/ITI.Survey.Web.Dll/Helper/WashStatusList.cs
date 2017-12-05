using System;
using System.Collections;

namespace ITI.Survey.Web.Dll.Helper
{
    [Serializable]
    public class WashStatusList : ArrayList
    {
        private static WashStatusList _washStatusList = null;

        public static WashStatusList Value
        {
            get
            {
                if (_washStatusList == null)
                {
                    _washStatusList = new WashStatusList();
                }
                return _washStatusList;
            }
        }

        public WashStatusList() : base()
        {
            Add("W"); 
            Add("D");
            Add("C");
            Add("X");
            Add("N");
        }
    }
}
