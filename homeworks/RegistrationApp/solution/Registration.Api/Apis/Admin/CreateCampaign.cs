using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Registration.Api.CrossCuttingConcerns;
using Registration.Api.DataAccess;

namespace Registration.Api.Apis.Admin;

public static class CampaignCreationApi
{
    public static async Task<IResult> CreateCampaign([FromBody] CreateCampaignRequest request, IJsonFileRepository repository)
    {
        // Note that we do not need to validate the request here. The validation filter did that for us.

        // Convert DTO to model class from data access layer.
        var campaign = new DataAccess.Campaign
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Organizer = request.Organizer,
            Status = CampaignStatus.Inactive, // Campaigns are inactive by default, must be activated explicitly
            Dates = [.. request.Dates?.Select(date => new DataAccess.CampaignDate
            {
                Date = date.Date,
                StartTime = date.StartTime,
                EndTime = date.EndTime,
                Status = CampaignDateStatus.Active, // Dates are active by default, must be hidden explicitly
                DepartmentAssignments = [.. date.DepartmentAssignments?.Select(assignment => new DataAccess.DepartmentAssignment
                {
                    DepartmentName = assignment.DepartmentName,
                    NumberOfSeats = assignment.NumberOfSeats,
                    ReservedRatioForGirls = assignment.ReservedRatioForGirls
                }) ?? []],
            }) ?? []],
            ReservedRatioForGirls = request.ReservedRatioForGirls,
            PurgeDate = request.PurgeDate,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedBy = "Admin API", // TODO: Set the current user here (not implemented in this exammple)
            UpdatedAt = DateTimeOffset.UtcNow,
            UpdatedBy = "Admin API", // TODO: Set the current user here (not implemented in this exammple)
        };

        await repository.Create(campaign.Id, campaign);

        return Results.Ok(new CreateCampaignResponse(campaign.Id));
    }

    #region DTOs
    public record CreateCampaignRequest(
        string Name,
        string Organizer,
        CreateDateRequest[]? Dates = null,
        decimal? ReservedRatioForGirls = null,
        DateOnly? PurgeDate = null);

    public record CreateDateRequest(
        DateOnly Date,
        DepartmentAssignmentRequest[]? DepartmentAssignments = null,
        TimeOnly? StartTime = null,
        TimeOnly? EndTime = null
    );

    public record DepartmentAssignmentRequest(
        string DepartmentName,
        short NumberOfSeats,
        decimal? ReservedRatioForGirls = null
    );

    public record CreateCampaignResponse(
        Guid Id
    // Many APIs return more data than just the ID. This decision has to be made
    // on a case-by-case basis.
    );
    #endregion

    #region Validators
    public class CampaignValidator(IDtoValidator<CreateDateRequest> dateValidator) : IDtoValidator<CreateCampaignRequest>
    {
        public IEnumerable<ValidationError> Validate(CreateCampaignRequest dto)
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
                if (dto.PurgeDate < DateOnly.FromDateTime(DateTimeOffset.Now.Date))
                {
                    yield return new ValidationError(name, nameof(dto.PurgeDate), "Purge date for campaign must be in the future");
                }

                if (dto.Dates?.Any(date => date.Date >= dto.PurgeDate.Value) == true)
                {
                    yield return new ValidationError(name, nameof(dto.PurgeDate), "Purge date for campaign must be after all dates");
                }
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

    public class DateValidator(IDtoValidator<DepartmentAssignmentRequest> departmentAssignmentValidator) : IDtoValidator<CreateDateRequest>
    {
        public IEnumerable<ValidationError> Validate(CreateDateRequest dto)
        {
            var dateString = dto.Date.ToString("O", CultureInfo.InvariantCulture);
            if (dto.Date < DateOnly.FromDateTime(DateTimeOffset.Now.Date))
            {
                yield return new ValidationError(dateString, nameof(dto.Date), "Date must be in the future");
            }

            if (dto.StartTime != null && dto.EndTime != null && dto.StartTime >= dto.EndTime)
            {
                yield return new ValidationError(dateString, nameof(dto.StartTime), "Start time must be before end time");
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

    public class DepartmentAssignmentValidator : IDtoValidator<DepartmentAssignmentRequest>
    {
        public IEnumerable<ValidationError> Validate(DepartmentAssignmentRequest dto)
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
