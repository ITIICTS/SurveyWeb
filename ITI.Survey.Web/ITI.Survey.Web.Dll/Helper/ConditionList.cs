using System;
using System.Collections;

namespace ITI.Survey.Web.Dll.Helper
{
    [Serializable()]
    public class ConditionList : ArrayList
    {
        private static ConditionList _conditionList = null;

        public static ConditionList Value
        {
            get
            {
                if (_conditionList == null)
                {
                    _conditionList = new ConditionList();
                }
                return _conditionList;
            }
        }

        public ConditionList() : base()
        {
            Add("AV");
            Add("DM");
            Add("WS");
        }
    }
}
