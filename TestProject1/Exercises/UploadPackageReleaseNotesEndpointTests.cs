using System.Text;
using Atc.Cosmos.EventStore.Cqrs;
using Clever.Firmware.Api.Endpoints;
using Clever.Firmware.Api.Endpoints.FirmwarePackages;
using Clever.Firmware.Contracts.FirmwarePackages;
using Clever.Firmware.Domain.FirmwarePackages;
using Clever.Firmware.Domain.FirmwarePackages.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exercises;

public class UploadPackageReleaseNotesEndpointTests
{
    [Fact]
    public void Is_ApiController()
        => typeof(UploadPackageReleaseNotesEndpoint)
            .GetCustomAttributes(false)
            .Should()
            .ContainItemsAssignableTo<ApiControllerAttribute>();

    [Fact]
    public void Has_Route()
        => typeof(UploadPackageReleaseNotesEndpoint)
            .GetCustomAttributes(false)
            .Should()
            .ContainEquivalentOf(
                new RouteAttribute("/v1/products/{productId}/packages/{packageId}/release-notes"));

    [Fact]
    public void Is_Post()
        => typeof(UploadPackageReleaseNotesEndpoint)
        .GetMethod(nameof(UploadPackageReleaseNotesEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainItemsAssignableTo<HttpPostAttribute>();

    [Fact]
    public void Authorized_For_Administrators()
        => typeof(UploadPackageReleaseNotesEndpoint)
        .GetMethod(nameof(UploadPackageReleaseNotesEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainEquivalentOf(
            new AuthorizeAttribute
            {
                Roles = AuthRoles.FirmwareAdministrator,
            });

    [Fact]
    public void Produces_Ok_Response_With_UploadPackageBinaryResponse()
        => typeof(UploadPackageReleaseNotesEndpoint)
        .GetMethod(nameof(UploadPackageReleaseNotesEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainEquivalentOf(
            new ProducesResponseTypeAttribute(
                typeof(UploadPackageBinaryResponse),
                StatusCodes.Status200OK));

    [Fact]
    public void Produces_NotModified_Response()
        => typeof(UploadPackageReleaseNotesEndpoint)
        .GetMethod(nameof(UploadPackageReleaseNotesEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainEquivalentOf(
            new ProducesResponseTypeAttribute(
                typeof(void),
                StatusCodes.Status304NotModified));

    [Fact]
    public void Produces_Conflict_Response()
        => typeof(UploadPackageReleaseNotesEndpoint)
        .GetMethod(nameof(UploadPackageReleaseNotesEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainEquivalentOf(
            new ProducesResponseTypeAttribute(
                typeof(void),
                StatusCodes.Status409Conflict));

    [Fact]
    public void Produces_NotFound_Response()
        => typeof(UploadPackageReleaseNotesEndpoint)
        .GetMethod(nameof(UploadPackageReleaseNotesEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainEquivalentOf(
            new ProducesResponseTypeAttribute(
                typeof(void),
                StatusCodes.Status404NotFound));

    [Theory, AutoNSubstituteData]
    public async Task Should_Call_CommandProcessor(
        [Frozen] ICommandProcessor<RegisterPackageReleaseNotesCommand> processor,
        [Frozen] IBinaryStorage storage,
        IProductReader reader,
        BinaryInfo info,
        UploadPackageReleaseNotesEndpoint sut,
        FirmwareProduct product,
        SingleFileRequest request,
        CancellationToken cancellationToken)
    {
        var file = GetFileMock("application/pdf", string.Empty);
        request = request with { File = file };

        reader
            .GetAsync(product.Id, cancellationToken)
            .Returns(product);

        storage
            .UploadAsync(request.File, cancellationToken)
            .Returns(info);

        await sut.ExecuteAsync(
            reader,
            product.Id,
            product.Packages[0].Id,
            request,
            cancellationToken);

        _ = processor
            .Received(1)
            .ExecuteAsync(
                Arg.Is<RegisterPackageReleaseNotesCommand>(c
                    => c.ProductId.Id == product.Id
                    && c.PackageId == product.Packages[0].Id
                    && c.StoragePath == info.StoragePath
                    && c.FileName == info.FileName
                    && c.FileSize == info.FileSize),
                cancellationToken);
    }

    private static IFormFile GetFileMock(string contentType, string content)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(content);

        var file = new FormFile(
            baseStream: new MemoryStream(bytes),
            baseStreamOffset: 0,
            length: bytes.Length,
            name: "Data",
            fileName: "dummy.pdf")
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType,
        };

        return file;
    }
}
