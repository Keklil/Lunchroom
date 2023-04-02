using ClientV2.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientV2.Pages.Report;

[Authorize]
public class Orders : PageModel
{
    private readonly IApiClientV2 _api;

    [BindProperty] public DateTime DateSearch { get; } = DateTime.Today;

    public void OnGet()
    {
    }

    public async Task<PartialViewResult> OnGetOrderDetailsAsync()
    {
        var orders =
            await _api.Orders_GetOrdersReportByDayAsync(DateTime.Today,
                new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        return Partial("_OrderDetails", orders);
    }

    public async Task<PartialViewResult> OnPostOrderDetailsAsync(DateTime DateSearch)
    {
        var orders =
            await _api.Orders_GetOrdersReportByDayAsync(DateSearch, new Guid("2b974f1e-618d-4aef-962e-713d1db8d2c6"));
        if (orders.Count == 0) return Partial("_OrderDetails", new List<OrderReportDto>());
        return Partial("_OrderDetails", orders);
    }

    public Orders(IApiClientV2 api)
    {
        _api = api;
    }
}