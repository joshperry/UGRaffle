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
using RaffleLib.Security;

namespace RaffleTests
{
    [TestClass]
    public class MemberTests
    {
        [TestMethod]
        public void Can_query_members_in_role()
        {
            var roles = new string[] { "Admin" };
            var members = new Member[] { new Member { Roles = roles} };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable);

            var result = new GetMembersInRole(repo.Object).Result(roles[0]);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(members[0], result.FirstOrDefault());
        }

        [TestMethod]
        public void Can_query_members_by_credentials()
        {
            string email = "josh@6bit.com";
            string password = "password";
            string hash = "333";
            var members = new Member[] { new Member { Email = email, PasswordHash = hash } };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable);
            var hasher = new Mock<IHasher>();
            hasher.Setup(x => x.Hash(password)).Returns(hash).Verifiable();

            var result = new GetMemberByCredentials(repo.Object, hasher.Object).Result(email, password);

            hasher.Verify();
            Assert.AreEqual(members[0], result);
        }

        [TestMethod]
        public void Can_login_member_with_redirect_url()
        {
            var login = new LoginViewModel { Email = "me@example.com", Password = "password" };
            var query = new Mock<IGetMemberByCredentials>();
            query.Setup(x => x.Result(login.Email, login.Password)).Returns(new Member());
            var auth = new Mock<IFormsAuthentication>();
            auth.Setup(x => x.SetAuthCookie(login.Email, false)).Verifiable();

            var result = new MemberController(null, auth.Object).Login(query.Object, login, "/blah") as RedirectResult;

            auth.Verify();
            Assert.IsNotNull(result, "Response was not a Redirect");
            Assert.AreEqual("/blah", result.Url);
        }

        [TestMethod]
        public void Can_login_member_without_redirect_url()
        {
            var login = new LoginViewModel { Email = "me@example.com", Password = "password" };
            var query = new Mock<IGetMemberByCredentials>();
            query.Setup(x => x.Result(login.Email, login.Password)).Returns(new Member());
            var auth = new Mock<IFormsAuthentication>();
            auth.Setup(x => x.SetAuthCookie(login.Email, false)).Verifiable();

            var result = new MemberController(null, auth.Object).Login(query.Object, login, null) as RedirectResult;

            auth.Verify();
            Assert.IsNotNull(result, "Response was not a Redirect");
            Assert.AreEqual("~/", result.Url);
        }

        [TestMethod]
        public void Can_not_login_with_bad_credentials()
        {
            var login = new LoginViewModel { Email = "me@example.com", Password = "password" };
            var query = new Mock<IGetMemberByCredentials>();
            query.Setup(x => x.Result(login.Email, login.Password)).Returns((Member)null);
            var auth = new Mock<IFormsAuthentication>();

            var result = new MemberController(null, auth.Object).Login(query.Object, login, null) as ViewResult;

            auth.Verify(x => x.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()),
                Times.Never(), "Auth cookie shouldn't be set with bad creds");
            Assert.IsNotNull(result, "Response was not a view");
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void Can_create_member()
        {
            var member = new Member { Email = "josh@6bit.com" };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.Setup(x => x.Save(member));
            var query = new Mock<IGetMemberByEmail>();
            query.Setup(x => x.Result(member.Email)).Returns((Member)null);

            new MemberController(repo.Object, null).Create(query.Object, member);

            query.Verify(x => x.Result(member.Email), "Didn't check for existing member");
            repo.Verify(x => x.Save(member), "Didn't save member to repo");
        }

        [TestMethod]
        public void Can_not_create_member_with_existing_email()
        {
            var member = new Member { Email = "josh@6bit.com" };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.Setup(x => x.Save(member));
            var query = new Mock<IGetMemberByEmail>();
            query.Setup(x => x.Result(member.Email)).Returns(member);

            new MemberController(repo.Object, null).Create(query.Object, member);

            repo.Verify(x => x.Save(member), Times.Never(), "Shouldn't save member to repo");
        }
    }
}
