﻿namespace Shared.DataTransferObjects.Order;

public record OrderOptionForCreationDto
{
    public Guid Id { get; init; }
    public int Quantity { get; init; }
}