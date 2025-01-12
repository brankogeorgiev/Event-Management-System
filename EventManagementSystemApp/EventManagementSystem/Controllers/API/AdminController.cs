using EMS.Domain.Models;
using EMS.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Web.Controllers.API
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;

        public AdminController(IOrderService orderService)
        {
            _orderService = orderService;
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
