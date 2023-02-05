namespace Clever.Firmware.Contracts.FirmwarePackages;

public record EditPackageRequest(
    DateTimeOffset? ReleaseDate,
    string? VersionString,
    int? Revision,
    PackageType? Type,
    IReadOnlyList<string> InstallsOn);
