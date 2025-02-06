using EMS.Domain.DTO;
using EMS.Domain.Models;
using EMS.Domain.Settings;
using EMS.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace EMS.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        private readonly StripeSettings _stripeSettings;

        public ShoppingCartsController(IShoppingCartService shoppingCartService, 
            IOrderService orderService,
            IOptions<StripeSettings> stripeSettings)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
            _stripeSettings = stripeSettings.Value;
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

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var userCart = _shoppingCartService.GetShoppingCartDetails(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(userCart.TotalPrice) * 100),
                Description = "Event Management System Application Payment",
                Currency = "mkd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                this.Order();
                return RedirectToAction("SuccessPayment");
            }
            else
            {
                return RedirectToAction("NotSuccessPayment");
            }
 
            return null;
        }

        public IActionResult SuccessPayment()
        {
            return View();
        }

        public IActionResult NotSuccessPayment()
        {
            return View();
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();

            return View();
        }

        public IActionResult DeleteTicketFromShoppingCart(Guid? ticketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            var result = _shoppingCartService.DeleteFromShoppingCart(userId, ticketId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult GetCartItemCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            var cartItems = _shoppingCartService.GetShoppingCartDetails(userId ?? "").AllTickets;

            int itemCount = cartItems?.Count() ?? 0;
            return Json(new { count = itemCount });
        }
    }
}
