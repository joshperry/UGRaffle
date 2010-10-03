using System;
using System.Collections.Generic;
using System.Linq;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain;
using RaffleLib.Domain.Queries;
using Machine.Specifications;

namespace RaffleLibTests
{
    [Subject("Query Meetings")]
    public class when_querying_for_todays_meeting : with_meeting_for_today
    {
        private static Meeting meeting;

        Because of = () => meeting = new GetCurrentMeetingAndRaffleItems(meetingRepo).Result();

        It should_not_be_a_null_meeting = () => meeting.ShouldNotBeNull();

        It should_get_todays_meeting = () => meeting.Date.ShouldEqual(DateTime.Today);
    }

    public class when_querying_for_todays_meeting_and_there_is_not_a_meeting_today : with_list_of_meetings
    {
        private static Meeting meeting;

        Because of = () => meeting = new GetCurrentMeetingAndRaffleItems(meetingRepo).Result();

        It should_be_null = () => meeting.ShouldBeNull();
    }

    [Subject("Query Meetings")]
    public class when_querying_for_all_meetings : with_list_of_meetings
    {
        static IEnumerable<Meeting> meetings;

        Because of = () => meetings = new GetAllMeetings(meetingRepo).Result();

        It should_get_all_meetings = () => meetings.Count().ShouldEqual(meetingRepo.Query.Count());
    }
    
    [Subject("Query Meetings")]
    public class when_querying_all_meetings_and_paging : with_list_of_meetings
    {
        private const int PAGE_SIZE = 2;
        private const int PAGE_INDEX = 2;
        static IEnumerable<Meeting> meetings;

        Because of = () => meetings = new GetAllMeetings(meetingRepo).Result(PAGE_SIZE, PAGE_INDEX);

        It should_get_page_size_count_of_meetings = () => meetings.Count().ShouldEqual(PAGE_SIZE);

        It should_get_proper_meetings = () => meetings.ShouldContainOnly(meetingRepo.Query.Skip(PAGE_SIZE*(PAGE_INDEX-1)).Take(PAGE_INDEX));
    }

    public class when_querying_for_existing_meeting_by_id : with_list_of_meetings
    {
        private static Meeting meeting;

        Because of = () => meeting = new GetMeetingById(meetingRepo).Result(meetingRepo.Query.First().Id);

        It should_not_be_null = () => meeting.ShouldNotBeNull();

        It should_retrieve_meeting_with_specified_id = () => meeting.ShouldBeTheSameAs(meetingRepo.Query.First());
    }

    public abstract class with_meeting_for_today : list_of_meetings_base
    {
        protected with_meeting_for_today()
            : base(
                new Meeting{
                    Id = Guid.NewGuid(),
                    Date= DateTime.Today
                }
            )
        {
        }
    }

    public abstract class with_list_of_meetings : list_of_meetings_base
    {
        protected with_list_of_meetings()
            :base(
                new Meeting{ Id = Guid.NewGuid() },
                new Meeting{ Id = Guid.NewGuid() },
                new Meeting{ Id = Guid.NewGuid() },
                new Meeting{ Id = Guid.NewGuid() }
            )
        {
        }
    }

    public abstract class list_of_meetings_base
    {
        private static Meeting[] _meetings;
        protected list_of_meetings_base(params Meeting[] meetings)
        {
            _meetings = meetings;
        }

        protected static IEntityRepository<Meeting> meetingRepo;

        private Establish context = () =>
                                        {
                                            var mock = new Moq.Mock<IEntityRepository<Meeting>>();
                                            mock.SetupGet(x => x.Query).Returns(_meetings.AsQueryable());

                                            meetingRepo = mock.Object;
                                        };
    }
}
