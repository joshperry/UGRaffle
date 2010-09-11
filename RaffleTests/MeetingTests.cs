using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain;
using Moq;
using RaffleLib.Domain.Queries;
using RaffleWeb.Controllers;
using System.Web.Mvc;

namespace RaffleTests
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

        [TestMethod]
        public void Can_view_current_meeting()
        {
            var meeting = new Meeting { Date = DateTime.Today };
            var query = new Mock<IGetCurrentMeetingAndRaffleItems>();
            query.Setup(x => x.Result()).Returns(meeting);

            var result = new MeetingController(null).Index(query.Object);

            Assert.AreEqual(meeting, result.ViewData.Model);
            Assert.AreEqual(string.Empty, result.ViewName); // default view
        }

        [TestMethod]
        public void Can_view_meeting_list()
        {
            var meetings = new Meeting[] { new Meeting(), new Meeting() };
            var query = new Mock<IGetAllMeetings>();
            query.Setup(x => x.Result(100, 1)).Returns(meetings.AsQueryable());

            var result = new MeetingController(null).List(query.Object);

            Assert.AreEqual(meetings.Length, ((IEnumerable<Meeting>)result.ViewData.Model).Count());
            Assert.AreEqual(string.Empty, result.ViewName); // default view
        }

        [TestMethod]
        public void Can_create_meeting()
        {
            var meeting = new Meeting();
            var repo = GetMeetingRepoMock();
            repo.Setup(x => x.Save(meeting)).Verifiable();

            var result = new MeetingController(repo.Object).Create(meeting) as RedirectToRouteResult;

            repo.Verify();
            Assert.IsNotNull(result, "result was not a redirect");
            Assert.AreEqual("List", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Can_delete_meeting()
        {
            var meeting = new Meeting();
            var repo = GetMeetingRepoMock(meeting);
            repo.Setup(x => x.Delete(meeting)).Verifiable();
            var query = new Mock<IGetMeetingById>();
            query.Setup(x => x.Result(meeting.Id)).Returns(meeting);

            var result = new MeetingController(repo.Object).Delete(query.Object, meeting.Id) as RedirectToRouteResult;

            repo.Verify();
            Assert.IsNotNull(result, "result was not a redirect");
            Assert.AreEqual("List", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Can_get_meeting_to_edit()
        {
            var meeting = new Meeting();
            var repo = GetMeetingRepoMock(meeting);
            var query = new Mock<IGetMeetingById>();
            query.Setup(x => x.Result(meeting.Id)).Returns(meeting);

            var result = new MeetingController(repo.Object).Edit(query.Object, meeting.Id);

            Assert.AreEqual(meeting, result.ViewData.Model);
            Assert.AreEqual(string.Empty, result.ViewName); // default view
        }

        [TestMethod]
        public void Can_edit_meeting()
        {
            var meeting = new Meeting();
            var repo = GetMeetingRepoMock();
            repo.Setup(x => x.Save(meeting)).Verifiable();

            var result = new MeetingController(repo.Object).Edit(meeting) as RedirectToRouteResult;

            repo.Verify();
            Assert.IsNotNull(result, "result was not a redirect");
            Assert.AreEqual("List", result.RouteValues["action"]);
        }
    }
}
