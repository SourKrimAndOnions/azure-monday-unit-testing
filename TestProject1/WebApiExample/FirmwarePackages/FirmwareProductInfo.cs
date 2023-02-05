namespace Clever.Firmware.Contracts.FirmwarePackages;

public record FirmwareProductInfo(
    string Id,
    string ChargePointVendor,
    DateTimeOffset Timestamp,
    int PackageCount);