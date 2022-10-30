using Entities.DataTransferObjects;


namespace Contracts
{
    public interface IMailParser
    {
        List<string> NormalizeMenu(string mailRaw);
        MenuForCreationDto ConvertMenu(List<string> menuRaw);
    }
}
