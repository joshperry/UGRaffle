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
using RaffleWeb.Infrastructure.Auth;

namespace RaffleTests
{
    [TestClass]
    public class MemberTests
    {
        [TestMethod]
        public void Can_login_member_with_redirect_url()
        {
            var login = new LoginViewModel { Email = "me@example.com", Password = "password" };
            var query = new Mock<IGetUserByEmailAndPassword>();
            query.Setup(x => x.Result(login.Email, login.Password)).Returns(new Member());
            var auth = new Mock<IFormsAuthentication>();
            auth.Setup(x => x.SetAuthCookie(login.Email, false)).Verifiable();

            var result = new MemberController(auth.Object).Login(query.Object, login, "/blah") as RedirectResult;

            auth.Verify();
            Assert.IsNotNull(result, "Response was not a Redirect");
            Assert.AreEqual("/blah", result.Url);
        }

        [TestMethod]
        public void Can_login_member_without_redirect_url()
        {
            var login = new LoginViewModel { Email = "me@example.com", Password = "password" };
            var query = new Mock<IGetUserByEmailAndPassword>();
            query.Setup(x => x.Result(login.Email, login.Password)).Returns(new Member());
            var auth = new Mock<IFormsAuthentication>();
            auth.Setup(x => x.SetAuthCookie(login.Email, false)).Verifiable();

            var result = new MemberController(auth.Object).Login(query.Object, login, null) as RedirectResult;

            auth.Verify();
            Assert.IsNotNull(result, "Response was not a Redirect");
            Assert.AreEqual("~/", result.Url);
        }

        [TestMethod]
        public void Can_not_login_with_bad_credentials()
        {
            var login = new LoginViewModel { Email = "me@example.com", Password = "password" };
            var query = new Mock<IGetUserByEmailAndPassword>();
            query.Setup(x => x.Result(login.Email, login.Password)).Returns((Member)null);
            var auth = new Mock<IFormsAuthentication>();

            var result = new MemberController(auth.Object).Login(query.Object, login, null) as ViewResult;

            auth.Verify(x => x.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()),
                Times.Never(), "Auth cookie shouldn't be set with bad creds");
            Assert.IsNotNull(result, "Response was not a view");
            Assert.AreEqual(string.Empty, result.ViewName);
        }
    }
}
