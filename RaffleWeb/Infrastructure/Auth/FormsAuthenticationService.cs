using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaffleWeb.Infrastructure.Auth
{
    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SetAuthCookie(string username, bool persistentCookie)
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(username, persistentCookie);
        }

        public void SignOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
        }
    }
}