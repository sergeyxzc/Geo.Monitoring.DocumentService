using Geo.Monitoring.DocumentService.Application;
using Microsoft.AspNetCore.Mvc;

namespace Geo.Monitoring.DocumentService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
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
    public async Task<UploadDocumentResponse> UploadAsync([FromQuery] string? externalId, [FromQuery] string? description, IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        await using var fileStream = file.OpenReadStream();

        var document = await _documentService.UploadAsync(new UploadDocumentRequest(file.ContentType, file.FileName, externalId, description, fileStream), cancellationToken);
        return document;
    }

    [HttpPost("find")]
    public async Task<ActionResult<FindDocumentResponse>> FindDocumentAsync([FromBody] FindRequest request, CancellationToken cancellationToken)
    {
        var response = await _documentService.FindAsync(request, cancellationToken);
        return response;
    }

    [HttpGet("{documentId:int}/download")]
    public async Task<IActionResult> DownloadDocumentAsync([FromRoute] int documentId, CancellationToken cancellationToken)
    {
        var response = await _documentService.DownloadAsync(documentId, cancellationToken);
        return File(response.Content, response.ContentType, response.Name);
    }
}