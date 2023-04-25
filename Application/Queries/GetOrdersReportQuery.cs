using Contracts;
using Shared.DataTransferObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries;

public sealed record GetOrdersReportQuery(DateTime Date, Guid GroupId)
    : IRequest<List<OrderReportDto>>;

internal class GetOrdersReportQueryHandler : IRequestHandler<GetOrdersReportQuery, List<OrderReportDto>>
{
    private readonly ILogger<GetOrdersReportQueryHandler> _logger;
    private readonly IOrdersReportService _reportService;

    public async Task<List<OrderReportDto>> Handle(GetOrdersReportQuery request, CancellationToken cancellationToken)
    {
        var report = await _reportService.GenerateOrdersReport(request.Date, request.GroupId, false);

        return report;
    }

    public GetOrdersReportQueryHandler(IOrdersReportService reportService, ILogger<GetOrdersReportQueryHandler> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }
}