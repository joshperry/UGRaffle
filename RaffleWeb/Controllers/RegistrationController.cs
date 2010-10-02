using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RaffleLib.Domain.Queries;
using RaffleLib.Domain;
using RaffleLib.Domain.Entities;

namespace RaffleWeb.Controllers
{
    public class RegistrationController : Controller
    {
        private IEntityRepository<Registration> _repo;
        public RegistrationController(IEntityRepository<Registration> repo)
        {
            _repo = repo;
        }

        public ViewResult Index(IGetCurrentMeetingAndRaffleItems query)
        {
            return View(query.Result());
        }

        [Authorize]
        public ViewResult Register(IGetMemberRegistration regQuery,
            IGetCurrentMeetingAndRaffleItems meetingQuery,
            IGetMemberByEmail memberQuery)
        {
            var member = memberQuery.Result(User.Identity.Name);
            var meeting = meetingQuery.Result();
            var registration = regQuery.Result(meeting, member);

            return View(registration);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Register(IGetMemberRegistration regQuery,
            IGetCurrentMeetingAndRaffleItems meetingQuery,
            IGetMemberById memberQuery, Guid memberId)
        {
            var member = memberQuery.Result(memberId);
            var meeting = meetingQuery.Result();

            if (member != null && meeting != null)
            {
                var registration = meeting.Register(member);
                _repo.Save(registration);
            }

            return RedirectToAction("Enter", "Raffle");
        }
    }
}
