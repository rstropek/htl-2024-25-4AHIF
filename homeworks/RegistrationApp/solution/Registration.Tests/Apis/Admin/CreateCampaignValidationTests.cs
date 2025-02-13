using Moq;
using Registration.Api.CrossCuttingConcerns;
using static Registration.Api.Apis.Admin.CampaignCreationApi;

namespace Registration.Tests.Apis.Admin;

public class CampaignValidatorTests
{
    private readonly Mock<IDtoValidator<CreateDateRequest>> dateValidatorMock;
    private readonly CampaignValidator validator;

    public CampaignValidatorTests()
    {
        dateValidatorMock = new Mock<IDtoValidator<CreateDateRequest>>();
        validator = new CampaignValidator(dateValidatorMock.Object);
    }

    [Fact]
    public void Validate_WithValidRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = new CreateCampaignRequest(
            Name: "Test Campaign",
            Organizer: "Test Organizer"
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidName_ReturnsError(string? name)
    {
        // Arrange
        var request = new CreateCampaignRequest(
            Name: name!,
            Organizer: "Test Organizer"
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => e.Source == nameof(request.Name));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidOrganizer_ReturnsError(string? organizer)
    {
        // Arrange
        var request = new CreateCampaignRequest(
            Name: "Test Campaign", 
            Organizer: organizer!
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => e.Source == nameof(request.Organizer));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    public void Validate_WithInvalidReservedRatio_ReturnsError(decimal ratio)
    {
        // Arrange
        var request = new CreateCampaignRequest(
            Name: "Test Campaign",
            Organizer: "Test Organizer",
            ReservedRatioForGirls: ratio
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => e.Source == nameof(request.ReservedRatioForGirls));
    }

    [Fact]
    public void Validate_WithPastPurgeDate_ReturnsError()
    {
        // Arrange
        var request = new CreateCampaignRequest(
            Name: "Test Campaign",
            Organizer: "Test Organizer",
            PurgeDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => e.Source == nameof(request.PurgeDate));
    }

    [Fact]
    public void Validate_WithPurgeDateBeforeDate_ReturnsError()
    {
        // Arrange
        var dates = new[]
        {
            new CreateDateRequest(
                Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
                StartTime: new TimeOnly(9, 0),
                EndTime: new TimeOnly(17, 0)
            )
        };

        var request = new CreateCampaignRequest(
            Name: "Test Campaign",
            Organizer: "Test Organizer",
            Dates: dates,
            PurgeDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3))
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => 
            e.Source == nameof(request.PurgeDate) && 
            e.Message == "Purge date for campaign must be after all dates"
        );
    }

    [Fact]
    public void Validate_WithDates_CallsDateValidator()
    {
        // Arrange
        var dates = new[]
        {
            new CreateDateRequest(
                Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                StartTime: new TimeOnly(9, 0),
                EndTime: new TimeOnly(17, 0)
            ),
            new CreateDateRequest(
                Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                StartTime: new TimeOnly(9, 0),
                EndTime: new TimeOnly(17, 0)
            )
        };

        var request = new CreateCampaignRequest(
            Name: "Test Campaign",
            Organizer: "Test Organizer",
            Dates: dates
        );

        dateValidatorMock
            .Setup(v => v.Validate(It.IsAny<CreateDateRequest>()))
            .Returns(Enumerable.Empty<ValidationError>());

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Empty(errors);
        dateValidatorMock.Verify(
            v => v.Validate(It.IsAny<CreateDateRequest>()), 
            Times.Exactly(2),
            "Date validator should be called for each date"
        );
    }

    [Fact]
    public void Validate_WithDates_ReturnsDatesErrors()
    {
        // Arrange
        var dates = new[]
        {
            new CreateDateRequest(
                Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                StartTime: new TimeOnly(9, 0),
                EndTime: new TimeOnly(17, 0)
            )
        };

        var request = new CreateCampaignRequest(
            Name: "Test Campaign",
            Organizer: "Test Organizer",
            Dates: dates
        );

        dateValidatorMock
            .Setup(v => v.Validate(It.IsAny<CreateDateRequest>()))
            .Returns([new ValidationError("Instance", "Source", "Date must be in the future")]);

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Single(errors);
        Assert.Equal("Test Campaign/Instance", errors[0].Instance);
        Assert.Equal("Source", errors[0].Source);
        Assert.Equal("Date must be in the future", errors[0].Message);
    }

    [Fact]
    public void Validate_WithDuplicateDates_ReturnsError()
    {
        // Arrange
        var dates = new[]
        {
            new CreateDateRequest(
                Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                StartTime: new TimeOnly(9, 0),
                EndTime: new TimeOnly(17, 0)
            ),
            new CreateDateRequest(
                Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), // Same date
                StartTime: new TimeOnly(10, 0),
                EndTime: new TimeOnly(18, 0)
            )
        };

        var request = new CreateCampaignRequest(
            Name: "Test Campaign",
            Organizer: "Test Organizer", 
            Dates: dates
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Single(errors);
        Assert.Equal("Test Campaign", errors[0].Instance);
        Assert.Equal(nameof(request.Dates), errors[0].Source);
        Assert.Contains("Duplicate date", errors[0].Message);
    }
}

public class DateValidatorTests
{
    private readonly DateValidator validator;
    private readonly Mock<IDtoValidator<DepartmentAssignmentRequest>> departmentValidatorMock;

    public DateValidatorTests()
    {
        departmentValidatorMock = new Mock<IDtoValidator<DepartmentAssignmentRequest>>();
        validator = new DateValidator(departmentValidatorMock.Object);
    }

    [Fact]
    public void Validate_WithValidRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = new CreateDateRequest(
            Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            StartTime: new TimeOnly(9, 0),
            EndTime: new TimeOnly(17, 0)
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_WithPastDate_ReturnsError()
    {
        // Arrange
        var request = new CreateDateRequest(
            Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => e.Source == nameof(request.Date));
    }

    [Fact]
    public void Validate_WithEndTimeBeforeStartTime_ReturnsError()
    {
        // Arrange
        var request = new CreateDateRequest(
            Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            StartTime: new TimeOnly(17, 0),
            EndTime: new TimeOnly(9, 0)
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => e.Source == nameof(request.StartTime));
    }
}

public class DepartmentAssignmentValidatorTests
{
    private readonly DepartmentAssignmentValidator validator;

    public DepartmentAssignmentValidatorTests()
    {
        validator = new DepartmentAssignmentValidator();
    }

    [Fact]
    public void Validate_WithValidRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = new DepartmentAssignmentRequest(
            DepartmentName: "Test Department",
            NumberOfSeats: 10,
            ReservedRatioForGirls: 0.5m
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithInvalidDepartmentName_ReturnsError(string? name)
    {
        // Arrange
        var request = new DepartmentAssignmentRequest(
            DepartmentName: name!,
            NumberOfSeats: 10
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => e.Source == nameof(request.DepartmentName));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidNumberOfSeats_ReturnsError(short seats)
    {
        // Arrange
        var request = new DepartmentAssignmentRequest(
            DepartmentName: "Test Department",
            NumberOfSeats: seats
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => e.Source == nameof(request.NumberOfSeats));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    public void Validate_WithInvalidReservedRatio_ReturnsError(decimal ratio)
    {
        // Arrange
        var request = new DepartmentAssignmentRequest(
            DepartmentName: "Test Department",
            NumberOfSeats: 10,
            ReservedRatioForGirls: ratio
        );

        // Act
        var errors = validator.Validate(request).ToList();

        // Assert
        Assert.Contains(errors, e => e.Source == nameof(request.ReservedRatioForGirls));
    }
}
