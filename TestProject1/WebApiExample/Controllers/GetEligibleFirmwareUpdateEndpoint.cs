using Clever.Firmware.Contracts.FirmwarePackages;
using Clever.Firmware.Domain.FirmwarePackages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clever.Firmware.Api.Endpoints.FirmwarePackages;

[ApiController]
[Route("/v1/products/packages/eligible")]
public class GetEligibleFirmwareUpdateEndpoint
{
    [HttpGet]
    [ProducesResponseType(typeof(EligiblePackages), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    [Authorize(Roles = AuthRoles.FirmwareAdministrator)]
    public async Task<IActionResult> ExecuteAsync(
        [FromServices] IProductReader reader,
        [FromQuery] string versionString,
        [FromQuery] string chargePointVendor,
        CancellationToken cancellationToken = default)
        => await reader
            .GetEligibleAsync(
                versionString,
                chargePointVendor,
                cancellationToken)
            switch
        {
            { Packages: { Count: > 0 } } p => new OkObjectResult(p),
            _ => new NotFoundResult(),
        };
}
