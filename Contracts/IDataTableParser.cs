using Microsoft.AspNetCore.Http;
using Shared;

namespace Contracts;

public interface IDataTableParser
{
    public Task<ImportReport> ImportMenuAsync(Guid kitchenId, IFormFile file);
}