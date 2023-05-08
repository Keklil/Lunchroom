using Domain.Exceptions;
using Domain.Models.Base;

namespace Domain.Models;

public class Kitchen : Entity
{
    public string OrganizationName { get; private set; }
    public string Address { get; private set; }
    public string Inn { get; private set; }
    public Contacts Contacts { get; private set; }
    public KitchenSettings? Settings { get; private set; }
    
    // Флаг, указывающий, что столовая столовая доступна для заказов
    public bool AllowForOrders => IsActive && IsApproved;
    // Флаг, указывающий, что столовая разрешила принимать заказы
    public bool IsActive { get; private set; }
    // Флаг, указывающий, что столовая прошла проверку по юридическим данным и контактам
    public bool IsApproved { get; private set; }
    
    public IReadOnlyCollection<User> Managers => _managers;
    private readonly List<User> _managers = new();

    public void ChangeName(string organizationName)
    {
        OrganizationName = organizationName;
    }
    
    public void ChangeAddress(string address)
    {
        Address = address;
    }
    
    public void EditContacts(string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Хотя бы один из контактов должнен быть заполнен");
        
        Contacts = new Contacts(email, phone);
    }
    
    public void ChangeSettings(KitchenSettings settings)
    {
        Settings = settings;
    }
    
    public void AddManager(User manager)
    {
        _managers.Add(manager);
    }

    public void VerifyKitchen()
    {
        IsApproved = true;
    }
    
    public void ActivateKitchen()
    {
        IsActive = true;
    }
    
    public Kitchen(string organizationName, string address, string inn, string? email, string? phone)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Хотя бы один из контактов должнен быть заполнен");
        
        if (string.IsNullOrWhiteSpace(organizationName))
            throw new DomainException("Не заполнено название организации");
        
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Не заполнен адрес");
        
        Contacts = new Contacts(email, phone);
        
        OrganizationName = organizationName;
        Address = address;
        Inn = inn;
        Settings = null;
    }
    
    private Kitchen(){}
}