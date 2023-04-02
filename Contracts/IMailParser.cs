using Domain.DataTransferObjects.Menu;

namespace Contracts;

public interface IMailParser
{
    List<string> NormalizeMenu(string mailRaw);
    MenuForCreationDto ConvertMenu(List<string> menuRaw);
}