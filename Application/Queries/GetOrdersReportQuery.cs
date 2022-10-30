using MediatR;
using Entities.DataTransferObjects;
using Contracts;
using Entities.Exceptions;

namespace Application.Queries
{
    public sealed record GetOrdersReportQuery(DateTime date) 
        : IRequest<List<OrderReportDto>>;
    
    internal class GetOrdersReportQueryHandler : IRequestHandler<GetOrdersReportQuery, List<OrderReportDto>>
    {
        private readonly IOrdersReportService _reportService;

        public GetOrdersReportQueryHandler(IOrdersReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<List<OrderReportDto>> Handle(GetOrdersReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _reportService.GenerateOrdersReport(request.date, false);
            if (report is null)
                throw new NotFoundException("The report requested fate does not exist");

            return report;
        }
    }
}
