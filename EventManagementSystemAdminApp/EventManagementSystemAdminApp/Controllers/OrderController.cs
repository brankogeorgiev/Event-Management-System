using ClosedXML.Excel;
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

        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");
                worksheet.Cell(1, 1).Value = "Order ID";
                worksheet.Cell(1, 2).Value = "Customer Email";
                
                HttpClient client = new HttpClient();
                string URL = "https://localhost:44304/Api/Admin/GetAllOrders";
                HttpResponseMessage message = client.GetAsync(URL).Result;
                var result = message.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 0; i < result.Count(); i++)
                {
                    var item = result.ElementAt(i);

                    worksheet.Cell(i + 2, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = item.Owner.Email.ToString();

                    var ticketColumn = 3;
                    for (int j = 0; j < item.TicketsInOrder.Count(); j++)
                    {
                        var ticket = item.TicketsInOrder.ElementAt(j);

                        worksheet.Cell(1, ticketColumn).Value = "Ticket-" + (j + 1);
                        worksheet.Cell(1, ticketColumn + 1).Value = "Quantity for Ticket-" + (j + 1);

                        worksheet.Cell(i + 2, ticketColumn).Value = ticket.Ticket.TicketDisplayString;
                        worksheet.Cell(i + 2, ticketColumn + 1).Value = ticket.Quantity;

                        ticketColumn += 2;
                    }
                }
                
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }
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
