namespace Clever.Firmware.Contracts.FirmwarePackages;

public record EligiblePackage(
    string Id,
    DateTimeOffset ReleaseDate,
    DateTimeOffset CreatedDate,
    DateTimeOffset? ApprovedDate,
    string VersionString,
    PackageType Type,
    PackageState State,
    IReadOnlyList<string> InstallsOn,
    Uri? PackageUrl,
    Uri? ReleaseNotesUrl);
