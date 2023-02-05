namespace Clever.Firmware.Contracts.FirmwarePackages;

public record FirmwarePackage(
    string Id,
    DateTimeOffset ReleaseDate,
    DateTimeOffset CreatedDate,
    DateTimeOffset? ApprovedDate,
    string VersionString,
    PackageType Type,
    PackageState State,
    IReadOnlyList<string> InstallsOn,
    BinaryFile? ReleaseNotes,
    BinaryFile? PackageFile,
    int Revision);
