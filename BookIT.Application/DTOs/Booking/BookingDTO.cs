using BookIT.Domain.Enums;

namespace BookIT.Application.DTOs.Booking;

public sealed record BookingDTO(
	long Id,
	long CustomerId,
	long ServiceId,
	long StaffId,
	DateOnly BookingDate,
	int StartTimeMinutes,
	int EndTimeMinutes,
	BookingStatus Status,
	Guid PublicToken);
