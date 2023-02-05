using Microsoft.AspNetCore.Http;

namespace Clever.Firmware.Domain.FirmwarePackages
{
    public interface IBinaryStorage
    {
        Uri GetUri(string storagePath);

        Task<BinaryInfo> UploadAsync(
            IFormFile file,
            CancellationToken cancellationToken);

        Task<Azure.Response<bool>> DeleteAsync(
            string storagePath,
            CancellationToken cancellationToken);

        Task<Azure.Response> RestoreAsync(
            string storagePath,
            CancellationToken cancellationToken);
    }
}
