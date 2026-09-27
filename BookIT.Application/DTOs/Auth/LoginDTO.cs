namespace BookIT.Application.DTOs.Auth;

public sealed record LoginDTO(string PhoneNumber, string Password);

public sealed record LoginResponseDTO(
	string Token,
	long UserId,
	long OrganizationId,
	string StaffType);
