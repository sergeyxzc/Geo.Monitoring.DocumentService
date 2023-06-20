using System.Data;
using System.Threading;
using Geo.Monitoring.DocumentService.Domain;
using Geo.Monitoring.DocumentService.Persistent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Geo.Monitoring.DocumentService.Application;

public record UploadDocumentRequest(string ContentType, string? Name, string? Description, string[]? Labels, Stream ContentStream);
public record UploadDocumentResponse(int DocumentId);

public record FindRequest(string Label);
public record FindResponse(DocumentResponse[] Documents);
public record DocumentResponse(int DocumentId, string ContentType, string? Name, string? Description);

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
        //await Task.Delay(10000, cancellationToken);
        var trans = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        using var memStream = new MemoryStream(64 * 1024);
        await request.ContentStream.CopyToAsync(memStream, cancellationToken);
        await memStream.FlushAsync(cancellationToken);

        var document = Document.Create(request.ContentType, request.Name, request.Description, request.Labels, memStream.ToArray());

        _dbContext.Documents.Add(document);

        await _dbContext.SaveChangesAsync(cancellationToken);
        await trans.CommitAsync(cancellationToken);

        return new UploadDocumentResponse(document.Id);
    }

    public async Task<DocumentResponse> GetDocumentAsync(int documentId, CancellationToken cancellationToken)
    {
        var document = await _dbContext.Documents
            .Where(x => x.Id == documentId)
            .Select(x => new Document()
            {
                Id = x.Id,
                Name = x.Name,
                ContentType = x.ContentType,
                Description = x.Description,
            }).SingleOrDefaultAsync(cancellationToken);

        if (document == null)
            throw new DocumentException($"Document with id {documentId} not found");

        return new DocumentResponse(document.Id, document.ContentType, document.Name, document.Description);
    }

    public async Task<FindResponse> FindAsync(FindRequest request, CancellationToken cancellationToken)
    {
        var documents = await _dbContext.Labels
            .Where(x => x.Label == request.Label)
            .Select(x => x.Document)
            .Select(x => new Document()
            {
                Id = x.Id,
                Name = x.Name,
                ContentType = x.ContentType,
                Description = x.Description,
            }).ToListAsync(cancellationToken);

        return new FindResponse(documents.Select(x => new DocumentResponse(x.Id, x.ContentType, x.Name, x.Description)).ToArray());
    }

    public async Task<DownloadDocumentResponse> DownloadAsync(int documentId, CancellationToken cancellationToken)
    {
        var document = await _dbContext.Documents
            .SingleOrDefaultAsync(x => x.Id == documentId, cancellationToken);

        if (document == null)
            return null;

        return new DownloadDocumentResponse(document.Name, document.ContentType, document.Content);
    }

    public async Task DeleteAsync(int documentId, CancellationToken cancellationToken)
    {
        var document = new Document() { Id = documentId };
        _dbContext.Documents.Attach(document);
        _dbContext.Documents.Remove(document);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}