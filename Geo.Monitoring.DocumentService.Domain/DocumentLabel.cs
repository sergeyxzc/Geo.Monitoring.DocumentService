namespace Geo.Monitoring.DocumentService.Domain;

public class DocumentLabel
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public Document Document { get; set; } = default!;
    public string Label { get; set; } = default!;
}