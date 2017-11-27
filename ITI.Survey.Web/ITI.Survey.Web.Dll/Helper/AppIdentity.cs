using ITI.Survey.Web.Dll.Model;
using System.Security.Principal;

namespace ITI.Survey.Web.Dll.Helper
{
    public class AppIdentity : UserLogin, IIdentity
    {
        public string AuthenticationType
        {
            get
            {
                return "Custom";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return UserId.Length != 0;
            }
        }

        public string Name
        {
            get
            {
                return UserId;
            }
        }
    }
}
