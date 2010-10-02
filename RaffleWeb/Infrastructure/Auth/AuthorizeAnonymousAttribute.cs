using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaffleWeb.Infrastructure.Auth
{
    /// <summary>
    /// Use this on actions to override an authorize attribute on the controller and allow access to anonymous users
    /// </summary>
    public class AuthorizeAnonymousAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            return;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            return;
        }

        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return HttpValidationStatus.Valid;
        }
    }
}