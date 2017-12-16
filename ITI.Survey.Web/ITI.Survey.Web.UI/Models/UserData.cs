namespace ITI.Survey.Web.UI.Models
{
    public class UserData
    {
        public string HEID { get; set; }
        public string OPID { get; set; }

        public UserData()
        {
            HEID = string.Empty;
            OPID = string.Empty;
        }
    }
}