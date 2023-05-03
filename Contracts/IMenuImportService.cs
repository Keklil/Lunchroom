using Microsoft.AspNetCore.Http;
using Shared;

namespace Contracts;

public interface IMenuImportService
{
    public Task<ImportReport> ImportMenuAsync(Guid kitchenId, IFormFile file);
}