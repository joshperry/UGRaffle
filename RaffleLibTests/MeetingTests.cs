using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain;
using Moq;
using RaffleLib.Domain.Queries;
using Machine.Specifications;

namespace RaffleLibTests
{
    [TestClass]
    public class MeetingTests
    {
        private static Mock<IEntityRepository<Meeting>> GetMeetingRepoMock(params Meeting[] meetings)
        {
            var repo = new Mock<IEntityRepository<Meeting>>();
            repo.SetupGet(x => x.Query).Returns(meetings.AsQueryable());
            return repo;
        }

        [TestMethod]
        public void Can_query_current_meeting()
        {
            var meeting = new Meeting { Date = DateTime.Today };
            var repo = GetMeetingRepoMock(meeting);

            var result = new GetCurrentMeetingAndRaffleItems(repo.Object).Result();

            Assert.AreEqual(meeting, result);
        }

        [TestMethod]
        public void Can_query_meeting_by_id()
        {
            var meeting = new Meeting();
            var repo = GetMeetingRepoMock(meeting);

            var result = new GetMeetingById(repo.Object).Result(meeting.Id);

            Assert.AreEqual(meeting, result);
        }

        [TestMethod]
        public void Can_query_all_meetings()
        {
            var repo = GetMeetingRepoMock(new Meeting(), new Meeting());

            var result = new GetAllMeetings(repo.Object).Result();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Can_query_page_all_meetings()
        {
            var repo = GetMeetingRepoMock(new Meeting(), new Meeting(), new Meeting(), new Meeting());

            var result = new GetAllMeetings(repo.Object).Result(2, 1);

            CollectionAssert.AreEqual(repo.Object.Query.Take(2).ToArray(), result.ToArray());
        }
    }

    public abstract class with_meeting_for_today
    {
        
    }
}
