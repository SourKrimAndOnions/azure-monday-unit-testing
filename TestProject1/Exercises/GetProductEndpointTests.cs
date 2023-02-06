using Clever.Firmware.Api.Endpoints.FirmwarePackages;
using Clever.Firmware.Contracts.FirmwarePackages;
using Clever.Firmware.Domain.FirmwarePackages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exercises;
public class GetProductEndpointTests
{
    [Fact]
    public void Is_ApiController()
        => typeof(GetProductEndpoint)
            .GetCustomAttributes(false)
            .Should()
            .ContainItemsAssignableTo<ApiControllerAttribute>();

    [Fact]
    public void Has_Route()
        => typeof(GetProductEndpoint)
            .GetCustomAttributes(false)
            .Should()
            .ContainEquivalentOf(
                new RouteAttribute("/v1/products/{productId}"));

    [Fact]
    public void Is_Get()
        => typeof(GetProductEndpoint)
        .GetMethod(nameof(GetProductEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainItemsAssignableTo<HttpGetAttribute>();

    [Fact]
    public void Authorized_For_FirmwareAll()
        => typeof(GetProductEndpoint)
        .GetMethod(nameof(GetProductEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainEquivalentOf(
            new AuthorizeAttribute
            {
                Roles = AuthRoles.FirmwareAll,
            });

    [Fact]
    public void Produces_Ok_Response_With_FirmwareProduct()
        => typeof(GetProductEndpoint)
        .GetMethod(nameof(GetProductEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainEquivalentOf(
            new ProducesResponseTypeAttribute(
                typeof(FirmwareProduct),
                StatusCodes.Status200OK));

    [Fact]
    public void Produces_NotFound_Response()
        => typeof(GetProductEndpoint)
        .GetMethod(nameof(GetProductEndpoint.ExecuteAsync))
        .GetCustomAttributes(false)
        .Should()
        .ContainEquivalentOf(
            new ProducesResponseTypeAttribute(
                typeof(void),
                StatusCodes.Status404NotFound));

    [Theory, AutoNSubstituteData]
    public async Task Should_Call_ProductReader(
        IProductReader reader,
        GetProductEndpoint sut,
        string packageId,
        string productId,
        CancellationToken cancellationToken)
    {
        await sut.ExecuteAsync(
            reader,
            productId,
            cancellationToken);

        _ = reader
            .Received(1)
            .GetAsync(
                productId,
                cancellationToken);
    }

    [Theory, AutoNSubstituteData]
    public async Task Should_Return_OK_With_FirmwareProduct(
        IProductReader reader,
        GetProductEndpoint sut,
        string packageId,
        string productId,
        FirmwareProduct product,
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        reader
            .GetAsync(default, cancellationToken)
            .ReturnsForAnyArgs(product);

        var result = await sut.ExecuteAsync(
            reader,
            productId,
            cancellationToken);

        result
            .Should()
            .BeEquivalentTo(new OkObjectResult(product));
    }

    [Theory, AutoNSubstituteData]
    public async Task Should_Return_NotFound(
        IProductReader reader,
        GetProductEndpoint sut,
        string productId,
        string packageId,
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        reader
            .GetAsync(default, cancellationToken)
            .ReturnsForAnyArgs(Task.FromResult<FirmwareProduct?>(null));

        var result = await sut.ExecuteAsync(
            reader,
            productId,
            cancellationToken);

        result
            .Should()
            .BeEquivalentTo(new NotFoundResult());
    }
}
