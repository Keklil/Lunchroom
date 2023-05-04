using Shared.DataTransferObjects.Menu;

namespace Contracts;

public interface IPlainTextParser
{
    List<string> NormalizeMenu(string mailRaw);
    MenuForCreationDto ConvertMenu(List<string> menuRaw);
}