namespace BookIT.Application.DTOs.Service;

public sealed record ServiceDTO(
	long Id,
	string Name,
	decimal Price,
	int DurationInMinutes,
	long OrganizationId);

public sealed record PublicServiceDTO(
	long Id,
	string Name,
	decimal Price,
	int DurationInMinutes);
