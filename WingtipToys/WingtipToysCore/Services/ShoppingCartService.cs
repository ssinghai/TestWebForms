using WingtipToys.Data;
using WingtipToys.Models;

namespace WingtipToys.Services;

public class ShoppingCartService
{
    private readonly ProductContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartSessionKey = "CartId";

    public ShoppingCartService(ProductContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCartId()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null)
            return Guid.NewGuid().ToString();

        var cartId = session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(cartId))
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (!string.IsNullOrWhiteSpace(user?.Identity?.Name))
            {
                cartId = user.Identity.Name;
            }
            else
            {
                cartId = Guid.NewGuid().ToString();
            }
            session.SetString(CartSessionKey, cartId);
        }
        return cartId;
    }

    public void AddToCart(int productId)
    {
        var cartId = GetCartId();

        var cartItem = _db.ShoppingCartItems
            .SingleOrDefault(c => c.CartId == cartId && c.ProductId == productId);

        if (cartItem == null)
        {
            var product = _db.Products.Find(productId);
            if (product != null)
            {
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    ProductId = productId,
                    CartId = cartId,
                    Product = product,
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };
                _db.ShoppingCartItems.Add(cartItem);
            }
        }
        else
        {
            cartItem.Quantity++;
        }
        _db.SaveChanges();
    }

    public List<CartItem> GetCartItems()
    {
        var cartId = GetCartId();
        return _db.ShoppingCartItems
            .Where(c => c.CartId == cartId)
            .ToList();
    }

    public decimal GetTotal()
    {
        var cartId = GetCartId();
        var total = _db.ShoppingCartItems
            .Where(c => c.CartId == cartId)
            .Sum(c => (decimal?)(c.Quantity * (c.Product != null ? c.Product.UnitPrice : 0)));
        return total ?? 0;
    }

    public void UpdateItem(int productId, int quantity)
    {
        var cartId = GetCartId();
        var cartItem = _db.ShoppingCartItems
            .FirstOrDefault(c => c.CartId == cartId && c.ProductId == productId);

        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            _db.SaveChanges();
        }
    }

    public void RemoveItem(int productId)
    {
        var cartId = GetCartId();
        var cartItem = _db.ShoppingCartItems
            .FirstOrDefault(c => c.CartId == cartId && c.ProductId == productId);

        if (cartItem != null)
        {
            _db.ShoppingCartItems.Remove(cartItem);
            _db.SaveChanges();
        }
    }

    public void EmptyCart()
    {
        var cartId = GetCartId();
        var cartItems = _db.ShoppingCartItems.Where(c => c.CartId == cartId);
        _db.ShoppingCartItems.RemoveRange(cartItems);
        _db.SaveChanges();
    }

    public int GetCount()
    {
        var cartId = GetCartId();
        return _db.ShoppingCartItems
            .Where(c => c.CartId == cartId)
            .Sum(c => (int?)c.Quantity) ?? 0;
    }

    public void MigrateCart(string userName)
    {
        var cartId = GetCartId();
        var shoppingCart = _db.ShoppingCartItems.Where(c => c.CartId == cartId);
        foreach (var item in shoppingCart)
        {
            item.CartId = userName;
        }
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.SetString(CartSessionKey, userName);
        _db.SaveChanges();
    }
}
