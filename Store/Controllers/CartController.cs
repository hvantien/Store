using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Models;

public class CartController : Controller
{
    [HttpPost]
    public IActionResult AddToCart(OrderItem item)
    {
        List<OrderItem> OrderItems;
        var existingCart = Request.Cookies["Cart"];

        if (!string.IsNullOrEmpty(existingCart))
        {
            OrderItems = JsonConvert.DeserializeObject<List<OrderItem>>(existingCart);
        }
        else
        {
            OrderItems = new List<OrderItem>();
        }

        OrderItems.Add(item);
        Response.Cookies.Append("Cart", JsonConvert.SerializeObject(OrderItems));

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public IActionResult Index()
    {
        var OrderItems = new List<OrderItem>();
        var existingCart = Request.Cookies["Cart"];

        if (!string.IsNullOrEmpty(existingCart))
        {
            OrderItems = JsonConvert.DeserializeObject<List<OrderItem>>(existingCart);
        }

        return View(OrderItems);
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        var OrderItems = new List<OrderItem>();
        var existingCart = Request.Cookies["Cart"];

        if (!string.IsNullOrEmpty(existingCart))
        {
            OrderItems = JsonConvert.DeserializeObject<List<OrderItem>>(existingCart);
            OrderItems.RemoveAll(item => item.ProductId == productId);
            Response.Cookies.Append("Cart", JsonConvert.SerializeObject(OrderItems));
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult EditFromCart(int productId, int newQuantity)
    {
        var OrderItems = new List<OrderItem>();
        var existingCart = Request.Cookies["Cart"];

        if (!string.IsNullOrEmpty(existingCart))
        {
            OrderItems = JsonConvert.DeserializeObject<List<OrderItem>>(existingCart);

            var itemToEdit = OrderItems.FirstOrDefault(item => item.ProductId == productId);

            if (itemToEdit != null)
            {
                itemToEdit.Quantity = newQuantity;

                Response.Cookies.Append("Cart", JsonConvert.SerializeObject(OrderItems));
            }
        }

        return RedirectToAction("Index");
    }
}
