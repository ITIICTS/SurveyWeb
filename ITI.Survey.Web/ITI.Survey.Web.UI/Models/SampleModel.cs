using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Models
{
    public class SampleModel
    {
        public Guid SampleID { get; set; }
        [Display(Name = "Sample Description")]
        public String SampleDescription { get; set; }
        [Display(Name = "Sample Drop List")]
        public String SampleDropList { get; set; }
        [Display(Name = "Sample Number")]
        public Int32 SampleNumber { get; set; }
        [Display(Name = "Sample Currency")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0}")]
        public Double SampleCurrency { get; set; }
        [Display(Name = "Sample Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? SampleDate { get; set; }

        public string SampleLookup { get; set; }

        public SampleModel()
        {
            this.SampleID = Guid.NewGuid();
            this.SampleDescription = string.Empty;
            this.SampleLookup = string.Empty;
            this.SampleDate = DateTime.Now;
            this.SampleList = new List<SampleModel>();
            this.SampleListJson = string.Empty;
        }

        public List<SampleModel> SampleList { get; set; }
        public string SampleListJson { get; set; }

        public void SampleValidate(ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(this.SampleDescription))
            {
                modelState.AddModelError("SampleDescription", "Sample Description is required.");
            }
        }
    }
}