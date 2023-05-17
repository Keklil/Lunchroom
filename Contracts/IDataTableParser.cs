using Microsoft.AspNetCore.Http;
using Shared;

namespace Contracts;

public interface IDataTableParser
{
    public Task<ImportReport> ParseMenuAsync(Guid kitchenId, IFormFile file);
}