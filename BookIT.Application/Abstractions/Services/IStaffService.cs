using BookIT.Application.DTOs.Staff;

namespace BookIT.Application.Abstractions.Services;

public interface IStaffService
{
    Task<List<StaffDTO>> GetStaffByOrganizationAsync(
        CancellationToken cancellationToken);

    Task<StaffDTO?> GetStaffByOrganizationAndStaffIdAsync(
        long staffId,
        CancellationToken cancellationToken);

    Task<List<PublicStaffDTO>> GetPublicStaffAsync(
        CancellationToken cancellationToken);

    Task<StaffDTO> CreateStaffAsync(
        StaffDTO staffDto,
        CancellationToken cancellationToken);

    Task<bool> UpdateStaffAsync(
        long staffId,
        StaffDTO staffDto,
        CancellationToken cancellationToken);

    Task<bool> DeleteStaffByOrganizationAsync(
        long staffId,
        CancellationToken cancellationToken);
}
