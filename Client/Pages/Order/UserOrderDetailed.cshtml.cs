using Client.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Order;

public class UserOrderDetailed : PageModel
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IApiClient _api;

    public OrderDto Order { get; set; }
    public decimal OrderSummary { get; set; }

    public async Task<ActionResult> OnGetAsync(string orderId)
    {
        var orderIdGuid = Guid.Parse(orderId);
        Order = await _api.Orders_GetOrderAsync(orderIdGuid);

        OrderSummary += Order.LunchSet.Price;
        foreach (var option in Order.Options) OrderSummary += option.Option.Price * option.OptionUnits;

        return Page();
    }

    public UserOrderDetailed(IApiClient api, IHttpContextAccessor accessor)
    {
        _api = api;
        _accessor = accessor;
    }
}