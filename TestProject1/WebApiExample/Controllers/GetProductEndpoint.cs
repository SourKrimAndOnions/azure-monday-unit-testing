using Clever.Firmware.Contracts.FirmwarePackages;
using Clever.Firmware.Domain.FirmwarePackages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clever.Firmware.Api.Endpoints.FirmwarePackages;

[ApiController]
[Route("/v1/products/{productId}")]
public class GetProductEndpoint
{
    [HttpGet]
    [ProducesResponseType(typeof(FirmwareProduct), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [Authorize(Roles = AuthRoles.FirmwareAll)]
    public async Task<IActionResult> ExecuteAsync(
        [FromServices] IProductReader reader,
        [FromRoute(Name = "productId")] string productId,
        CancellationToken cancellationToken = default)
        => await reader.GetAsync(productId, cancellationToken)
        switch
        {
            { } p => new OkObjectResult(p),
            _ => new NotFoundResult(),
        };
}
