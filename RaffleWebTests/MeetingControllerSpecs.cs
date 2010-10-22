using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain.Queries;
using RaffleLib.Domain;
using RaffleWeb.Controllers;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace RaffleWebTests
{
    [Subject(typeof(MeetingController), "List Meetings")]
    public class when_displaying_list_of_meetings : with_get_all_query
    {
        static ViewResult result;

        Because of = () => result = new MeetingController(null).List(query);

        It should_display_a_view = () => result.ShouldNotBeNull();

        It should_display_the_default_view = () => result.ViewName.ShouldBeEmpty();

        It should_set_view_model_to_list_of_meetings = () => ((IEnumerable<Meeting>)result.ViewData.Model).ShouldContainOnly(meetings);
    }

    [Subject(typeof(MeetingController), "Create Meeting")]
    public class when_displaying_create_meeting_view : with_empty_meeting_and_repo
    {
        static ViewResult result;

        Because of = () => result = new MeetingController(null).Create();

        It should_display_a_view = () => result.ShouldNotBeNull();

        It should_display_default_view = () => result.ViewName.ShouldBeEmpty();

        It should_set_view_model_to_empty_meeting = () => result.ViewData.Model.ShouldNotBeNull();
    }

    [Subject(typeof(MeetingController), "Create Meeting")]
    public class when_submitting_meeting_create_view_with_valid_data : with_empty_meeting_and_repo
    {
        static RedirectToRouteResult result;

        Because of = () => result = new MeetingController(repoMock.Object).Create(meeting) as RedirectToRouteResult;

        It should_save_meeting_to_repository = () => repoMock.Verify(x => x.Save(meeting), Times.Once());

        It should_redirect = () => result.ShouldNotBeNull();

        It should_redirect_to_meeting_list = () => result.RouteValues["action"].ShouldEqual("List");
    }

    [Subject(typeof(MeetingController), "Create Meeting")]
    public class when_submitting_meeting_create_view_with_invalid_data : with_empty_meeting_and_repo
    {
        static ViewResult result;

        Because of = () =>
                         {
                             var controller = new MeetingController(repoMock.Object);
                             controller.ModelState.AddModelError("Error", "Misshapen");
                             result = controller.Create(meeting) as ViewResult;
                         };

        It should_not_save_meeting_to_repository = () => repoMock.Verify(x => x.Save(meeting), Times.Never());

        It should_display_a_view = () => result.ShouldNotBeNull();

        It should_display_default_view = () => result.ViewName.ShouldBeEmpty();
    }

    [Subject(typeof(MeetingController), "Delete Meeting")]
    public class when_deleting_an_existing_meeting : with_get_by_id_query
    {
        static RedirectToRouteResult result;

        Because of = () => result = new MeetingController(repoMock.Object).Delete(query, meeting.Id);

        It should_delete_meeting_from_repo = () => repoMock.Verify(x => x.Delete(meeting), Times.Once());

        It should_redirect = () => result.ShouldNotBeNull();

        It should_redirect_to_meeting_list = () => result.RouteValues["action"].ShouldEqual("List");
    }

    [Subject(typeof(MeetingController), "Delete Meeting")]
    public class when_deleting_a_nonexisting_meeting : with_get_by_id_query
    {
        static RedirectToRouteResult result;

        Because of = () => result = new MeetingController(repoMock.Object).Delete(query, Guid.NewGuid());

        It should_not_delete_meeting_from_repo = () => repoMock.Verify(x => x.Delete(Moq.It.IsAny<Meeting>()), Times.Never());

        It should_redirect = () => result.ShouldNotBeNull();

        It should_redirect_to_meeting_list = () => result.RouteValues["action"].ShouldEqual("List");
    }

    [Subject(typeof(MeetingController), "Edit Meeting")]
    public class when_displaying_edit_view_for_existing_meeting : with_get_by_id_query
    {
        static ViewResult result;

        Because of = () => result = new MeetingController(repoMock.Object).Edit(query, meeting.Id) as ViewResult;

        It should_display_a_view = () => result.ShouldNotBeNull();

        It should_display_default_view = () => result.ViewName.ShouldEqual(string.Empty);

        It should_set_view_model_to_meeting = () => result.ViewData.Model.ShouldBeTheSameAs(meeting);
    }

    [Subject(typeof(MeetingController), "Edit Meeting")]
    public class when_displaying_edit_view_for_nonexisting_meeting : with_get_by_id_query
    {
        static RedirectToRouteResult result;

        Because of = () => result = new MeetingController(repoMock.Object).Edit(query, Guid.Empty) as RedirectToRouteResult;

        It should_redirect_to_route = () => result.ShouldNotBeNull();

        It should_redirect_to_meeting_list = () => result.RouteValues["action"].ShouldEqual("List");
    }

    [Subject(typeof(MeetingController), "Edit Meeting")]
    public class when_saving_meeting_edit_view_with_valid_data : with_empty_meeting_and_repo
    {
        static RedirectToRouteResult result;

        Because of = () => result = new MeetingController(repoMock.Object).Edit(meeting) as RedirectToRouteResult;

        It should_save_meeting_to_repository = () => repoMock.Verify(x => x.Save(meeting), Times.Once());

        It should_redirect_to_route = () => result.ShouldNotBeNull();

        It should_redirect_to_meeting_list = () => result.RouteValues["action"].ShouldEqual("List");
    }

    [Subject(typeof(MeetingController), "Edit Meeting")]
    public class when_saving_meeting_edit_view_with_invalid_data : with_empty_meeting_and_repo
    {
        static ViewResult result;

        Because of = () =>
                         {
                             var controller = new MeetingController(repoMock.Object);
                             controller.ModelState.AddModelError("Error", "Bunk Data");
                             result = controller.Edit(meeting) as ViewResult;
                         };

        It should_not_save_meeting_to_repository = () => repoMock.Verify(x => x.Save(Moq.It.IsAny<Meeting>()), Times.Never());

        It should_display_a_view = () => result.ShouldNotBeNull();

        It should_display_default_view = () => result.ViewName.ShouldEqual(string.Empty);

        It should_set_view_model_to_invalid_meeting = () => result.ViewData.Model.ShouldBeTheSameAs(meeting);
    }

    public abstract class with_get_all_query : with_empty_meeting_and_repo
    {
        protected static Meeting[] meetings = new[] {meeting, new Meeting(), new Meeting(),};
        protected static IGetAllMeetings query;

        Establish context = () =>
                                {
                                    var queryMock = new Mock<IGetAllMeetings>();
                                    queryMock.Setup(x => x.Result(Moq.It.IsAny<int>(), Moq.It.IsAny<int>())).Returns(meetings.AsQueryable());
                                    query = queryMock.Object;
                                };
    }

    public abstract class with_get_by_id_query : with_empty_meeting_and_repo
    {
        protected static IGetMeetingById query;

        Establish context = () =>
                                {
                                    var queryMock = new Mock<IGetMeetingById>();
                                    queryMock.Setup(x => x.Result(Moq.It.IsAny<Guid>())).Returns((Meeting) null);
                                    queryMock.Setup(x => x.Result(meeting.Id)).Returns(meeting);
                                    query = queryMock.Object;
                                };
    }

    public abstract class with_empty_meeting_and_repo
    {
        protected static readonly Meeting meeting = new Meeting{ Id = Guid.NewGuid() };
        protected static readonly Mock<IEntityRepository<Meeting>> repoMock = new Mock<IEntityRepository<Meeting>>();
    }
}
