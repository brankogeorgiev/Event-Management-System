using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMS.Domain.Models;
using EMS.Repository;
using EMS.Domain.DTO;
using EMS.Service.Interface;

namespace EMS.Web.Controllers
{
    public class ScheduledEventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly IScheduledEventService _scheduledEventService;
        private readonly ITicketService _ticketService;

        public ScheduledEventsController(ApplicationDbContext context, 
            IEventService eventService,
            IScheduledEventService scheduledEventService,
            ITicketService ticketService)
        {
            _context = context;
            _eventService = eventService;
            _scheduledEventService = scheduledEventService;
            _ticketService = ticketService;
        }

        // GET: ScheduledEvents
        public IActionResult Index()
        {
            var scheduledEvents = _scheduledEventService.GetAllScheduledEvents();
            return View(scheduledEvents);
        }

        // GET: ScheduledEvents/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledEvent = _scheduledEventService.GetScheduledEventById(id);
            scheduledEvent.Tickets = _ticketService.GetAllTicketsByScheduledEventId(scheduledEvent.Id);
            if (scheduledEvent == null)
            {
                return NotFound();
            }

            return View(scheduledEvent);
        }

        // GET: ScheduledEvents/Create
        public IActionResult Create()
        {
            ScheduledEventDTO dto = new ScheduledEventDTO()
            {
                ListModel = _eventService.GetAllEvents(),
                ScheduledEventDateTime = DateTime.Now
            };
            return View(dto);
        }

        // POST: ScheduledEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ScheduledEventDateTime,ScheduledEventLocation,ScheduledEventImage,EventId,Id")] ScheduledEvent scheduledEvent)
        {
            if (ModelState.IsValid)
            {
                scheduledEvent.Id = Guid.NewGuid();
                _scheduledEventService.CreateNewScheduledEvent(scheduledEvent);
                return RedirectToAction(nameof(Index));
            }
            return View(scheduledEvent);
        }

        // GET: ScheduledEvents/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledEvent = _scheduledEventService.GetScheduledEventById(id);
            if (scheduledEvent == null)
            {
                return NotFound();
            }
            return View(scheduledEvent);
        }

        // POST: ScheduledEvents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("ScheduledEventDateTime,ScheduledEventLocation,ScheduledEventImage,EventId,Id")] ScheduledEvent scheduledEvent)
        {
            if (id != scheduledEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _scheduledEventService.UpdateNewScheduledEvent(scheduledEvent);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduledEventExists(scheduledEvent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(scheduledEvent);
        }

        // GET: ScheduledEvents/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduledEvent = _scheduledEventService.GetScheduledEventById(id);
            if (scheduledEvent == null)
            {
                return NotFound();
            }

            return View(scheduledEvent);
        }

        // POST: ScheduledEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var scheduledEvent = _scheduledEventService.GetScheduledEventById(id);
            if (scheduledEvent != null)
            {
                _scheduledEventService.DeleteNewScheduledEvent(scheduledEvent);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduledEventExists(Guid id)
        {
            return _scheduledEventService.GetScheduledEventById(id) != null;
        }
    }
}
