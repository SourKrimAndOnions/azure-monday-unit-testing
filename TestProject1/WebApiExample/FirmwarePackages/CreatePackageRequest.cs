namespace Clever.Firmware.Contracts.FirmwarePackages;

public record CreatePackageRequest(
    string VersionString,
    PackageType PackageType,
    DateTimeOffset ReleaseDate,
    IReadOnlyList<string> InstallsOn);
