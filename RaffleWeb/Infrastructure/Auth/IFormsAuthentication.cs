using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaffleWeb.Infrastructure.Auth
{
    public interface IFormsAuthentication
    {
        void SetAuthCookie(string username, bool persistentCookie);
        void SignOut();
    }
}
