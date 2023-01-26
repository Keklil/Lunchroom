using MediatR;
using Domain.DataTransferObjects;
using Contracts;
using Domain.Exceptions;

namespace Application.Queries
{
    public sealed record GetOrdersReportQuery(DateTime Date, Guid GroupId) 
        : IRequest<List<OrderReportDto>>;
    
    internal class GetOrdersReportQueryHandler : IRequestHandler<GetOrdersReportQuery, List<OrderReportDto>>
    {
        private readonly IOrdersReportService _reportService;
        private readonly ILoggerManager _logger;

        public GetOrdersReportQueryHandler(IOrdersReportService reportService, ILoggerManager logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        public async Task<List<OrderReportDto>> Handle(GetOrdersReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _reportService.GenerateOrdersReport(request.Date, request.GroupId, false);
            if (report is null)
                _logger.LogWarning($"Для группы {request.GroupId} на дату {request.Date} отчета вернулся пустым");

            return report;
        }
    }
}
