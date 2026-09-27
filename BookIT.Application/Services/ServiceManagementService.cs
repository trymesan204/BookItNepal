using BookIT.Application.Abstractions.Repository;
using BookIT.Application.Abstractions.Services;
using BookIT.Application.Abstractions.Context;
using BookIT.Application.DTOs.Service;
using BookIT.Domain.Entities;

namespace BookIT.Application.Services;

public sealed class ServiceManagementService(
    IServiceRepository serviceRepository,
    IOrganizationContext organizationContext) : IServiceManagementService
{
    public async Task<List<ServiceDTO>> GetServicesByOrganizationAsync(
        CancellationToken cancellationToken)
    {
        var services = await serviceRepository.GetByOrganizationIdAsync(
            GetOrganizationId(),
            cancellationToken);

        return services.Select(ToDTO).ToList();
    }

    public async Task<ServiceDTO?> GetServiceByOrganizationAndServiceIdAsync(
        long serviceId,
        CancellationToken cancellationToken)
    {
        var service = await serviceRepository.GetByOrganizationIdAndServiceIdAsync(
            GetOrganizationId(),
            serviceId,
            cancellationToken);

        return service is null ? null : ToDTO(service);
    }

    public async Task<List<PublicServiceDTO>> GetPublicServicesAsync(
        CancellationToken cancellationToken)
    {
        var services = await serviceRepository.GetPublicByOrganizationIdAsync(
            GetOrganizationId(),
            cancellationToken);

        return services
            .Select(service => new PublicServiceDTO(
                service.Id,
                service.Name,
                service.Price,
                service.DurationInMinutes))
            .ToList();
    }

    public async Task<ServiceDTO> CreateServiceAsync(
        ServiceDTO serviceDto,
        CancellationToken cancellationToken)
    {
        var service = new Service
        {
            Name = serviceDto.Name,
            Price = serviceDto.Price,
            DurationInMinutes = serviceDto.DurationInMinutes,
            OrganizationId = GetOrganizationId()
        };

        await serviceRepository.AddAsync(service, cancellationToken);
        await serviceRepository.SaveChangesAsync(cancellationToken);
        return ToDTO(service);
    }

    public async Task<bool> UpdateServiceAsync(
        long serviceId,
        ServiceDTO serviceDto,
        CancellationToken cancellationToken)
    {
        var service = await serviceRepository.GetByOrganizationIdAndServiceIdAsync(
            GetOrganizationId(),
            serviceId,
            cancellationToken);

        if (service is null)
        {
            return false;
        }

        service.Name = serviceDto.Name;
        service.Price = serviceDto.Price;
        service.DurationInMinutes = serviceDto.DurationInMinutes;

        serviceRepository.Update(service);
        await serviceRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteServiceByOrganizationAsync(
        long serviceId,
        CancellationToken cancellationToken)
    {
        var service = await serviceRepository.GetByOrganizationIdAndServiceIdAsync(
            GetOrganizationId(),
            serviceId,
            cancellationToken);

        if (service is null)
        {
            return false;
        }

        serviceRepository.Remove(service);
        await serviceRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private long GetOrganizationId()
    {
        return organizationContext.OrganizationId
            ?? throw new UnauthorizedAccessException("Organization was not resolved.");
    }

    private static ServiceDTO ToDTO(Service service)
    {
        return new ServiceDTO(
            service.Id,
            service.Name,
            service.Price,
            service.DurationInMinutes,
            service.OrganizationId);
    }
}
