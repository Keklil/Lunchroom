using Shared.DataTransferObjects;

namespace Contracts;

public interface IOrdersReportService
{
    Task<List<OrderReportDto>> GenerateOrdersReport(DateTime date, Guid groupId, bool excludeWithoutConfirmedPayment);
    Task<List<string>> GenerateOrdersSummaryForKitchen(List<OrderReportDto> listOrders);
}