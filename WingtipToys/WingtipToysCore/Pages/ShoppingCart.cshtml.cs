using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WingtipToys.Data;
using WingtipToys.Models;
using WingtipToys.Services;

namespace WingtipToys.Pages;

public class ShoppingCartModel : PageModel
{
    private readonly ShoppingCartService _cartService;
    private readonly ProductContext _context;

    public ShoppingCartModel(ShoppingCartService cartService, ProductContext context)
    {
        _cartService = cartService;
        _context = context;
    }

    public List<CartItem> CartItems { get; set; } = new();
    public decimal Total { get; set; }

    [BindProperty]
    public List<CartItemUpdate> CartItemUpdates { get; set; } = new();

    public void OnGet()
    {
        LoadCart();
    }

    public IActionResult OnPost()
    {
        foreach (var item in CartItemUpdates)
        {
            if (item.Remove)
            {
                _cartService.RemoveItem(item.ProductId);
            }
            else if (item.Quantity > 0)
            {
                _cartService.UpdateItem(item.ProductId, item.Quantity);
            }
            else
            {
                _cartService.RemoveItem(item.ProductId);
            }
        }

        return RedirectToPage();
    }

    private void LoadCart()
    {
        CartItems = _cartService.GetCartItems();
        // Load product details
        foreach (var item in CartItems)
        {
            if (item.Product == null)
            {
                item.Product = _context.Products.Find(item.ProductId);
            }
        }
        Total = _cartService.GetTotal();
    }

    public class CartItemUpdate
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool Remove { get; set; }
    }
}
