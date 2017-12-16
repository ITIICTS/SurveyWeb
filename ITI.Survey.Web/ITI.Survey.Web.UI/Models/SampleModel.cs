using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ITI.Survey.Web.UI.Models
{
    public class SampleModel
    {
        public Guid SampleID { get; set; }
        [Display(Name = "Sample Description")]
        public string SampleDescription { get; set; }
        [Display(Name = "Sample Drop List")]
        public string SampleDropList { get; set; }
        [Display(Name = "Sample Number")]
        public int SampleNumber { get; set; }
        [Display(Name = "Sample Currency")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0}")]
        public double SampleCurrency { get; set; }
        [Display(Name = "Sample Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? SampleDate { get; set; }

        public string SampleLookup { get; set; }

        public SampleModel()
        {
            SampleID = Guid.NewGuid();
            SampleDescription = string.Empty;
            SampleLookup = string.Empty;
            SampleDate = DateTime.Now;
            SampleList = new List<SampleModel>();
            SampleListJson = string.Empty;
        }

        public List<SampleModel> SampleList { get; set; }
        public string SampleListJson { get; set; }

        public void SampleValidate(ModelStateDictionary modelState)
        {
            if (string.IsNullOrEmpty(SampleDescription))
            {
                modelState.AddModelError("SampleDescription", "Sample Description is required.");
            }
        }
    }
}