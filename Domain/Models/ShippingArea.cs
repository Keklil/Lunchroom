using Domain.Models.Base;
using NetTopologySuite.Geometries;

namespace Domain.Models;

public class ShippingArea : ValueObject
{
    public Polygon Polygon { get; }

    public ShippingArea(Polygon polygon)
    {
        Polygon = polygon;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
    
    private ShippingArea(){}
}