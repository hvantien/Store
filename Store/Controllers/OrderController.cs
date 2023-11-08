using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Store.Data;
using Store.Models;
using System.Data;

namespace Store.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Detail(int id)
        {
            // Lấy thông tin chi tiết về đơn hàng dựa trên id
            var order = _context.Orders.Where(x => x.OrderId == id).Include(p => p.Items);
            var orders = order.FirstOrDefault();
            if (orders == null)
            {
                // Xử lý trường hợp đơn hàng không tồn tại
                return NotFound();
            }

            return View(orders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                var OrderItems = new List<OrderItem>();
                var existingCart = Request.Cookies["Cart"];

                if (!string.IsNullOrEmpty(existingCart))
                {
                    OrderItems = JsonConvert.DeserializeObject<List<OrderItem>>(existingCart);
                }
                if(OrderItems != null)
                {
                    order.Items = OrderItems;
                    _context.SaveChanges();

                    return RedirectToAction("OrderSuccess");
                }
                return View(order);
            }
            catch
            {
                return View(order);
            }
            
        }

        public IActionResult OrderSuccess()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return View();
        }
    }
}
