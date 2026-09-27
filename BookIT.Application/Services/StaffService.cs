using BookIT.Application.Abstractions.Repository;
using BookIT.Application.Abstractions.Services;
using BookIT.Application.Abstractions.Context;
using BookIT.Application.DTOs.Staff;
using BookIT.Domain.Entities;

namespace BookIT.Application.Services;

public sealed class StaffService(
    IStaffRepository staffRepository,
    IPasswordHasher passwordHasher,
    IOrganizationContext organizationContext) : IStaffService
{
    public async Task<List<StaffDTO>> GetStaffByOrganizationAsync(CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByOrganizationIdAsync(GetOrganizationId(), cancellationToken);
        return staff.Select(ToDTO).ToList();
    }

    public async Task<StaffDTO?> GetStaffByOrganizationAndStaffIdAsync(
        long staffId,
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByOrganizationIdAndStaffIdAsync(GetOrganizationId(), staffId, cancellationToken);
        return staff is null ? null : ToDTO(staff);
    }

    public async Task<List<PublicStaffDTO>> GetPublicStaffAsync(
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByOrganizationIdAsync(GetOrganizationId(), cancellationToken);
        return staff.Select(staffMember => new PublicStaffDTO(staffMember.Id, staffMember.Name)).ToList();
    }

    public async Task<StaffDTO> CreateStaffAsync(
        StaffDTO staffDto,
        CancellationToken cancellationToken)
    {
        var staff = new Staff
        {
            Name = staffDto.Name,
            PhoneNumber = staffDto.PhoneNumber,
            PasswordHash = passwordHasher.Hash(staffDto.Password),
            StaffType = staffDto.StaffType,
            OrganizationId = GetOrganizationId()
        };

        await staffRepository.AddAsync(staff, cancellationToken);
        await staffRepository.SaveChangesAsync(cancellationToken);
        return ToDTO(staff);
    }

    public async Task<bool> UpdateStaffAsync(
        long staffId,
        StaffDTO staffDto,
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByOrganizationIdAndStaffIdAsync(GetOrganizationId(), staffId, cancellationToken);
        if (staff is null)
        {
            return false;
        }

        staff.Name = staffDto.Name;
        staff.PhoneNumber = staffDto.PhoneNumber;
        staff.StaffType = staffDto.StaffType;

        if (!string.IsNullOrWhiteSpace(staffDto.Password))
        {
            staff.PasswordHash = passwordHasher.Hash(staffDto.Password);
        }

        staffRepository.Update(staff);
        await staffRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteStaffByOrganizationAsync(
        long staffId,
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByOrganizationIdAndStaffIdAsync(GetOrganizationId(), staffId, cancellationToken);
        if (staff is null)
        {
            return false;
        }

        staffRepository.Remove(staff);
        await staffRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private long GetOrganizationId()
    {
        return organizationContext.OrganizationId
            ?? throw new UnauthorizedAccessException("Organization was not resolved.");
    }

    private static StaffDTO ToDTO(Staff staff)
    {
        return new StaffDTO(
            staff.Id,
            staff.Name,
            staff.PhoneNumber,
            string.Empty,
            staff.StaffType);
    }
}
