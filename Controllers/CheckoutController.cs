using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Data;
using ECommerceApp.Models;
using System.Linq;
using System.Security.Claims;
using System;
using System.Collections.Generic;

namespace ECommerceApp.Controllers
{
    [Authorize] 
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PlaceOrder([FromBody] OrderRequestModel orderRequest)
        {
            if (orderRequest.CartItems == null || !orderRequest.CartItems.Any())
                return BadRequest("Cart is empty.");

            if (string.IsNullOrWhiteSpace(orderRequest.Address) || string.IsNullOrWhiteSpace(orderRequest.Contact))
                return BadRequest("Address and Contact are required.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not logged in.");

            // Ensure all ProductIds exist in the database
            var productIds = orderRequest.CartItems.Select(i => i.ProductId).ToList();
            var existingProducts = _context.Products.Where(p => productIds.Contains(p.Id)).Select(p => p.Id).ToList();

            foreach (var item in orderRequest.CartItems)
            {
                if (!existingProducts.Contains(item.ProductId))
                {
                    return BadRequest($"Product with ID {item.ProductId} does not exist.");
                }
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = orderRequest.CartItems.Sum(item => item.Price * item.Quantity),
                Status = "Pending",
                Address = orderRequest.Address,
                Contact = orderRequest.Contact,
                PaymentMethod = orderRequest.PaymentMethod,
                OrderDetails = orderRequest.CartItems.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // This will also save OrderDetails because of the relationship

            return Json(new { success = true, message = "Order placed successfully!" });
        }


    }
}
