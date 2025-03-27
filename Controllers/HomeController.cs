using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.Models;

namespace ECommerceApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        var categories = await _context.Categories.ToListAsync();
        var products = _context.Products.Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue && categoryId != 0)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }

        // Convert to list and shuffle products
        var random = new Random();
        var shuffledProducts = (await products.ToListAsync()).OrderBy(p => random.Next()).ToList();

        var viewModel = new HomeViewModel
        {
            Categories = categories,
            Products = shuffledProducts,
            SelectedCategoryId = categoryId ?? 0
        };

        return View(viewModel);
    }
    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    public IActionResult Cart()
    {
        return View();
    }





    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
