namespace Gardens.DataAccess;

public class Member
{
    // Note that we do not need to store the garden name as its coordinates are part of the file name.
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTimeOffset AssignmentTimestamp { get; set; } = DateTimeOffset.UtcNow;
}
