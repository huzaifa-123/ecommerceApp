using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get total orders
            int totalOrders = await _context.Orders.CountAsync();

            // Get total users
            int totalUsers = await _context.Users.CountAsync();

            // Find product with most orders
            var mostOrderedProduct = await _context.OrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    OrderCount = g.Count()
                })
                .OrderByDescending(g => g.OrderCount)
                .FirstOrDefaultAsync();

            var mostOrderedProductName = mostOrderedProduct != null
                ? _context.Products.FirstOrDefault(p => p.Id == mostOrderedProduct.ProductId)?.Name
                : "Not enough data for now";

            // Find product with least orders
            var leastOrderedProduct = await _context.OrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    OrderCount = g.Count()
                })
                .OrderBy(g => g.OrderCount)
                .FirstOrDefaultAsync();

            var leastOrderedProductName = leastOrderedProduct != null
                ? _context.Products.FirstOrDefault(p => p.Id == leastOrderedProduct.ProductId)?.Name
                : "Not enough data for now";

            // Pass data to View
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.MostOrderedProduct = mostOrderedProductName;
            ViewBag.LeastOrderedProduct = leastOrderedProductName;

            return View();
        }
    }
}
