using Geo.Monitoring.DocumentService.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geo.Monitoring.DocumentService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class DocumentController : ControllerBase
{
    private readonly GeoDocumentService _documentService;
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(GeoDocumentService documentService, ILogger<DocumentController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<UploadDocumentResponse> UploadAsync([FromQuery] string? description, [FromQuery] string[] labels, IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        await using var fileStream = file.OpenReadStream();

        var document = await _documentService.UploadAsync(new UploadDocumentRequest(file.ContentType, file.FileName, description, labels, fileStream), cancellationToken);
        return document;
    }

    [HttpGet("{documentId:int}")]
    public async Task<DocumentResponse> GetDocumentAsync([FromRoute] int documentId, CancellationToken cancellationToken)
    {
        var response = await _documentService.GetDocumentAsync(documentId, cancellationToken);
        return response;
    }

    [HttpGet("find")]
    public async Task<ActionResult<FindResponse>> FindDocumentsAsync([FromQuery] string label, CancellationToken cancellationToken)
    {
        var response = await _documentService.FindAsync(new FindRequest(label), cancellationToken);
        return response;
    }

    [HttpGet("{documentId:int}/download")]
    public async Task<IActionResult> DownloadDocumentAsync([FromRoute] int documentId, CancellationToken cancellationToken)
    {
        var response = await _documentService.DownloadAsync(documentId, cancellationToken);
        return File(response.Content, response.ContentType, response.Name);
    }

    [HttpDelete("{documentId:int}")]
    public async Task DeleteDocumentAsync([FromRoute] int documentId, CancellationToken cancellationToken)
    {
        await _documentService.DeleteAsync(documentId, cancellationToken);
    }
}