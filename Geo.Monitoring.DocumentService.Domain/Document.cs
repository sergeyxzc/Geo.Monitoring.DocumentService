namespace Geo.Monitoring.DocumentService.Domain;

public class Document
{
    public static Document Create(string contentType, string? name, string? description, string[]? labels, byte[] content)
    {
        return new Document
        {
            Name = name,
            ContentType = contentType,
            Description = description,
            Content = content,
            Labels = labels?.Select(x => new DocumentLabel()
            {
                Label = x,
            }).ToList()
        };
    }

    public int Id { get; set; }

    public string ContentType { get; set; } = default!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public byte[] Content { get; set; } = default!;

    public ICollection<DocumentLabel>? Labels { get; set; }
}