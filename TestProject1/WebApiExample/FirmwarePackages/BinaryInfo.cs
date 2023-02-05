namespace Clever.Firmware.Domain.FirmwarePackages;

public record BinaryInfo(
    string StoragePath,
    string FileName,
    long FileSize);
