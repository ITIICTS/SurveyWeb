namespace ITI.Survey.Web.UI.Models
{
    public class DataTableModel
    {
        public int TotalRow { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string PropertyOrder { get; set; }
        public bool IsDescending { get; set; } // true is descending, false is ascending
    }
}