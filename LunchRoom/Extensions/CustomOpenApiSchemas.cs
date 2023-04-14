namespace LunchRoom.Extensions;

public class CustomOpenApiSchemas
{
    // private static OpenApiSchema GetPolygonSchema()
    // {
    //     return new OpenApiSchema
    //     {
    //         Type = "object",
    //         Required = new HashSet<string> { "type", "coordinates" },
    //         Properties = new Dictionary<string, OpenApiSchema>
    //         {
    //             ["type"] = new() { Type = "string", Enum = { new OpenApiString("Polygon") } },
    //             ["coordinates"] = new()
    //             {
    //                 Type = "array",
    //                 Items = new OpenApiSchema
    //                 {
    //                     Type = "array",
    //                     MinItems = 4,
    //                     Items = new OpenApiSchema
    //                     {
    //                         Type = "array",
    //                         MinItems = 2,
    //                         Items = new OpenApiSchema { Type = "number" }
    //                     }
    //                 }
    //             }
    //         }
    //     };
    // }
}