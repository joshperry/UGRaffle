﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain;
using RaffleLib.Domain.Queries;
using RaffleLib.Domain.Repositories.Fake;
using WebLib.Auth;

namespace RaffleWeb.Controllers
{
    [Authorize(Roles="Admin")]
    public class MeetingController : Controller
    {
        private IEntityRepository<Meeting> _meetingRepo;
        public MeetingController(IEntityRepository<Meeting> meetingRepo)
        {
            _meetingRepo = meetingRepo;
        }

        public ViewResult List(IGetAllMeetings query)
        {
            return View(query.Result());
        }

        public RedirectToRouteResult Delete(IGetMeetingById query, Guid id)
        {
            var meeting = query.Result(id);
            if (meeting != null)
                _meetingRepo.Delete(meeting);

            return RedirectToAction("List");
        }

        public ViewResult Create()
        {
            return View(new Meeting { Date = DateTime.Today, TicketsForRegistering = 10 });
        }

        [HttpPost]
        public ActionResult Create(Meeting meeting)
        {
            if(!ModelState.IsValid)
                return View(meeting);

            _meetingRepo.Save(meeting);

            return RedirectToAction("List");
        }

        public ActionResult Edit(IGetMeetingById query, Guid id)
        {
            var meeting = query.Result(id);

            if(meeting == null)
                return RedirectToAction("List");

            return View(meeting);
        }

        [HttpPost]
        public ActionResult Edit(Meeting meeting)
        {
            if(!ModelState.IsValid)
                return View(meeting);

            _meetingRepo.Save(meeting);

            return RedirectToAction("List");
        }
    }
}
