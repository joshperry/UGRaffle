using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Queries;
using RaffleLib.Domain.Entities;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace RaffleWebTests
{
    [Subject(typeof(Meeting))]
    public class when_displaying_index_view_for_no_current_meeting : with_no_current_meeting
    {
        
    }

    [Subject(typeof(Meeting))]
    public class when_displaying_index_view_for_current_meeting : with_current_meeting
    {
        
    }

    [Subject(typeof(Meeting))]
    public class when_displaying_index_view_for_unauthenticated_member : with_queries_and_null_entities
    {
        
    }

    public abstract class with_current_meeting : with_no_current_meeting
    {
        Establish context = () =>
        {
            meeting = new Meeting { Date = DateTime.Now, Id = Guid.NewGuid() };
        };
    }

    public abstract class with_no_current_meeting : with_queries_and_null_entities
    {
        Establish context = () => {
            member = new Member { Id = memberId, Email = memberEmail };
        };
    }

    public abstract class with_queries_and_null_entities
    {
        protected static Guid memberId = Guid.NewGuid();
        protected static string memberEmail = "me@example.com";
        protected static Member member = null;
        protected static Meeting meeting = null;

        protected static IGetCurrentMeetingAndRaffleItems meetingQuery;
        protected static IGetMemberByEmail memberByEmailQuery;
        protected static IGetMemberById memberByIdQuery;

        Establish context = () => {
            var meetingQueryMock = new Mock<IGetCurrentMeetingAndRaffleItems>();
            meetingQueryMock.Setup(q => q.Result()).Returns(meeting);
            meetingQuery = meetingQueryMock.Object;

            var memberByEmailQueryMock = new Mock<IGetMemberByEmail>();
            memberByEmailQueryMock.Setup(q => q.Result(Moq.It.IsAny<string>())).Returns(member);
            memberByEmailQuery = memberByEmailQueryMock.Object;

            var memberByIdQueryMock = new Mock<IGetMemberById>();
        };
    }
}
