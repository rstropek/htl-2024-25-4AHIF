namespace SoftwareInventoryApi.Logic;

public class VersionChecker
{
    public bool IsOutdated(string inputVersion, string installedVersion)
    {
        var inputParts = inputVersion.Split('.').Select(int.Parse).ToArray();
        var installedParts = installedVersion.Split('.').Select(int.Parse).ToArray();

        // Normalize arrays to length 3 by padding with zeros
        Array.Resize(ref inputParts, 3);
        Array.Resize(ref installedParts, 3);

        // Compare based on the number of segments in input version
        var inputSegments = inputVersion.Split('.').Length;

        switch (inputSegments)
        {
            case 1: // Single segment (major version only)
                return installedParts[0] <= inputParts[0];

            case 2: // Two segments (major.minor)
                return installedParts[0] < inputParts[0] ||
                       (installedParts[0] == inputParts[0] && installedParts[1] <= inputParts[1]);

            case 3: // Three segments (major.minor.patch)
                return installedParts[0] < inputParts[0] ||
                       (installedParts[0] == inputParts[0] && installedParts[1] < inputParts[1]) ||
                       (installedParts[0] == inputParts[0] && installedParts[1] == inputParts[1] && installedParts[2] <= inputParts[2]);

            default:
                throw new ArgumentException("Invalid version format", nameof(inputVersion));
        }
    }
}
