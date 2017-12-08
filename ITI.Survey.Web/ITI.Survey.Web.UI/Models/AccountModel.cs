using System.ComponentModel.DataAnnotations;

namespace ITI.Survey.Web.UI.Models
{
    public class AccountModel
    {
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        public string HE { get; set; }

        [Required]
        public string OPID { get; set; }

        public bool RememberMe { get; set; }

        public LoginModel()
        {
            this.UserId = string.Empty;
            this.Password = string.Empty;
            this.RememberMe = true;
        }
    }
}