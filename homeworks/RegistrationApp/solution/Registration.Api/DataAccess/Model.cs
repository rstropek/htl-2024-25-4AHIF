namespace Registration.Api.DataAccess;

using System.Collections.ObjectModel;

public enum CampaignStatus
{
    Active,
    Inactive
}

public class Campaign
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Organizer { get; set; }
    public Collection<CampaignDate> Dates { get; set; } = [];
    public decimal? ReservedRatioForGirls { get; set; }
    public DateOnly? PurgeDate { get; set; }
    public CampaignStatus Status { get; set; } = CampaignStatus.Inactive;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? UpdatedBy { get; set; }
}

public enum CampaignDateStatus
{
    Active,
    Inactive,
    Hidden
}

public class CampaignDate
{
    public required DateOnly Date { get; set; }
    public Collection<DepartmentAssignment> DepartmentAssignments { get; set; } = [];
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public CampaignDateStatus Status { get; set; } = CampaignDateStatus.Hidden;
}

public class DepartmentAssignment
{
    public required string DepartmentName { get; set; }
    public required short NumberOfSeats { get; set; }
    public decimal? ReservedRatioForGirls { get; set; }
    public Collection<Registration> Registrations { get; set; } = [];
}

public enum Gender
{
    Male,
    Female,
    Other
}

public enum RegistrationStatus
{
    Confirmed,
    Pending
}

public class Registration
{
    public required string Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? SecondaryEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? SecondaryPhoneNumber { get; set; }
    public required string CurrentSchool { get; set; }
    public required byte YearsOfSchooling { get; set; }
    public required Gender Gender { get; set; }
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? UpdatedBy { get; set; }
}
