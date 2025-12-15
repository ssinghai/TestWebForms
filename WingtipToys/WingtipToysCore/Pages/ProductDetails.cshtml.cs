using Microsoft.AspNetCore.Mvc.RazorPages;
using WingtipToys.Data;
using WingtipToys.Models;

namespace WingtipToys.Pages;

public class ProductDetailsModel : PageModel
{
    private readonly ProductContext _context;

    public ProductDetailsModel(ProductContext context)
    {
        _context = context;
    }

    public Product? Product { get; set; }

    public void OnGet(int? productID)
    {
        if (productID.HasValue && productID > 0)
        {
            Product = _context.Products.FirstOrDefault(p => p.ProductID == productID);
        }
    }
}
