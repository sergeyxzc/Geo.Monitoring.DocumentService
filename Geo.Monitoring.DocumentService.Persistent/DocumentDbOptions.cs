namespace Geo.Monitoring.DocumentService.Persistent;

public class DocumentDbOptions
{
    public string ConnectionString { get; set; } = default!;
    public int CommandTimeoutSecond { get; set; } = 60;
    public bool MigrationEnabled { get; set; } = false;
}