using ClientV2.Apis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Orders;

public class History : PageModel
{
    private readonly IApiClientV2 _api;
    private readonly IHttpContextAccessor _accessor;

    public List<OrderDto> UserOrders { get; set; }
    
    public History(IApiClientV2 api, IHttpContextAccessor accessor)
    {
        _api = api;
        _accessor = accessor;
        UserOrders = new();
    }
    
    public async Task<ActionResult> OnGetAsync()
    {
        var userIdFromContext = _accessor.HttpContext.User.Claims.First(x => x.Type == "UserId").Value;
        var userId = Guid.Parse(userIdFromContext);
        var userOrders = await _api.Orders_GetOrdersByUserAsync(userId, new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        foreach (var order in userOrders.OrderByDescending(x=> x.Date))
        {
            var todayOrder = await _api.Orders_GetOrderAsync(order.Id);
            UserOrders.Add(todayOrder);
        }

        return Page();
    }

    public async Task<ActionResult> OnGetOldOrdersPartialAsync()
    {
        var userIdFromContext = _accessor.HttpContext.User.Claims.First(x => x.Type == "UserId").Value;
        var userId = Guid.Parse(userIdFromContext);
        var userOrders = await _api.Orders_GetOrdersByUserAsync(userId, new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        var todayOrders = await _api.Orders_GetTodayOrdersByUserAsync(userId, new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        var userOrderExceptToday = new List<OrderDto>();
        foreach (var order in userOrders.IntersectBy(todayOrders.Select(x => x.Id), x => x.Id).OrderByDescending(x=> x.Date))
        {
            var todayOrder = await _api.Orders_GetOrderAsync(order.Id);
            userOrderExceptToday.Add(todayOrder);
        }

        return Partial("_OldOrdersPartial", userOrderExceptToday);
    }
}