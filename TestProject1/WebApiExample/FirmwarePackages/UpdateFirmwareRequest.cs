namespace Clever.Firmware.Contracts.FirmwarePackages
{
    public record UpdateFirmwareRequest(
        string ProductId,
        string PackageId,
        Reason? Reason,
        int? Retries,
        DateTimeOffset RetrieveDate,
        int? RetryInterval);
}
