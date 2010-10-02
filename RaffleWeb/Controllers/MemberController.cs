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
        private IEntityRepository<Member> _repo;
        private IFormsAuthentication _auth;
        public MemberController(IEntityRepository<Member> repo, IFormsAuthentication auth)
        {
            _repo = repo;
            _auth = auth;
        }

        public ViewResult Login()
        {
            return View(new LoginViewModel());
        }

        public ViewResult Create()
        {
            return View(new Member());
        }

        [HttpPost]
        public ActionResult Create(IGetMemberByEmail query, Member member)
        {
            if (ModelState.IsValid)
            {
                var dbmember = query.Result(member.Email);
                if (dbmember == null)
                {
                    _repo.Save(member);
                    return Redirect("~/");
                }
                else
                {
                    ModelState.AddModelError("", "A member with this email already exists");
                }
            }
            
            return View(member);
        }

        [HttpPost]
        public ActionResult Login(IGetMemberByCredentials query, LoginViewModel model, string returnUrl)
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

        public RedirectResult Logout()
        {
            _auth.SignOut();
            return Redirect("~/");
        }
    }
}