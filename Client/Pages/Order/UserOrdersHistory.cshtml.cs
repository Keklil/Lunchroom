using Client.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Order;

[Authorize]
public class UserOrdersHistory : PageModel
{
    private readonly IApiClient _api;
    private readonly IHttpContextAccessor _accessor;
    
    public List<OrdersForUser> UserOrders { get; set; }
    
    [BindProperty]
    public string OrderId { get; set; }
    
    public UserOrdersHistory(IApiClient api, IHttpContextAccessor accessor)
    {
        _api = api;
        _accessor = accessor;
    }
    public async Task<ActionResult> OnGet()
    {
        var userIdFromContext = _accessor.HttpContext.User.Claims.First(x => x.Type == "UserId").Value;
        var userId = Guid.Parse(userIdFromContext);
        UserOrders = await _api.Orders_GetOrdersByUserAsync(userId) as List<OrdersForUser>;
    
        return Page();
    }

    public async Task<ActionResult> OnPostAsync()
    {
        var id = OrderId;
        return Redirect($"/Order/UserOrderDetailed?orderId={id}");
    }
}