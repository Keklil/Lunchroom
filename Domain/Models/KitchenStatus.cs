namespace Domain.Models;

public enum KitchenStatus
{
    // Новая столовая, подлежит проверке по юридическим данным и контактам
    New,
    
    // Столовая, прошедшая проверку по юридическим данным и контактам
    Approved
}