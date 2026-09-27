using BookIT.Domain.Enums;

namespace BookIT.Application.DTOs.Staff;

public sealed record StaffDTO(
	long Id,
	string Name,
	string PhoneNumber,
	string Password,
	StaffType StaffType);

public sealed record PublicStaffDTO(long Id, string Name);
