using ITI.Survey.Web.Dll.DAL;
using ITI.Survey.Web.Dll.Model;
using System.Security.Principal;
using System.Threading;

namespace ITI.Survey.Web.Dll.Helper
{
    public class AppPrincipal : IPrincipal
    {
        public AppIdentity AppIdentity = new AppIdentity();

        private AppPrincipal()
        {

        }

        public IIdentity Identity
        {
            get
            {
                return AppIdentity;
            }
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public static bool LoginForService(string userId)
        {
            Thread.CurrentPrincipal = null;
            AppPrincipal appPrincipal = new AppPrincipal();
            AppIdentity appIdentity = appPrincipal.AppIdentity;
            UserLoginDAL userLoginDal = new UserLoginDAL();
            userLoginDal.FillUserLoginByUserId(userId, appIdentity);
            if (appIdentity.IsNew)
            {
                return false;
            }
            Thread.CurrentPrincipal = appPrincipal;
            return true;
        }
    }
}
