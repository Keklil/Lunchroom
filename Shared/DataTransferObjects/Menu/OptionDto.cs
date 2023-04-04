namespace Shared.DataTransferObjects.Menu;

public class OptionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public static class OptionMapper
{
    public static OptionDto Map(this Domain.Models.Option option)
    {
        return new OptionDto
        {
            Id = option.Id,
            Name = option.Name,
            Price = option.Price
        };
    }
}