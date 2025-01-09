using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMS.Domain.Models;
using EMS.Repository;
using EMS.Service.Interface;

namespace EMS.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IScheduledEventService _scheduledEventService;

        public EventsController(ApplicationDbContext context, 
            IEventService eventService,
            IScheduledEventService scheduledEventService)
        {
            _context = context;
            _eventService = eventService;
            _scheduledEventService = scheduledEventService;
        }

        // GET: Events
        public IActionResult Index()
        {
            return View(_eventService.GetAllEvents());
        }

        // GET: Events/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventDetails = _eventService.GetEventById(id);
            if (eventDetails == null)
            {
                return NotFound();
            }
            var scheduledEventsForEvent = _scheduledEventService.GetAllScheduledEventsByEventId(id);
            eventDetails.ScheduledEvents = scheduledEventsForEvent;

            return View(eventDetails);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EventName,EventImage,EventDescription,Id")] Event newEvent)
        {
            if (ModelState.IsValid)
            {
                newEvent.Id = Guid.NewGuid();
                _eventService.CreateNewEvent(newEvent);
                return RedirectToAction(nameof(Index));
            }
            return View(newEvent);
        }



        // GET: Events/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventDetails = _eventService.GetEventById(id);
            if (eventDetails == null)
            {
                return NotFound();
            }
            return View(eventDetails);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("EventName,EventImage,EventDescription,Id")] Event newEvent)
        {
            if (id != newEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _eventService.UpdateEvent(newEvent);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(newEvent);
        }

        // GET: Events/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventToDelete = _eventService.GetEventById(id);
            if (eventToDelete == null)
            {
                return NotFound();
            }

            return View(eventToDelete);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _eventService.DeleteEvent(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
