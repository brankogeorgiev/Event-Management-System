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
        private readonly UserManager<Attendee> _userManager;

        public AdminController(IOrderService orderService, UserManager<Attendee> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
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

        [HttpGet("[action]")]
        public List<Order> GetAllOrders()
        {
            return _orderService.GetAllOrders();
        }

        [HttpPost("[action]")]
        public Order GetDetailsForOrder(BaseEntity model)
        {
            return _orderService.GetOrderDetails(model);
        }
    }
}
