using EMS.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartsController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            return View(_shoppingCartService.GetShoppingCartDetails(userId ?? ""));
        }

        public IActionResult Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            var result = _shoppingCartService.OrderTickets(userId ?? "");
            return RedirectToAction("Index", "ShoppingCarts");
        }

        public IActionResult DeleteTicketFromShoppingCart(Guid? ticketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            var result = _shoppingCartService.DeleteFromShoppingCart(userId, ticketId);

            return RedirectToAction(nameof(Index));
        }
    }
}
