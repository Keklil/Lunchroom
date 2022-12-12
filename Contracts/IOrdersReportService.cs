using Domain.DataTransferObjects;

namespace Contracts;

public interface IOrdersReportService
{
    Task<List<OrderReportDto>> GenerateOrdersReport(DateTime date, bool excludeWithoutConfirmedPayment);
    Task<List<string>> GenerateOrdersSummaryForKitchen(List<OrderReportDto> listOrders);
}