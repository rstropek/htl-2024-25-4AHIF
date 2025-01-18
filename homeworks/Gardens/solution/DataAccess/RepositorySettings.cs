namespace Gardens.DataAccess;

public class RepositorySettings
{
    public string DataFolder { get; set; } = "data";
    public int NumberOfRetries { get; set; }
    public int RetryDelayMilliseconds { get; set; }
}
