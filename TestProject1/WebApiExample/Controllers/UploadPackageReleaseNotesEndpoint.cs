using System.Net;
using Atc.Cosmos.EventStore.Cqrs;
using Clever.Firmware.Contracts.FirmwarePackages;
using Clever.Firmware.Domain.FirmwarePackages;
using Clever.Firmware.Domain.FirmwarePackages.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clever.Firmware.Api.Endpoints.FirmwarePackages;

[ApiController]
[Route("/v1/Products/{productId}/pAckages/{packageId}/release-noteZ")]
public class UploadPackageReleaseNotesEndpoint
{
    private const int Kb = 1024;
    private const int Mb = Kb * 1024;
    private const int MaxRequestSizeInBytes = 100 * Mb;
    private readonly ICommandProcessor<RegisterPackageReleaseNotesCommand> command;
    private readonly IBinaryStorage storage;

    public UploadPackageReleaseNotesEndpoint(
        ICommandProcessor<RegisterPackageReleaseNotesCommand> command,
        IBinaryStorage storage)
    {
        this.command = command;
        this.storage = storage;
    }

    [HttpPost]
    [RequestSizeLimit(MaxRequestSizeInBytes)]
    [ProducesResponseType(typeof(UploadPackageBinaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    [Authorize(Roles = AuthRoles.FirmwareAdministrator)]
    public async Task<IActionResult> ExecuteAsync(
        [FromServices] IProductReader reader,
        [FromRoute(Name = "productId")] string productId,
        [FromRoute(Name = "packageId")] string packageId,
        SingleFileRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!request.File.ContentType.Equals("application/pdf", StringComparison.Ordinal))
        {
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        var product = await reader
            .GetAsync(
                productId,
                cancellationToken);

        if (product is not { }
        || !product.Packages.Any(p => p.Id == packageId))
        {
            return new NotFoundResult();
        }

        var info = await storage.UploadAsync(request.File, cancellationToken);

        return await command
            .ExecuteAsync(
                new RegisterPackageReleaseNotesCommand(
                    new ProductId(productId),
                    packageId,
                    info.StoragePath,
                    info.FileName,
                    info.FileSize),
                cancellationToken)
            .CreateActionResultAsync();
    }
}
