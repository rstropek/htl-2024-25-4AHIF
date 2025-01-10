namespace SoftwareInventoryApi.DataAccess;

public enum OperatingSystem
{
    Windows,
    Linux,
    MacOS
}

public class Computer
{
    public required string MacAddress { get; set; }
    public required string IpAddress { get; set; }
    public required string Hostname { get; set; }
    public required string Cpu { get; set; }
    public required decimal RamGb { get; set; }
    public required decimal DiskSizeGb { get; set; }
    public required OperatingSystem Os { get; set; }
    public required DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset? DecommissionedAt { get; set; }
    public List<Software> Software { get; set; } = [];
}

public class Software
{
    public required string Identifier { get; set; }
    public required string Name { get; set; }
    public string? Vendor { get; set; }
    public required string Version { get; set; }
    public DateTimeOffset? FirstReported { get; set; }
    public DateTimeOffset? LastReported { get; set; }
}