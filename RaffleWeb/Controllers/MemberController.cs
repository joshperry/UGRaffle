using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain;
using RaffleLib.Domain.Queries;
using RaffleWeb.Models;
using System.Web.Security;
using RaffleWeb.Infrastructure.Auth;

namespace RaffleWeb.Controllers
{
    public class MemberController : Controller
    {
        private IFormsAuthentication _auth;
        public MemberController(IFormsAuthentication auth)
        {
            _auth = auth;
        }

        public ViewResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(IGetUserByCredentials query, LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var member = query.Result(model.Email, model.Password);
                if (member == null)
                    ModelState.AddModelError("", "Unknown username or password");
            }

            if (ModelState.IsValid)
            {
                _auth.SetAuthCookie(model.Email, false);
                return Redirect(returnUrl ?? "~/");
            }
            else
                return View(model);
        }
    }
}
