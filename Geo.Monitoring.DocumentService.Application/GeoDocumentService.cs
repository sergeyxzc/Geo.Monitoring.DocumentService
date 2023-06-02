using Geo.Monitoring.DocumentService.Domain;
using Geo.Monitoring.DocumentService.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Geo.Monitoring.DocumentService.Application;

public record UploadDocumentRequest(string ContentType, string? Name, string? ExternalId, string? Description, Stream ContentStream);
public record UploadDocumentResponse(int DocumentId);

public record FindRequest(int? DocumentId = null, string? ExternalId = null);

public record FindDocumentResponse(int DocumentId, string? ExternalId, string? Name, string? ContentType, string? Description);

public record DownloadDocumentResponse(string? Name, string ContentType, byte[] Content);

public class GeoDocumentService
{
    private readonly DocumentDbContext _dbContext;

    public GeoDocumentService(DocumentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UploadDocumentResponse> UploadAsync(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        var trans = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var isExternalIdExist = await _dbContext.Documents.AnyAsync(x => x.ExternalId == request.ExternalId, cancellationToken);
        if (isExternalIdExist)
            throw new ArgumentException("ExternalId is already exists");

        using var memStream = new MemoryStream(64 * 1024);
        await request.ContentStream.CopyToAsync(memStream, cancellationToken);
        await memStream.FlushAsync(cancellationToken);

        var document = Document.Create(request.ContentType, request.ExternalId, request.Name, request.Description, memStream.ToArray());

        _dbContext.Documents.Add(document);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await trans.CommitAsync(cancellationToken);

        return new UploadDocumentResponse(document.Id);
    }

    public async Task<FindDocumentResponse> FindAsync(FindRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Documents.AsNoTracking();

        Document? document = null;

        if (request.DocumentId.HasValue)
        {
            document = await query
                .Where(x => x.Id == request.DocumentId.Value)
                .Select(x => new Document()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ContentType = x.ContentType,
                    Description = x.Description,
                    ExternalId = x.ExternalId
                }).SingleOrDefaultAsync(cancellationToken);
        }

        if (document == null && !string.IsNullOrEmpty(request.ExternalId))
        {
            document = await query
                .Where(x => x.ExternalId == request.ExternalId)
                .Select(x => new Document()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ContentType = x.ContentType,
                    Description = x.Description,
                    ExternalId = x.ExternalId
                }).SingleOrDefaultAsync(cancellationToken);
        }

        if (document == null)
            return null;

        return new FindDocumentResponse(document.Id, document.ExternalId, document.Name, document.ContentType, document.Description);
    }

    public async Task<DownloadDocumentResponse> DownloadAsync(int documentId, CancellationToken cancellationToken)
    {
        var document = await _dbContext.Documents
            .SingleOrDefaultAsync(x => x.Id == documentId, cancellationToken);

        if (document == null)
            return null;

        return new DownloadDocumentResponse(document.Name, document.ContentType, document.Content);
    }
}