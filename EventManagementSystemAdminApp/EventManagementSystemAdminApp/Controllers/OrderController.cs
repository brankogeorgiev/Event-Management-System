using EventManagementSystemAdminApp.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EventManagementSystemAdminApp.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44304/Api/Admin/GetAllOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Order>>().Result;

            return View(data);
        }

        public IActionResult Details(Guid orderId)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44304/Api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = orderId
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(URL, content).Result;
            var data = response.Content.ReadAsAsync<Order>().Result;

            return View(data);
        }

        public FileContentResult CreateInvoice(Guid orderId)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44304/Api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = orderId
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage message = client.PostAsync(URL, content).Result;

            var result = message.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{Username}}", result.Owner.Email);

            StringBuilder sb = new StringBuilder();
            var totalPrice = 0.0;
            foreach (var item in result.TicketsInOrder)
            {
                sb.AppendLine(item.Ticket.TicketDisplayString + " with quantity of: " + item.Quantity + ", and price of: " + item.Ticket.TicketPrice + " MKD");
                totalPrice += item.Quantity * item.Ticket.TicketPrice;
            }
            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + " MKD");

            var stream = new MemoryStream();
            
            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}
