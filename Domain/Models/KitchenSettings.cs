using Domain.Exceptions;
using Domain.Models.Enums;
using NetTopologySuite.Geometries;

namespace Domain.Models;

public class KitchenSettings
{
    public Guid Id { get; }
    public Guid KitchenId { get; }
    public TimeSpan LimitingTimeForOrder { get; private set; }
    public MenuUpdatePeriod MenuUpdatePeriod { get; private set; }
    public MenuFormat MenuFormat { get; private set; }
    public IReadOnlyList<ShippingArea> ShippingAreas => _shippingAreas;
    private List<ShippingArea> _shippingAreas;

    public void EditShippingAreas(List<Polygon> polygons)
    {
        if (polygons.Count >= 3) 
            throw new DomainException("Превышено допустимое количество зон доставки");

        _shippingAreas = new List<ShippingArea>();
        foreach (var polygon in polygons)
            _shippingAreas.Add(new ShippingArea(polygon));
    }

    public void EditLimitingTimeForOrder(TimeSpan limitingTimeForOrder)
    {
        LimitingTimeForOrder = limitingTimeForOrder;
    }
    
    public void ChangeMenuUpdatePeriod(MenuUpdatePeriod menuUpdatePeriod)
    {
        MenuUpdatePeriod = menuUpdatePeriod;
    }
    
    public void ChangeMenuFormat(MenuFormat menuFormat)
    {
        MenuFormat = menuFormat;
    }

    public KitchenSettings(
        Guid kitchenId,
        TimeSpan limitingTimeForOrder,
        MenuUpdatePeriod menuUpdatePeriod, 
        MenuFormat menuFormat)
    {
        Id = Guid.NewGuid();
        KitchenId = kitchenId;
        LimitingTimeForOrder = limitingTimeForOrder;
        MenuUpdatePeriod = menuUpdatePeriod;
        MenuFormat = menuFormat;
        _shippingAreas = new ();
    }

    private KitchenSettings(){}
}