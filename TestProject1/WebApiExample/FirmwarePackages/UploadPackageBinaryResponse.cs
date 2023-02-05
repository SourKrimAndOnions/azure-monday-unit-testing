namespace Clever.Firmware.Contracts.FirmwarePackages
{
    public record UploadPackageBinaryResponse(
        string ProductId,
        string PackageId,
        string StoragePath,
        string FileName,
        long FileSize);
}
