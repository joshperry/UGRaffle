using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaffleLib.Domain;
using RaffleLib.Domain.Entities;
using Moq;
using RaffleWeb.Models;
using RaffleLib.Domain.Queries;
using RaffleWeb.Controllers;
using System.Web.Mvc;

namespace RaffleTests
{
    [TestClass]
    public class MemberTests
    {
        [TestMethod]
        public void Can_login_member_with_redirect_url()
        {
            var members = new Member[] { new Member { Email = "me@example.com", PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8" } };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable());
            var loginModel = new LoginViewModel { Email = "me@example.com", Password = "password" };

            var result = new MemberController(repo.Object).Login(new GetUserByEmailAndPassword(repo.Object), loginModel, "/blah") as RedirectResult;

            Assert.IsNotNull(result, "Response was not a Redirect");
            Assert.AreEqual("/blah", result.Url);
        }

        [TestMethod]
        public void Can_login_member_without_redirect_url()
        {
            var members = new Member[] { new Member { Email = "me@example.com", PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8" } };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable());
            var loginModel = new LoginViewModel { Email = "me@example.com", Password = "password" };

            var result = new MemberController(repo.Object).Login(new GetUserByEmailAndPassword(repo.Object), loginModel, null) as RedirectResult;

            Assert.IsNotNull(result, "Response was not a Redirect");
            Assert.AreEqual("~/", result.Url);
        }

        [TestMethod]
        public void Can_not_login_with_bad_credentials()
        {
            var members = new Member[] { new Member { Email = "me@example.com", PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8" } };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable());
            var loginModel = new LoginViewModel { Email = "me@example.com", Password = "nottapassword" };

            var result = new MemberController(repo.Object).Login(new GetUserByEmailAndPassword(repo.Object), loginModel, null) as ViewResult;

            Assert.IsNotNull(result, "Response was not a view");
            Assert.AreEqual(string.Empty, result.ViewName);
        }
    }
}
