using System.Net;
using Atc.Cosmos.EventStore.Cqrs;
using Microsoft.AspNetCore.Mvc;

namespace Clever.Firmware.Api.Endpoints;

public static class CommandProcessorExtensions
{
    public static async Task<IActionResult> CreateActionResultAsync(
        this ValueTask<CommandResult> result)
        => CreateActionResultAsync(await result);

    public static IActionResult CreateActionResultAsync(
        this CommandResult result)
        => result switch
        {
            { Result: ResultType.Conflict }
              => new StatusCodeResult(StatusCodes.Status409Conflict),
            { Result: ResultType.NotModified, Response: { } r }
              => new OkObjectResult(r),
            { Result: ResultType.NotModified }
              => new StatusCodeResult(StatusCodes.Status304NotModified),
            { Result: ResultType.NotFound }
              => new StatusCodeResult(StatusCodes.Status404NotFound),
            { Response: { } r }
              => new OkObjectResult(r),
            { Result: ResultType.Exists }
              => new BadRequestObjectResult(
                    new Contracts.BadRequestDetails(
                        "Stream not empty",
                        new Dictionary<string, string[]> { { "Error", new[] { "Stream is not empty" } } })),
            { } r
              => new OkObjectResult(
                    new Contracts.CommandResult(
                        r.Id.Parts.Count > 1
                            ? r.Id.Parts[1]
                            : r.Id.Parts[0],
                        r.Version.Value)),

            _ => new StatusCodeResult(
                StatusCodes.Status500InternalServerError),
        };
}
