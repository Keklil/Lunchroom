using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientV2.Helpers;

public static class EnumHelper
{
    public static SelectList ToEnumDescriptionsList<TEnum>(this TEnum value)
    {
        var values = Enum
            .GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(x => new
            {
                Id = x,
                Name = x?.ToString()
            });
        return new SelectList(values, "Id", "Name");
    }
}