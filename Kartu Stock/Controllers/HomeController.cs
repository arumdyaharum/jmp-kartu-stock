using Kartu_Stock.Data;
using Kartu_Stock.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kartu_Stock.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public ActionResult Index(
            [FromQuery] string? StartDate,
            [FromQuery] string? EndDate,
            [FromQuery] string? ProductID
        )
        {
            string StartDateModify = !string.IsNullOrEmpty(StartDate) ? StartDate : DateTime.Now.ToString("yyyy-MM-dd");
            string EndDateModify = !string.IsNullOrEmpty(EndDate) ? EndDate : DateTime.Now.ToString("yyyy-MM-dd");
            string ProductIDModify = !string.IsNullOrEmpty(ProductID) ? ProductID : "";

            ViewData["StartDate"] = StartDateModify;
            ViewData["EndDate"] = EndDateModify;
            ViewData["ProductID"] = ProductIDModify;

            var products = _context.GetProducts();

            if (
                !string.IsNullOrEmpty(StartDate)
                && !string.IsNullOrEmpty(EndDate)
                && (
                    DateOnly.Parse(StartDate) > DateOnly.FromDateTime(DateTime.Now)
                    || DateOnly.Parse(EndDate) > DateOnly.FromDateTime(DateTime.Now)
                    || DateOnly.Parse(StartDate) > DateOnly.Parse(EndDate)
                )
               )
            {
                if (DateOnly.Parse(StartDate) > DateOnly.FromDateTime(DateTime.Now))
                {
                    ViewData["AlertMessage"] = "Tanggal awal tidak boleh lebih maju daripada hari ini!";
                }
                else if (DateOnly.Parse(EndDate) > DateOnly.FromDateTime(DateTime.Now))
                {
                    ViewData["AlertMessage"] = "Tanggal akhir tidak boleh lebih maju daripada hari ini!";
                }
                else if (DateOnly.Parse(StartDate) > DateOnly.Parse(EndDate))
                {
                    ViewData["AlertMessage"] = "Tanggal awal tidak boleh lebih maju daripada tanggal akhir!";
                }
                List<TransactionModel> transactionsError = new List<TransactionModel>();

                var viewModelError = new IndexViewModel
                {
                    Transactions = transactionsError,
                    Products = products
                };
                return View(viewModelError);
            }

            ViewData["HideAlert"] = "display: none;";
            var transactions = _context.GetTransactions(StartDateModify, EndDateModify, ProductIDModify);
            
            int transactionLength = transactions.Count;
            var tempBalance = 0;
            for (int i = 0; i < transactionLength; i++)
            {
                tempBalance += transactions[i].PurchaseQuantity;
                tempBalance -= transactions[i].SalesQuantity;
                transactions[i].Balance = tempBalance;
            }

            var viewModel = new IndexViewModel
            {
                Transactions = transactions,
                Products = products
            };
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
