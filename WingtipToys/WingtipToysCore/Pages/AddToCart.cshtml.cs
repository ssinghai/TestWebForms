using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WingtipToys.Services;

namespace WingtipToys.Pages;

public class AddToCartModel : PageModel
{
    private readonly ShoppingCartService _cartService;

    public AddToCartModel(ShoppingCartService cartService)
    {
        _cartService = cartService;
    }

    public IActionResult OnGet(int? productID)
    {
        if (productID.HasValue && productID > 0)
        {
            _cartService.AddToCart(productID.Value);
        }
        return RedirectToPage("/ShoppingCart");
    }
}
