using System.Text.Json;
using ClientV2.Apis;
using ClientV2.Pages.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Cart;

[Authorize]
public class Index : PageModel
{
    private readonly IApiClientV2 _api;
    private readonly IHttpContextAccessor _accessor;
    public MenuDto Menu { get; set; }
    public List<CartOrder> Cart { get; set; }
    public string TotalPrice { get; set; }

    public Index(IApiClientV2 api, IHttpContextAccessor accessor)
    {
        _api = api;
        _accessor = accessor;
    }
    
    public async Task<ActionResult> OnGetAsync()
    {
        var cart = HttpContext.Session.GetString("cart");
        Menu = await _api.Menu_GetTodayMenuAsync(new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        if (string.IsNullOrWhiteSpace(cart))
        {
            return Redirect("/Cart/Empty");
        }

        var cartEntity = JsonSerializer.Deserialize<List<OrderContainer>>(cart);

        var cartContainer = new List<CartOrder>();
        foreach (var item in cartEntity)
        {
            var order = new CartOrder() { Id = item.Id };
            if (item.LunchSetId != default)
            {
                order.LunchId = item.LunchSetId;
                order.LunchName = "Комбо набор";
                order.LunchPrice = Menu.LunchSets.Single(x => x.Id == item.LunchSetId).Price;
                order.LunchSetUnits = item.LunchSetUnits;
            }
            foreach (var option in item.OptionsIds)
            {
                var cartOption = new CartOrder.CartOption()
                {
                    Id = option,
                    Name = Menu.Options.SingleOrDefault(x => x.Id == option)?.Name ?? string.Empty,
                    Price = Menu.Options.SingleOrDefault(x => x.Id == option)?.Price ?? 0L,
                };
                order.Options.Add(cartOption);
            }
            cartContainer.Add(order);
        }
        
        if (cartContainer.Count == 0)
        {
            return Redirect("/Cart/Empty");
        }
        
        Cart = cartContainer;
        TotalPrice = Cart.Sum(x => x.TotalPrice).ToString();
        HttpContext.Session.SetString("totalPrice", TotalPrice);
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        return Redirect("/Orders/Confirm");
    }

    public async Task<ActionResult> OnPostDeleteAsync(Guid cartOrderId)
    {
        var cart = HttpContext.Session.GetString("cart");
        if (string.IsNullOrWhiteSpace(cart))
        {
            return Redirect("/Cart/Empty");
        }

        var cartEntity = JsonSerializer.Deserialize<List<OrderContainer>>(cart);
        cartEntity = cartEntity?.Where(x => x.Id != cartOrderId).ToList();
        var cartJson = JsonSerializer.Serialize(cartEntity);
        HttpContext.Session.SetString("cart", cartJson);
        return Redirect("/Cart");
    }
}

public class CartOrder
{
    public Guid Id { get; set; }
    public Guid LunchId { get; set; }
    public string LunchName { get; set; }
    public decimal LunchPrice { get; set; }
    public int LunchSetUnits { get; set; }
    public decimal TotalPrice => LunchPrice * LunchSetUnits + OptionsTotalPrice;
    public decimal OptionsTotalPrice => Options.Sum(x => x.Price);
    public List<CartOption> Options { get; set; } = new();
    
    public class CartOption
    {
       public Guid Id { get; set; }
       public string Name { get; set; }
       public decimal Price { get; set; }
    }

    
}