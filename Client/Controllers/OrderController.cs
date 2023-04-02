using Client.Apis;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly IApiClient _api;

    [HttpPost]
    public async Task<ActionResult> Delete(string orderId)
    {
        var orderIdGuid = Guid.Parse(orderId);

        await _api.Orders_DeleteOrderAsync(orderIdGuid);

        return RedirectToPage("/Index");
    }

    public OrderController(IApiClient api)
    {
        _api = api;
    }
}