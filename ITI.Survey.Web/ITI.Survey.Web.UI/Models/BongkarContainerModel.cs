using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITI.Survey.Web.UI.Models
{
    public class BongkarContainerModel
    {
        public BongkarContainerModel()
        {
            
        }

        public long ContCardID { get; set; }
        public string ActiveUser { get; set; }
        public long ContInOutId { get; set; }
        public string Location { get; set; }
        public string FlagAct { get; set; }
        public string EqpId { get; set; }
        public string Shipper { get; set; }
        public string OPID { get; set; }
        public string Cont { get; set; }
        public string Cont2 { get; set; }

        [StringLength(1)]
        public string Blok { get; set; }
        [Required]
        [StringLength(2)]
        public string Bay { get; set; }
        [Required]
        [StringLength(2)]
        public string Row { get; set; }
        [Required]
        [StringLength(1)]
        public string Tier { get; set; }
        public string Side { get; set; }
    }
}