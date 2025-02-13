using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Registration.Api.CrossCuttingConcerns;
using Registration.Api.DataAccess;

namespace Registration.Api.Apis.Admin;

public static class CampaignUpdateApi
{
    public static async Task<IResult> UpdateCampaign([FromBody] Campaign dto, [FromRoute] Guid campaignId, IJsonFileRepository repository, IOptions<ErrorHandlingOptions> settings)
    {
        using var campaignStream = await repository.Open(campaignId, true);
        if (campaignStream is null) { return Results.NotFound(); }

        var campaign = await repository.Get<DataAccess.Campaign>(campaignStream)
            ?? throw new InvalidOperationException($"Could not read campaign with id {campaignId} from repository");

        if (campaign.UpdatedAt != dto.UpdatedAt)
        {
            return Results.Problem(new ProblemDetails
            {
                Type = $"{settings.Value.ProblemDetailsUriPrefix}/conflict",
                Title = "Optimistic concurrency control failed, campaign has been updated by another user",
                Status = StatusCodes.Status409Conflict,
                Instance = campaignId.ToString()
            });
        }

        if (campaign.Status == CampaignStatus.Inactive && dto.Status == CampaignStatus.Active)
        {
            return Results.Problem(new ProblemDetails
            {
                Type = $"{settings.Value.ProblemDetailsUriPrefix}/forbidden",
                Title = "Campaigns cannot be activated using this endpoint. Use the ActivateCampaign endpoint instead.",
                Status = StatusCodes.Status403Forbidden,
                Instance = campaignId.ToString()
            });
        }

        var updateProblem = UpdateCampaignFromDto(dto, campaign);
        if (updateProblem is not null)
        {
            updateProblem.Type = $"{settings.Value.ProblemDetailsUriPrefix}/forbidden";
            updateProblem.Status = StatusCodes.Status403Forbidden;
            return Results.Problem(updateProblem);
        }

        campaign.UpdatedAt = DateTimeOffset.UtcNow;
        campaign.UpdatedBy = "Admin API"; // TODO: Set the current user here (not implemented in this exammple)

        await repository.Update(campaignStream, campaign);

        return Results.Ok(campaign);
    }

    internal static void UpdateDepartmentAssignment(DepartmentAssignment dto, DataAccess.DepartmentAssignment assignment)
    {
        assignment.DepartmentName = dto.DepartmentName;
        assignment.NumberOfSeats = dto.NumberOfSeats;
        assignment.ReservedRatioForGirls = dto.ReservedRatioForGirls;
    }

    internal static ProblemDetails? MergeDepartmentAssignments(IEnumerable<DepartmentAssignment>? dto, List<DataAccess.DepartmentAssignment> assignments)
    {
        var deletedAssignments = assignments.Where(a => !(dto?.Any(b => b.DepartmentName == a.DepartmentName) ?? false)).ToArray();
        foreach (var deletedAssignment in deletedAssignments)
        {
            if (deletedAssignment.Registrations.Any())
            {
                return new ProblemDetails
                {
                    Title = "Cannot delete department with registrations",
                    Instance = deletedAssignment.DepartmentName
                };
            }

            assignments.Remove(deletedAssignment);
        }

        foreach (var assignment in dto ?? [])
        {
            var existingAssignment = assignments.FirstOrDefault(a => a.DepartmentName == assignment.DepartmentName);
            if (existingAssignment is null)
            {
                assignments.Add(new DataAccess.DepartmentAssignment
                {
                    DepartmentName = assignment.DepartmentName,
                    NumberOfSeats = assignment.NumberOfSeats,
                    ReservedRatioForGirls = assignment.ReservedRatioForGirls
                });
            }
            else
            {
                UpdateDepartmentAssignment(assignment, existingAssignment);
            }
        }

        return null;
    }

    internal static ProblemDetails? UpdateCampaignDate(CampaignDate dto, DataAccess.CampaignDate date)
    {
        date.StartTime = dto.StartTime;
        date.EndTime = dto.EndTime;
        date.Status = dto.Status;
        var mergeResult = MergeDepartmentAssignments(dto.DepartmentAssignments, date.DepartmentAssignments);
        if (mergeResult is not null)
        {
            mergeResult.Instance = $"{date.Date:O}/{mergeResult.Instance}";
            return mergeResult;
        }

        return null;
    }

    internal static ProblemDetails? MergeCampaignDates(IEnumerable<CampaignDate>? dto, List<DataAccess.CampaignDate> dates)
    {
        var deletedDates = dates.Where(date => !(dto?.Any(d => d.Date == date.Date) ?? false)).ToArray();
        foreach (var date in deletedDates)
        {
            if (date.DepartmentAssignments.Any(da => da.Registrations.Any()))
            {
                return new ProblemDetails
                {
                    Title = "Cannot delete date with registrations",
                    Instance = date.Date.ToString("O")
                };
            }

            dates.Remove(date);
        }

        foreach (var dtoDate in dto ?? [])
        {
            var existingDate = dates.FirstOrDefault(d => d.Date == dtoDate.Date);
            if (existingDate is null)
            {
                if (dtoDate.Date < DateOnly.FromDateTime(DateTimeOffset.Now.Date))
                {
                    return new ProblemDetails
                    {
                        Title = "Cannot add date in the past",
                        Instance = dtoDate.Date.ToString("O")
                    };
                }

                dates.Add(new DataAccess.CampaignDate
                {
                    Date = dtoDate.Date,
                    StartTime = dtoDate.StartTime,
                    EndTime = dtoDate.EndTime,
                    Status = dtoDate.Status,
                });
            }
            else
            {
                var result = UpdateCampaignDate(dtoDate, existingDate);
                if (result is not null) { return result; }
            }
        }

        return null;
    }

    internal static ProblemDetails? UpdateCampaignFromDto(Campaign dto, DataAccess.Campaign campaign)
    {
        campaign.Name = dto.Name;
        campaign.Organizer = dto.Organizer;
        campaign.ReservedRatioForGirls = dto.ReservedRatioForGirls;
        campaign.PurgeDate = dto.PurgeDate;
        campaign.Status = dto.Status;

        var mergeResult = MergeCampaignDates(dto.Dates, campaign.Dates);
        if (mergeResult is not null)
        {
            mergeResult.Instance = $"{campaign.Id:O}/{mergeResult.Instance}";
            return mergeResult;
        }

        return null;
    }

    #region DTOs
    public class Campaign
    {
        public required string Name { get; set; }
        public required string Organizer { get; set; }
        public CampaignDate[]? Dates { get; set; } = [];
        public decimal? ReservedRatioForGirls { get; set; }
        public DateOnly? PurgeDate { get; set; }
        public CampaignStatus Status { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class CampaignDate
    {
        public required DateOnly Date { get; set; }
        public DepartmentAssignment[]? DepartmentAssignments { get; set; } = [];
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public CampaignDateStatus Status { get; set; } = CampaignDateStatus.Hidden;
    }

    public class DepartmentAssignment
    {
        public required string DepartmentName { get; set; }
        public required short NumberOfSeats { get; set; }
        public decimal? ReservedRatioForGirls { get; set; }
    }
    #endregion

    #region Validators
    public class CampaignValidator(IDtoValidator<CampaignDate> dateValidator) : IDtoValidator<Campaign>
    {
        public IEnumerable<ValidationError> Validate(Campaign dto)
        {
            var name = dto.Name ?? string.Empty;

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                yield return new ValidationError(name, nameof(dto.Name), "Campaign name must be set");
            }

            if (string.IsNullOrWhiteSpace(dto.Organizer))
            {
                yield return new ValidationError(name, nameof(dto.Organizer), "Campaign organizer must be set");
            }

            if (dto.ReservedRatioForGirls != null && (dto.ReservedRatioForGirls < 0 || dto.ReservedRatioForGirls > 1))
            {
                yield return new ValidationError(name, nameof(dto.ReservedRatioForGirls), "Reserved ratio for girls must be between 0 and 1");
            }

            if (dto.PurgeDate != null)
            {
                // We must not check if purge date is in the future. The user might update an old campaign.

                if (dto.Dates?.Any(date => date.Date <= dto.PurgeDate.Value) == true)
                {
                    yield return new ValidationError(name, nameof(dto.PurgeDate), "Purge date for campaign must be after all dates");
                }
            }

            if (!Enum.IsDefined(dto.Status))
            {
                yield return new ValidationError(name, nameof(dto.Status), "Invalid campaign status");
            }

            if (dto.Dates != null)
            {
                var duplicateDates = ValidationHelpers.FindDuplicates(dto.Dates, d => d.Date);
                foreach (var date in duplicateDates)
                {
                    yield return new ValidationError(name, nameof(dto.Dates), $"Duplicate date {date:O} found");
                }
            }

            foreach (var date in dto.Dates ?? [])
            {
                foreach (var result in dateValidator.Validate(date))
                {
                    yield return new ValidationError(name, result);
                }
            }
        }
    }

    public class CampaignDateValidator(IDtoValidator<DepartmentAssignment> departmentAssignmentValidator) : IDtoValidator<CampaignDate>
    {
        public IEnumerable<ValidationError> Validate(CampaignDate dto)
        {
            var dateString = dto.Date.ToString("O", CultureInfo.InvariantCulture);

            // We must not check if date is in the future. The user might update an old campaign.

            if (dto.StartTime != null && dto.EndTime != null && dto.StartTime >= dto.EndTime)
            {
                yield return new ValidationError(dateString, nameof(dto.StartTime), "Start time must be before end time");
            }

            if (Enum.IsDefined(dto.Status))
            {
                yield return new ValidationError(dateString, nameof(dto.Status), "Invalid campaign date status");
            }

            foreach (var assignment in dto.DepartmentAssignments ?? [])
            {
                foreach (var result in departmentAssignmentValidator.Validate(assignment))
                {
                    yield return new ValidationError(dateString, result);
                }
            }
        }
    }

    public class DepartmentAssignmentValidator : IDtoValidator<DepartmentAssignment>
    {
        public IEnumerable<ValidationError> Validate(DepartmentAssignment dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DepartmentName))
            {
                yield return new ValidationError(dto.DepartmentName, nameof(dto.DepartmentName), "Department name must be set");
            }

            if (dto.NumberOfSeats < 1)
            {
                yield return new ValidationError(dto.DepartmentName, nameof(dto.NumberOfSeats), "Number of seats must be greater than 0");
            }

            if (dto.ReservedRatioForGirls != null && (dto.ReservedRatioForGirls < 0 || dto.ReservedRatioForGirls > 1))
            {
                yield return new ValidationError(dto.DepartmentName, nameof(dto.ReservedRatioForGirls), "Reserved ratio for girls must be between 0 and 1");
            }
        }
    }

    #endregion
}
