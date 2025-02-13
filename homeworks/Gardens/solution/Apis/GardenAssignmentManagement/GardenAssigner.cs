using Gardens.DataAccess;
using Gardens.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Gardens.Apis.GardenAssignmentManagement;

public static class GardenAssigner
{
    public static async Task<IResult> HandleAssignment([FromBody] MemberDto memberDto, [FromRoute] string gardenName, GardenNameConverter gardenNameConverter, IJsonFileRepository repository)
    {
        var gardenCoordinates = gardenNameConverter.ConvertGardenNameToCoordinates(gardenName);
        if (gardenCoordinates == null)
        {
            return ValidationHelpers.GetInvalidGardenNameProblemDetails(gardenName);
        }

        if (repository.EnumerateAll().Any(file => file.Id == gardenCoordinates.ToString()))
        {
            return ValidationHelpers.GetGardenAlreadyAssignedProblemDetails(gardenName);
        }

        var newMember = new Member
        {
            Name = memberDto.Name,
            Email = memberDto.Email,
        };
        await repository.Create(gardenCoordinates.ToString()!, newMember);
        return Results.Created("", newMember);
    }

    public class MemberDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    public static Dictionary<string, string[]> ValidateMember(MemberDto member)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(member.Name))
        {
            errors["Name"] = ["Name is required"];
        }

        if (string.IsNullOrWhiteSpace(member.Email))
        {
            errors["Email"] = ["Email is required"];
        }

        return errors;
    }

}
