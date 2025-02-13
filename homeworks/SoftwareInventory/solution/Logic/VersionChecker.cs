namespace SoftwareInventoryApi.Logic;

public class VersionChecker
{
  public bool IsOutdated(string inputVersion, string installedVersion)
  {
    var inputParts = inputVersion.Split('.').Select(int.Parse).ToArray();
    var installedParts = installedVersion.Split('.').Select(int.Parse).ToArray();

    // Compare based on the number of segments in input version
    var inputSegments = inputVersion.Split('.').Length;

    return inputSegments switch
    {
      // Single segment (major version only)
      1 => installedParts[0] <= inputParts[0],
      // Two segments (major.minor)
      2 => installedParts[0] < inputParts[0] ||
            (installedParts[0] == inputParts[0] && installedParts[1] <= inputParts[1]),
      // Three segments (major.minor.patch)
      3 => installedParts[0] < inputParts[0] ||
            (installedParts[0] == inputParts[0] && installedParts[1] < inputParts[1]) ||
            (installedParts[0] == inputParts[0] && installedParts[1] == inputParts[1] && installedParts[2] <= inputParts[2]),
      _ => throw new ArgumentException("Invalid version format", nameof(inputVersion)),
    };
  }

  public bool IsWithinRange(string inputVersion, string versionFilter)
  {
    var inputParts = inputVersion.Split('.').Select(int.Parse).ToArray();
    var isCaretRange = versionFilter.StartsWith('^');
    var isTildeRange = versionFilter.StartsWith('~');
    if (isCaretRange || isTildeRange)
    {
      versionFilter = versionFilter[1..];
    }

    var filterParts = versionFilter.Split('.').Select(int.Parse).ToArray();

    // Exact version match
    if (!isCaretRange && !isTildeRange)
    {
      return inputParts.SequenceEqual(filterParts);
    }

    // Check if version is lower than the minimum. If it is, it is definitely not within the range
    if (inputParts[0] < filterParts[0] ||
        (inputParts[0] == filterParts[0] && inputParts[1] < filterParts[1]) ||
        (inputParts[0] == filterParts[0] && inputParts[1] == filterParts[1] && inputParts[2] < filterParts[2]))
    {
      return false;
    }

    // Caret range (^) - allows changes up to the next major version
    if (isCaretRange)
    {
      return inputParts[0] == filterParts[0];
    }

    // Tilde range (~) - allows only patch-level changes
    if (isTildeRange)
    {
      return inputParts[0] == filterParts[0] && inputParts[1] == filterParts[1];
    }

    return false;
  }
}
