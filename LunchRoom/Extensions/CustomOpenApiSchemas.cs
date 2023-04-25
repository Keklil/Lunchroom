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
                            Item = new JsonSchemaProperty()
                            {
                                Type = JsonObjectType.Array,
                                MinItems = 2,
                                Example = new[] { 25.1m, 60m },
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