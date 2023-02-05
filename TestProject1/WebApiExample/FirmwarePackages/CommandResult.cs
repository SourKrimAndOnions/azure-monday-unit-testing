namespace Clever.Firmware.Contracts;

public record CommandResult(
    string Id,
    long Etag);
