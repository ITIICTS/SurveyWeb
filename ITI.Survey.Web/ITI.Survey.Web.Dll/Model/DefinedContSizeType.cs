using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Survey.Web.Dll.Model
{
    public class DefinedContSizeType
    {
        InOutRevenue _parent = null;

        public long DefinedContSizeTypeId { get; set; }
        public long InOutRevenueId { get; set; }
        public string Cont { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public InOutRevenue Parent
        {
            get
            {
                return _parent;
            }
        }


        public DefinedContSizeType(InOutRevenue parent)
        {
            _parent = parent;
            DefinedContSizeTypeId = 0;
            InOutRevenueId = 0;
            Cont = string.Empty;
            Size = string.Empty;
            Type = string.Empty;
        }
    }
}
