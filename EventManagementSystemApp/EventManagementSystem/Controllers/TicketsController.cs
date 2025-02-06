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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EMS.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScheduledEventService _scheduledEventService;
        private readonly ITicketService _ticketService;
        private readonly IShoppingCartService _shoppingCartService;

        public TicketsController(ApplicationDbContext context, 
            IScheduledEventService scheduledEventService,
            ITicketService ticketService,
            IShoppingCartService shoppingCartService)
        {
            _context = context;
            _scheduledEventService = scheduledEventService;
            _ticketService = ticketService;
            _shoppingCartService = shoppingCartService;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            return View(_ticketService.GetAllTickets());
        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            TicketDTO dto = new TicketDTO()
            {
                ListModel =  _scheduledEventService.GetAllScheduledEvents()
            };
            return View(dto);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TicketPrice,ScheduledEventId,Id")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                _ticketService.CreateNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("TicketPrice,ScheduledEventId,Id")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ticketService.UpdateTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var ticket = _ticketService.GetTicketById(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult AddToShoppingCart(Guid id)
        {
            var result = _shoppingCartService.GetTicketInfo(id);
            if (result != null)
            {
                return View(result);
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddToShoppingCart(AddToShoppingCartDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            var result = _shoppingCartService.AddTicketToShoppingCart(userId, model);
            if (result != null)
            {
                return RedirectToAction("Index", "ShoppingCarts");
            }
            else
            {
                return View(model);
            }
        }

        private bool TicketExists(Guid id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
