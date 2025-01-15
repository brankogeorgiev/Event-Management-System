using EMS.Domain.DTO;
using EMS.Domain.Identity;
using EMS.Domain.Models;
using EMS.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Web.Controllers.API
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IEventService _eventService;
        private readonly IScheduledEventService _scheduledEventService;
        private readonly ITicketService _ticketService;
        private readonly UserManager<Attendee> _userManager;

        public AdminController(IOrderService orderService, 
            IEventService eventService,
            IScheduledEventService scheduledEventService,
            ITicketService ticketService,
            UserManager<Attendee> userManager)
        {
            _orderService = orderService;
            _eventService = eventService;
            _scheduledEventService = scheduledEventService;
            _ticketService = ticketService;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        public List<Order> GetAllOrders()
        {
            return _orderService.GetAllOrders();
        }

        [HttpGet("[action]")]
        public List<Event> GetAllEvents()
        {
            return _eventService.GetAllEvents();
        }

        [HttpGet("[action]")]
        public List<ScheduledEvent> GetAllScheduledEvents()
        {
            return _scheduledEventService.GetAllScheduledEvents();
        }

        [HttpGet("[action]")]
        public List<Ticket> GetAllTickets()
        {
            return _ticketService.GetAllTickets().ToList();
        }

        [HttpPost("[action]")]
        public Order GetDetailsForOrder(BaseEntity model)
        {
            return _orderService.GetOrderDetails(model);
        }



        [HttpPost("[action]")]
        public bool ImportAllAttendees(List<AttendeeRegistrationDto> attendees)
        {
            bool status = true;
            foreach  (var  attendee in attendees)
            {
                var userCheck = _userManager.FindByEmailAsync(attendee.Email).Result;
                if (userCheck == null)
                {
                    var newAttendee = new Attendee
                    {
                        FirstName = attendee.FirstName, 
                        LastName = attendee.LastName, 
                        Address = attendee.Address,
                        UserName = attendee.Email,
                        NormalizedUserName = attendee.Email,
                        Email = attendee.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        ShoppingCart = new ShoppingCart()
                    };
                    var result = _userManager.CreateAsync(newAttendee, attendee.Password).Result;

                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }
            return status;
        }
    }
}
