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
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace RaffleLibTests
{
    [Subject("Query Members")]
    public class when_querying_for_member_with_valid_credentials : with_member_credential_mocks
    {
        private static Member result;

        Because of = () => 
            result = new GetMemberByCredentials(memberRepo, hasherMock.Object).Result(member.Email, password);

        It should_hash_the_password = () => hasherMock.Verify(x => x.Hash(password));

        It should_get_proper_member = () => result.ShouldBeTheSameAs(member);
    }

    [Subject("Query Members")]
    public class when_querying_for_member_with_invalid_credentials : with_member_credential_mocks
    {
        private static Member result;

        Because of = () => 
            result = new GetMemberByCredentials(memberRepo, hasherMock.Object).Result(member.Email, "bad password");

        It should_get_no_member = () => result.ShouldBeNull();
    }

    [Subject("Query Members")]
    public class when_querying_for_members_in_nonempty_role : with_member_role_mocks
    {
        private static IEnumerable<Member> result;

        Because of = () => result = new GetMembersInRole(memberRepo).Result(role);

        It should_get_members_in_role = () => result.ShouldContainOnly(memberRepo.Query);
    }

    [Subject("Query Members")]
    public class when_querying_for_members_in_empty_role : with_member_role_mocks
    {
        private static IEnumerable<Member> result;

        Because of = () => result = new GetMembersInRole(memberRepo).Result("FakeRole");

        It should_get_no_members = () => result.ShouldBeEmpty();
    }

    public abstract class with_member_credential_mocks
    {
        protected const string password  = "password";
        protected static readonly Member member = new Member {Email = "josh@6bit.com", PasswordHash = "333"};
        protected static readonly Mock<IHasher> hasherMock = new Mock<IHasher>();
        protected static IEntityRepository<Member> memberRepo;

        Establish context = () =>
                                {
                                    var mockRepo = new Mock<IEntityRepository<Member>>();
                                    mockRepo.SetupGet(x => x.Query).Returns(new[] { member }.AsQueryable());
                                    memberRepo = mockRepo.Object;

                                    hasherMock.Setup(x => x.Hash(Moq.It.IsAny<string>())).Returns("8585");
                                    hasherMock.Setup(x => x.Hash(password)).Returns(member.PasswordHash);
                                };
    }

    public abstract class with_member_role_mocks
    {
        protected const string role = "Admin";
        protected static readonly Member member = new Member {Roles = new[]{role}};
        protected static IEntityRepository<Member> memberRepo;

        Establish context = () =>
                                {
                                    var mockRepo = new Mock<IEntityRepository<Member>>();
                                    mockRepo.SetupGet(x => x.Query).Returns(new[] {member}.AsQueryable());
                                    memberRepo = mockRepo.Object;
                                };
    }
}
