using Atc.Cosmos.EventStore.Cqrs;

namespace Clever.Firmware.Domain.FirmwarePackages.Commands;

public record RegisterPackageReleaseNotesCommand(
    ProductId ProductId,
    string PackageId,
    string StoragePath,
    string FileName,
    long FileSize)
    : CommandBase<ProductId>(
        ProductId,
        RequiredVersion: EventStreamVersion.Exists);
