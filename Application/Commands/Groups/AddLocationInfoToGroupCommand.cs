using Contracts.Repositories;
using Domain.Models;
using MediatR;
using NetTopologySuite.Geometries;
using Nominatim.API.Geocoders;
using Nominatim.API.Interfaces;
using Nominatim.API.Models;
using Shared.DataTransferObjects.Group;

namespace Application.Commands.Groups;

public sealed record AddLocationInfoToGroupCommand(GroupConfigByAddressDto Config) : IRequest;

internal sealed class AddLocationInfoToGroupHandler : IRequestHandler<AddLocationInfoToGroupCommand>
{
    private readonly IRepositoryManager _repository;
    private readonly IForwardGeocoder _geocoder;

    public async Task Handle(AddLocationInfoToGroupCommand request, CancellationToken cancellationToken)
    {
        var groupEntity = await _repository.Groups.GetGroupAsync(request.Config.GroupId);

        var groupSettings = await CreateGroupSettingsFromDto(request.Config);

        groupEntity.SetSettings(groupSettings);
        _repository.Groups.UpdateGroup(groupEntity);
        await _repository.SaveAsync(cancellationToken);
    }

    private async Task<GroupSettings> CreateGroupSettingsFromDto(GroupConfigByAddressDto dto)
    {
        var streetAddress = !string.IsNullOrWhiteSpace(dto.Address.Street) ? dto.Address.Street + ", " : "";
        streetAddress += !string.IsNullOrWhiteSpace(dto.Address.Number) ? dto.Address.Number + ", " : "";
        streetAddress += !string.IsNullOrWhiteSpace(dto.Address.Office) ? dto.Address.Office : "";
        streetAddress = streetAddress.Trim().Trim(',');

        var geocodeRequest = new ForwardGeocodeRequest()
        {
            ShowGeoJSON = true,
            Country = dto.Address.Country,
            State = dto.Address.State,
            City = dto.Address.City,
            StreetAddress = streetAddress
        };
        
        var coordinates = await FetchCoordinatesByAddress(geocodeRequest);
        
        var address = !string.IsNullOrWhiteSpace(dto.Address.Country) ? dto.Address.Country + ", " : "";
        address += !string.IsNullOrWhiteSpace(dto.Address.State) ? dto.Address.State + ", " : "";
        address += !string.IsNullOrWhiteSpace(dto.Address.City) ? dto.Address.City + ", " : "";
        address += streetAddress;
        address = address.Trim().Trim(',');
        
        var groupSettings = new GroupSettings(dto.GroupId, 
            address,
            GeometryFactory.Default.CreatePoint(coordinates));
        
        return groupSettings;
    }

    private async Task<Coordinate> FetchCoordinatesByAddress(ForwardGeocodeRequest geocodeRequest)
    {
        var geoData = (await _geocoder.Geocode(geocodeRequest)).First();
        
        return new Coordinate(geoData.Latitude, geoData.Longitude);
    }

    public AddLocationInfoToGroupHandler(IRepositoryManager repository, IForwardGeocoder geocoder)
    {
        _repository = repository;
        _geocoder = geocoder;
    }
}