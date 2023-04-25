﻿using Domain.Exceptions;

namespace Domain.Models;

public class Kitchen
{
    public Guid Id { get; }
    public string OrganizationName { get; private set; }

    public string Address { get; private set; }
    public string Inn { get; private set; }
    public Contacts Contacts { get; private set; }
    
    // Флаг, указывающий, что столовая столовая доступна для заказов
    public bool AllowForOrders => IsActive && IsApproved;
    // Флаг, указывающий, что столовая разрешила принимать заказы
    public bool IsActive { get; private set; }
    // Флаг, указывающий, что столовая прошла проверку по юридическим данным и контактам
    public bool IsApproved { get; private set; }
    public IReadOnlyCollection<User> Managers => _managers;
    private List<User> _managers = new();
    
    public KitchenSettings? Settings { get; private set; }

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
    
    public void EditSettings(KitchenSettings settings)
    {
        Settings = settings;
    }
    
    public void AddManager(User manager)
    {
        _managers.Add(manager);
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
        
        Id = Guid.NewGuid();
        OrganizationName = organizationName;
        Address = address;
        Inn = inn;
        Settings = null;
    }
    
    private Kitchen(){}
}