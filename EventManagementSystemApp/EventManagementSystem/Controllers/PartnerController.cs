using EMS.Domain.PartnerModels;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Web.Controllers
{
    public class PartnerController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string booksUrl = "https://bookshopappis.azurewebsites.net/api/AdminApp/GetAllBooks";
            string authorsUrl = "https://bookshopappis.azurewebsites.net/api/AdminApp/GetAllAuthors";

            HttpResponseMessage response = client.GetAsync(booksUrl).Result;
            var allBooks = response.Content.ReadAsAsync<List<Book>>().Result;

            HttpResponseMessage response2 = client.GetAsync(authorsUrl).Result;
            var allAuthors = response2.Content.ReadAsAsync<List<Author>>().Result;

            foreach (var book in allBooks)
            {
                var author = allAuthors.Where(z => z.Id == book.authorId).FirstOrDefault();
                book.author = author;
            }

            return View(allBooks);
        }
    }
}
