using ClientV2.Apis;
using Microsoft.AspNetCore.Mvc;

namespace ClientV2.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrderController : ControllerBase
{
    private readonly IApiClientV2 _api;

    [HttpPost]
    public async Task<ActionResult> Delete(string orderId)
    {
        var orderIdGuid = Guid.Parse(orderId);

        await _api.Orders_DeleteOrderAsync(orderIdGuid);

        return RedirectToPage("/Index");
    }

    public OrderController(IApiClientV2 api)
    {
        _api = api;
    }
}