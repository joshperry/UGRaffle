using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Moq;
using RaffleLib.Domain;
using RaffleLib.Domain.Entities;
using RaffleWeb.Infrastructure;

namespace RaffleWebTests
{
//    [TestClass]
    public class MemberRoleProviderTests
    {
//        [TestMethod]
        public void Can_get_users_by_role()
        {
            var role = "Admin";
            var email = "josh@6bit.com";
            var members = new Member[] { new Member { Email = email, Roles = new string[] { role } }, new Member() };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable());

            var result = new MemberRoleProvider(repo.Object).GetUsersInRole(role);

            //assert.areequal(1, result.length);
            //assert.areequal(email, result[0]);
        }

 //       [TestMethod]
        public void Can_get_if_user_in_role()
        {
            var role = "Admin";
            var email = "josh@6bit.com";
            var members = new Member[] { new Member { Email = email, Roles = new string[] { role } } };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable());

            var result = new MemberRoleProvider(repo.Object).IsUserInRole(email, role);

            //Assert.IsTrue(result);
        }

//        [TestMethod]
        public void Can_get_if_user_not_in_role()
        {
            var role = "Admin";
            var email = "josh@6bit.com";
            var members = new Member[] { new Member { Email = email, Roles = new string[] { role } } };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable());

            var result = new MemberRoleProvider(repo.Object).IsUserInRole(email, "NotRole");

            //Assert.IsFalse(result);
        }

//        [TestMethod]
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
            //Assert.AreEqual(role, member.Roles[0]);
        }

//        [TestMethod]
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
            //Assert.AreEqual(0, member.Roles.Count);
        }
    }
}
