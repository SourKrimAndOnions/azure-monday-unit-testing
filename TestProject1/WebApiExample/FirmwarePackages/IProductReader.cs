using Clever.Firmware.Contracts.FirmwarePackages;

namespace Clever.Firmware.Domain.FirmwarePackages
{
    public interface IProductReader
    {
        Task<FirmwareProductInfo[]> ListAsync(
            CancellationToken cancellationToken);

        Task<FirmwarePackage?> GetPackageById(
            string productId,
            string packageId,
            CancellationToken cancellationToken);

        Task<FirmwareProduct?> GetPackageByVendorNameAndVersionString(
            string vendorName,
            string versionString,
            CancellationToken cancellationToken);

        Task<EligiblePackages?> GetEligibleAsync(
                string versionString,
                string chargePointVendor,
                CancellationToken cancellationToken);

        Task<FirmwareProduct?> GetAsync(
            string productId,
            CancellationToken cancellationToken);

        Task<Uri?> GetBinaryUriAsync(
            string productId,
            string packageId,
            CancellationToken cancellationToken);

        Task<Uri?> GetReleaseNotesUriAsync(
            string productId,
            string packageId,
            CancellationToken cancellationToken);

        Task<string> GetBinaryStoragePathAsync(
            string productId,
            string packageId,
            CancellationToken cancellationToken);

        Task<string> GetReleaseNotesStoragePathAsync(
            string productId,
            string packageId,
            CancellationToken cancellationToken);
    }
}
