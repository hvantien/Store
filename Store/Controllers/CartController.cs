using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Models;

public class CartController : Controller
{
    [HttpPost]
    public IActionResult AddToCart(CartItem item)
    {
        List<CartItem> cartItems;
        var existingCart = Request.Cookies["Cart"];

        if (!string.IsNullOrEmpty(existingCart))
        {
            cartItems = JsonConvert.DeserializeObject<List<CartItem>>(existingCart);
        }
        else
        {
            cartItems = new List<CartItem>();
        }

        cartItems.Add(item);
        Response.Cookies.Append("Cart", JsonConvert.SerializeObject(cartItems));

        return Redirect(Request.Headers["Referer"].ToString());
    }

    public IActionResult Index()
    {
        var cartItems = new List<CartItem>();
        var existingCart = Request.Cookies["Cart"];

        if (!string.IsNullOrEmpty(existingCart))
        {
            cartItems = JsonConvert.DeserializeObject<List<CartItem>>(existingCart);
        }

        return View(cartItems);
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        var cartItems = new List<CartItem>();
        var existingCart = Request.Cookies["Cart"];

        if (!string.IsNullOrEmpty(existingCart))
        {
            cartItems = JsonConvert.DeserializeObject<List<CartItem>>(existingCart);
            cartItems.RemoveAll(item => item.ProductId == productId);
            Response.Cookies.Append("Cart", JsonConvert.SerializeObject(cartItems));
        }

        return RedirectToAction("Index");
    }
}
