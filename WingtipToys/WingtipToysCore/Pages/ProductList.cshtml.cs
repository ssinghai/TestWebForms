using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WingtipToys.Data;
using WingtipToys.Models;

namespace WingtipToys.Pages;

public class ProductListModel : PageModel
{
    private readonly ProductContext _context;

    public ProductListModel(ProductContext context)
    {
        _context = context;
    }

    public List<Product> Products { get; set; } = new();

    public void OnGet(int? id)
    {
        IQueryable<Product> query = _context.Products;

        if (id.HasValue && id > 0)
        {
            query = query.Where(p => p.CategoryID == id);
        }

        Products = query.ToList();
    }
}
