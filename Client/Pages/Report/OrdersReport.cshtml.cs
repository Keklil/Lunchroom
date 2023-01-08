using Client.Apis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Report;

[Authorize(Roles = "Admin")]
public class OrdersReport : PageModel
{
    private readonly IApiClient _api;
    public List<OrderReportDto>? ReportDto { get; set; }
    public List<string> ReportForSend { get; set; } = new();
    public string dateView;
    public decimal OrdersSummaryPrice { get; set; }

    public OrdersReport(IApiClient api)
    {
        _api = api;
    }
    
    public async Task<IActionResult> OnGetAsync(string date)
    {
        var dateSearch = DateTime.Parse(date);
        dateView = dateSearch.ToString("dddd, dd MMMM");
        ReportDto = await _api.Orders_GetOrdersReportByDayAsync(dateSearch) as List<OrderReportDto?>;

        var dict = new Dictionary<string, OrderSummary>();
        var reportWithSubmittedPayment = ReportDto.Where(x => x.Payment).ToList();
        OrdersSummaryPrice = reportWithSubmittedPayment.Sum(x => x.Summary);
        foreach (var row in reportWithSubmittedPayment)
        {
            var rowString = row.LunchSetPrice;
            if (row.OptionsPrice is not null && row.OptionsPrice.Length > 0)
                rowString += "+" + row.OptionsPrice;
            
            if (!dict.ContainsKey(rowString))
            {
                var orderSummary = new OrderSummary() { Quantity = 1, TotalPrice = row.Summary };
                dict.Add(rowString, orderSummary);
            }
            else
            {
                dict[rowString].Quantity++;
            }
        }
        
        foreach (var row in dict)
        {
            var tempString = row.Value.Quantity + " x " + row.Key + " – " + row.Value.TotalPrice * row.Value.Quantity;
            ReportForSend.Add(tempString);
        }
        
        return Page();
    }
}

public class OrderSummary
{
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
};