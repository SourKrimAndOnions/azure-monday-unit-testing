namespace Clever.Firmware.Contracts.FirmwarePackages;

public record FirmwareProduct(
    string Id,
    string ChargePointVendor,
    DateTimeOffset Timestamp,
    IReadOnlyList<FirmwarePackage> Packages);
