using BookIT.Application.DTOs.Service;

namespace BookIT.Application.Abstractions.Services;

public interface IServiceManagementService
{
    Task<List<ServiceDTO>> GetServicesByOrganizationAsync(
        CancellationToken cancellationToken);

    Task<ServiceDTO?> GetServiceByOrganizationAndServiceIdAsync(
        long serviceId,
        CancellationToken cancellationToken);

    Task<List<PublicServiceDTO>> GetPublicServicesAsync(
        CancellationToken cancellationToken);

    Task<ServiceDTO> CreateServiceAsync(
        ServiceDTO serviceDto,
        CancellationToken cancellationToken);

    Task<bool> UpdateServiceAsync(
        long serviceId,
        ServiceDTO serviceDto,
        CancellationToken cancellationToken);

    Task<bool> DeleteServiceByOrganizationAsync(
        long serviceId,
        CancellationToken cancellationToken);
}
