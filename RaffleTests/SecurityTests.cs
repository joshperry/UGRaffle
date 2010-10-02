using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RaffleLib.Security;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain;
using RaffleWeb.Infrastructure.Auth;

namespace RaffleTests
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void Can_create_sha256_hash()
        {
            var cleartext = "Corned beef";
            var hash = "bb7247cf4973167d0e4ed9525c8fc15088d30a5d12269dfffd460e49f4674061";

            var result = new Sha256Hasher().Hash(cleartext);

            Assert.AreEqual(hash, result);
        }

        [TestMethod]
        public void Can_get_users_by_role()
        {
            var role = "Admin";
            var email = "josh@6bit.com";
            var members = new Member[] { new Member { Email = email, Roles = new string[] { role } }, new Member() };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable());

            var result = new MemberRoleProvider(repo.Object).GetUsersInRole(role);

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(email, result[0]);
        }

        [TestMethod]
        public void Can_get_if_user_in_role()
        {
            var role = "Admin";
            var email = "josh@6bit.com";
            var members = new Member[] { new Member { Email = email, Roles = new string[] { role } } };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable());

            var result = new MemberRoleProvider(repo.Object).IsUserInRole(email, role);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_get_if_user_not_in_role()
        {
            var role = "Admin";
            var email = "josh@6bit.com";
            var members = new Member[] { new Member { Email = email, Roles = new string[] { role } } };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable());

            var result = new MemberRoleProvider(repo.Object).IsUserInRole(email, "NotRole");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Can_add_user_to_role()
        {
            var role = "Admin";
            var email = "josh@6bit.com";
            var member = new Member { Email = email };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(new Member[] { member }.AsQueryable());
            repo.Setup(x => x.Save(member)).Verifiable();

            new MemberRoleProvider(repo.Object).AddUsersToRoles(new string[] { email }, new string[] { role });

            repo.Verify();
            Assert.AreEqual(role, member.Roles[0]);
        }

        [TestMethod]
        public void Can_remove_user_from_role()
        {
            var role = "Admin";
            var email = "josh@6bit.com";
            var member = new Member { Email = email };
            member.Roles.Add(role);
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(new Member[] { member }.AsQueryable());
            repo.Setup(x => x.Save(member)).Verifiable();

            new MemberRoleProvider(repo.Object).RemoveUsersFromRoles(new string[] { email }, new string[] { role });

            repo.Verify();
            Assert.AreEqual(0, member.Roles.Count);
        }
    }
}
