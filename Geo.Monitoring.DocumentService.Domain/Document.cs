namespace Geo.Monitoring.DocumentService.Domain;

public class Document
{
    public static Document Create(string contentType, string? externalId, string? name, string? description, byte[] content)
    {
        return new Document()
        {
            ExternalId = externalId,
            Name = name,
            ContentType = contentType,
            Description = description,
            Content = content
        };
    }

    public int Id { get; set; }
    public string? ExternalId { get; set; }
    public string ContentType { get; set; } = default!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public byte[] Content { get; set; } = default!;
}