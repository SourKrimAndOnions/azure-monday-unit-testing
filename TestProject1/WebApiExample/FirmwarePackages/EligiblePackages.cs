namespace Clever.Firmware.Contracts.FirmwarePackages;

public record EligiblePackages(
    string ProductId,
    IReadOnlyList<EligiblePackage> Packages);
