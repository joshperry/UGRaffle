using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaffleLib.Domain;
using RaffleLib.Domain.Entities;
using Moq;
using RaffleLib.Domain.Queries;
using RaffleLib.Security;

namespace RaffleLibTests
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
            const string email = "josh@6bit.com";
            const string password = "password";
            const string hash = "333";
            var members = new Member[] { new Member { Email = email, PasswordHash = hash } };
            var repo = new Mock<IEntityRepository<Member>>();
            repo.SetupGet(x => x.Query).Returns(members.AsQueryable);
            var hasher = new Mock<IHasher>();
            hasher.Setup(x => x.Hash(password)).Returns(hash).Verifiable();

            var result = new GetMemberByCredentials(repo.Object, hasher.Object).Result(email, password);

            hasher.Verify();
            Assert.AreEqual(members[0], result);
        }
    }
}
