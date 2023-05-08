using NetTopologySuite.Geometries;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

namespace LunchRoom.Extensions;

public static class CustomOpenApiSchemas
{
    public static ObjectTypeMapper GetPolygonSchema()
    {
        return new ObjectTypeMapper(typeof(Polygon), new JsonSchema
        {
            Type = JsonObjectType.Object,
            Properties =
            {
                {
                    "type",
                    new JsonSchemaProperty { IsRequired = true, Type = JsonObjectType.String, Default = "Polygon" }
                },
                {
                    "coordinates", new JsonSchemaProperty
                    {
                        IsRequired = true,
                        Type = JsonObjectType.Array,
                        Item = new JsonSchemaProperty()
                        {
                            Type = JsonObjectType.Array,
                            MinItems = 3,
                            Example = new[] { new[] { 70m, 66m }, new[] { 70m, 46m }, new[] { 50m, 46m }, new[] { 50m, 66m }, new[] { 70m, 66m } },
                            Item = new JsonSchemaProperty()
                            {
                                Type = JsonObjectType.Array,
                                MinItems = 2,
                                Item = new JsonSchemaProperty()
                                {
                                    Type = JsonObjectType.Number,
                                }
                            }
                        }
                    }
                }
            }
        });
    }
    
    public static ObjectTypeMapper GetPointSchema()
    {
        return new ObjectTypeMapper(typeof(Point), new JsonSchema
        {
            Type = JsonObjectType.Object,
            Properties =
            {
                {
                    "type",
                    new JsonSchemaProperty { IsRequired = true, Type = JsonObjectType.String, Default = "Point" }
                },
                {
                    "coordinates", new JsonSchemaProperty
                    {
                        IsRequired = true,
                        Type = JsonObjectType.Array,
                        MinItems = 2,
                        Example = new[] { 30.1m, 10m },
                        Item = new JsonSchemaProperty()
                        {
                            Type = JsonObjectType.Number,
                        }
                    }
                }
            }
        });
    }
}