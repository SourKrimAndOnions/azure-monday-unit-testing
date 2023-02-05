namespace Clever.Firmware.Contracts;

public record BadRequestDetails(
    string Detail,
    IDictionary<string, string[]> Errors,
    int? Status = 400,
    string Title = "One or more validation errors occurred.",
    string Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    string Instance = "");