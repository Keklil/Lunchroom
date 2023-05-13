using Contracts;
using Contracts.Repositories;
using Domain.Exceptions;
using Shared.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Shared.DataTransferObjects.Order;

namespace Services.OrdersReport;

public class OrdersReportService : IOrdersReportService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<OrdersReportService> _logger;
    
    public async Task<List<OrderReportDto>> GenerateOrdersReport(DateTime date, Guid groupId,
        bool excludeWithoutConfirmedPayment)
    {
        var orders = await _repository.Order.GetOrdersByDateAsync(date, groupId);
        if (excludeWithoutConfirmedPayment)
            orders = orders.Where(x => x.Payment).ToList();

        var listUserIds = orders.AsParallel().Select(x => x.CustomerId).ToList();
        var users = await _repository.User.GetListUsersByIds(listUserIds);
        var menu = await GetMenu(date, groupId);

        var orderReport = new List<OrderReportDto>();

        if (menu is null)
            return orderReport;

        // var result = orders
        //     .Join(users,
        //         o => o.CustomerId,
        //         u => u.Id,
        //         (o, u) => new { o, u })
        //     .Join(menu.LunchSets,
        //         lo => lo.o.LunchSet.Id,
        //         ls => ls.Id,
        //         (lo, ls) => new { lo.o, lo.u, ls })
        //     .OrderBy(c => c.ls.Price);
        //
        //
        // foreach (var order in result)
        // {
        //     var orderOptions = order.o.Options
        //         .Join(menu.Options,
        //             ord => ord.OptionId,
        //             o => o.Id,
        //             (ord, o) => new { ord, o })
        //         .OrderBy(x => x.o.Price)
        //         .ToList();
        //
        //     var optionsPrice = 0m;
        //     var optionsPriceToString = string.Empty;
        //     foreach (var option in orderOptions)
        //     {
        //         optionsPrice += option.o.Price;
        //         if (menu.Options.Count(x => x.Price == option.o.Price) > 1)
        //             optionsPriceToString += "+" + option.o.Price + $"({option.o.Name})"; //
        //         else
        //             optionsPriceToString += "+" + option.o.Price;
        //     }
        //
        //     var row = new OrderReportDto
        //     {
        //         LunchSetUnits = order.o.LunchSetUnits,
        //         LunchSetPrice = order.ls.Price.ToString(),
        //         OptionsPrice = optionsPriceToString.TrimStart('+'),
        //         Summary = order.o.Payment ? order.ls.Price + optionsPrice : 0,
        //         UserName = order.u.Surname + " " + order.u.Name,
        //         Payment = order.o.Payment
        //     };
        //
        //     orderReport.Add(row);
        // }

        return orderReport;
    }

    public async Task<List<string>> GenerateOrdersSummaryForKitchen(List<OrderReportDto> listOrders)
    {
        List<string> reportForSend = new();

        var dict = new Dictionary<string, OrderSummaryDto>();
        foreach (var row in listOrders)
        {
            var rowString = row.LunchSetPrice;
            if (row.OptionsPrice is not null && row.OptionsPrice.Length > 0)
                rowString += "+" + row.OptionsPrice;

            if (!dict.ContainsKey(rowString))
            {
                var orderSummary = new OrderSummaryDto { Quantity = 1, TotalPrice = row.Summary };
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
            reportForSend.Add(tempString);
        }

        return reportForSend;
    }

    private async Task<Domain.Models.Menu?> GetMenu(DateTime date, Guid groupId)
    {
        try
        {
            var menu = await _repository.Menu.GetMenuByDateAsync(date, groupId);

            return menu;
        }
        catch (NotFoundException e)
        {
            _logger.LogInformation(e, "");
            return null;
        }
    }

    public OrdersReportService(IRepositoryManager repository, ILogger<OrdersReportService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}