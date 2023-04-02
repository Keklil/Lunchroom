using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Services.OrdersReport;

[DisallowConcurrentExecution]
public class ReportingKitchenService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IServiceProvider _provider;

    private int hour, minute;
    private string kitchenEmail;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
            //TODO: Переписать под групповые меню
            await Task.Delay(30000, cancellationToken);
        //
        // var targetTime = DateTime.Today.AddHours(hour).AddMinutes(minute);
        // if (DateTime.Now < targetTime)
        // {
        //     _logger.LogInfo("Not time yet for reporting");
        //     continue;
        // }
        // using (var scope = _provider.CreateScope())
        // {
        //     var repository = scope.ServiceProvider.GetService<IRepositoryManager>();
        //
        //     var menu = await repository.Menu.GetMenuByDateAsync(DateTime.UtcNow.Date);
        //     if (menu is null)
        //         continue;
        //         
        //     if (menu.IsReported)
        //         continue;
        //         
        //
        //     var ordersReportService = scope.ServiceProvider.GetService<IOrdersReportService>();
        //
        //     _logger.LogInfo("Creating report for kitchen");            
        //     var ordersReport = await ordersReportService.GenerateOrdersReport(DateTime.UtcNow.Date, 
        //         true);
        //                
        //     if (ordersReport.Count == 0)
        //     {
        //         _logger.LogInfo("No orders for today");
        //         continue;
        //     }
        //                            
        //     var reportForKitchen = await ordersReportService.GenerateOrdersSummaryForKitchen(ordersReport);
        //                            
        //     var reportString = string.Empty;
        //     foreach (var item in reportForKitchen.OrderBy(x => x.Length))
        //     { 
        //         reportString += "<p>" + item + "</p>";
        //     }
        //
        //     var orderSum = ordersReport.Sum(x => x.Summary).ToString();
        //     
        //     reportString = reportString.Insert(reportString.Length,"<p>Итого – " + orderSum + "</p>");
        //
        //     reportString = reportString.Insert(0, "<!doctype html><html><body><p>Здравствуйте! Заказы для Сибирский тракт, 12, к.7, этаж 4</p>");
        //     reportString = reportString.Insert(reportString.Length, "</body></html>");
        //     _logger.LogInfo(reportString);                
        //     
        //     var mailService = scope.ServiceProvider.GetService<IMailSender>();
        //
        //     await mailService.SendEmailAsync(kitchenEmail, "Заказы обедов", reportString);
        //     await mailService.SendEmailAsync("world-20866@mail.ru", "Заказы обедов", reportString);
        //
        //
        //     _logger.LogInfo("Report send to kitchen");
        //     
        //     menu.Reported();
        //     
        //     repository.Menu.UpdateMenu(menu);
        //     await repository.SaveAsync();
        // }
    }

    public ReportingKitchenService(ILogger logger, IServiceProvider provider,
        IConfiguration configuration)
    {
        _logger = logger;
        _provider = provider;
        _configuration = configuration;

        kitchenEmail = _configuration.GetSection("MailServer:Sender").Value;
        hour = _configuration.GetValue<int>("TimeOrdersSendToKitchen:Hour");
        minute = _configuration.GetValue<int>("TimeOrdersSendToKitchen:Minute");
    }
}