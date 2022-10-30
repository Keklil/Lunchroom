using System.Text.Json;
using Client.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Order;

[Authorize]
public class SubmitOrder : PageModel
{
    private readonly IApiClient _api;
    private readonly IConfiguration _configuration;

    private int hour, minute;

    public string paymentLink = "https://www.tinkoff.ru/cf/7fWSZf7TQ0i";
    
    public string? TotalPrice { get; set; }
    public OrderDto Order { get; set; }

    public SubmitOrder(IApiClient api, IConfiguration configuration)
    {
        _api = api;
        _configuration = configuration;

        hour = _configuration.GetValue<int>("TimeMenuExpiration:Hour");
        minute = _configuration.GetValue<int>("TimeMenuExpiration:Minute");
    }
    
    public async Task<ActionResult> OnGetAsync(string? orderId)
    {
        if (DateTime.Now.TimeOfDay > new TimeSpan(hour, minute, 0))
            return RedirectToPage("/Order/MenuExpired");
        
        var orderIdGuid = Guid.Empty;
        var orderJson = HttpContext.Session.GetString("OrderJson");

        if (orderJson is null)
        {
            Guid.TryParse(orderId, out orderIdGuid);
            
            if (orderIdGuid.Equals(Guid.Empty))
            {
                return RedirectToPage("/Index");
            }

            try
            {
                var order = await _api.Orders_GetOrderAsync(orderIdGuid);
                orderJson = JsonSerializer.Serialize(order);
                TotalPrice = (order.LunchSet.Price + order.Options.Sum(x => x.Option.Price)).ToString();
            }
            catch (Exception e)
            {
                return RedirectToPage("/Index");
            }
            
            HttpContext.Session.SetString("OrderJson", orderJson);
        }

        if (TotalPrice is null)
            TotalPrice = HttpContext.Session.GetString("TotalPrice");
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (DateTime.Now.TimeOfDay > new TimeSpan(hour, minute, 0))
            return RedirectToPage("/Order/MenuExpired");
        
        var orderJson = HttpContext.Session.GetString("OrderJson");
        var order = JsonSerializer.Deserialize<OrderDto>(orderJson);

        await _api.Orders_ConfirmPaymentAsync(order.Id);
        
        HttpContext.Session.Remove("OrderJson");
        return RedirectToPage("/Index");
    }
}