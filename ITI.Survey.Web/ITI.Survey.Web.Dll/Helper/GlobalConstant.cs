using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Helper
{
    public static class GlobalConstant
    {
        public const string DATE_YMDHMS_LONG_FORMAT = "yyyy-MM-dd HH::mm:ss";

        private static List<string> _containerSizeList = new List<string>();
        public static List<string> ContainerSizeList
        {
            get
            {
                if(_containerSizeList.Count.Equals(0))
                {
                    _containerSizeList.Add("20");
                    _containerSizeList.Add("40");
                    _containerSizeList.Add("45");
                }
                return _containerSizeList;
            }
        }    
    }
}
