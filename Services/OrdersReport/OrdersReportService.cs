using System.ComponentModel;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Services.OrdersReport
{
    public class OrdersReportService : IOrdersReportService
    {
        private readonly IRepositoryManager _repository;
        private IMemoryCache _cache;
        private MemoryCacheEntryOptions cacheOptions;

        public OrdersReportService(IRepositoryManager repository)
        {
            _repository = repository;

            cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(5)
            };
        }
        
        public async Task<List<OrderReportDto>> GenerateOrdersReport(DateTime date, bool excludeWithoutConfirmedPayment)
        {
            var orders = await _repository.Order.GetOrdersByDateAsync(date);
            if (excludeWithoutConfirmedPayment)
                orders = orders.Where(x => x.Payment).ToList();
            
            var listUserIds = orders.AsParallel().Select(x => x.CustomerId).ToList();
            var users = await _repository.User.GetListUsersByIds(listUserIds);
            var menu = await GetMenu(date);

            var orderReport = new List<OrderReportDto>();

            if (menu is null)
                return orderReport;

            var result = orders
                .Join(users,
                    o => o.CustomerId,
                    u => u.Id,
                    (o, u) => new { o, u })
                .Join(menu.LunchSets,
                    lo => lo.o.LunchSetId,
                    ls => ls.Id,
                    (lo, ls) => new { o = lo.o, u = lo.u, ls })
                .OrderBy(c => c.ls.Price);
            
            
            
            foreach (var order in result)
            {
                var orderOptions = order.o.Options
                    .Join(menu.Options,
                        ord => ord.OptionId,
                        o => o.Id,
                        (ord, o) => new {ord, o})
                    .OrderBy(x => x.o.Price)
                    .ToList();
                
                var optionsPrice = 0m;
                var optionsPriceToString = string.Empty;
                foreach (var option in orderOptions)
                {
                    optionsPrice += option.o.Price;
                    if (menu.Options.Where(x => x.Price == option.o.Price).Count() > 1)
                        optionsPriceToString += "+" + option.o.Price.ToString()+$"({option.o.Name})";//
                    else
                        optionsPriceToString += "+" + option.o.Price.ToString();
                }
                
                var row = new OrderReportDto()
                {
                    
                    LunchSetPrice = order.ls.Price.ToString(),
                    OptionsPrice = optionsPriceToString.TrimStart('+'),
                    Summary = order.o.Payment ? order.ls.Price + optionsPrice : 0,
                    UserName = order.u.Surname + " " + order.u.Name,
                    Payment = order.o.Payment
                };
                
                orderReport.Add(row);
            }

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
                    var orderSummary = new OrderSummaryDto() { Quantity = 1, TotalPrice = row.Summary };
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
        
        public async Task<Menu> GetMenu(DateTime date)
        {
            var menu = await _repository.Menu.GetMenuByDateAsync(date);

            return menu;
        }
    }
}
