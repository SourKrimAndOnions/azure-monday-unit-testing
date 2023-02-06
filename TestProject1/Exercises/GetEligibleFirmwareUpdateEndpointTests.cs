using System.Reflection;
using Clever.Firmware.Api.Endpoints;
using Clever.Firmware.Api.Endpoints.FirmwarePackages;
using Clever.Firmware.Contracts.FirmwarePackages;
using Clever.Firmware.Domain.FirmwarePackages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exercises;
public class GetEligibleFirmwareUpdateEndpointTests
{
    [Theory]
    [InlineData(typeof(EligiblePackages), StatusCodes.Status200OK)]
    [InlineData(typeof(void), StatusCodes.Status404NotFound)]
    [InlineData(typeof(void), StatusCodes.Status304NotModified)]
    [InlineData(typeof(void), StatusCodes.Status409Conflict)]
    public void ExecuteAsync_Contains_Correct_ResponseTypes(
        Type responseType,
        int statusCode)
        => typeof(GetEligibleFirmwareUpdateEndpoint)
        .GetMethod(nameof(GetEligibleFirmwareUpdateEndpoint.ExecuteAsync))!
        .GetCustomAttributes(false)!
        .Should()
        .ContainEquivalentOf(
            new ProducesResponseTypeAttribute(
                responseType,
                statusCode));

    [Fact]
    public void ExecuteAsync_Authorize_Correct_AuthRoles()
        => typeof(GetEligibleFirmwareUpdateEndpoint)
        .GetMethod(nameof(GetEligibleFirmwareUpdateEndpoint.ExecuteAsync))!
        .GetCustomAttribute<AuthorizeAttribute>()!
        .Roles
        .Should()
        .Be(AuthRoles.FirmwareAdministrator);

    [Fact]
    public void ExecuteAsync_Contains_Correct_HttpMethod()
        => typeof(GetEligibleFirmwareUpdateEndpoint)
        .GetMethod(nameof(GetEligibleFirmwareUpdateEndpoint.ExecuteAsync))!
        .GetCustomAttribute<HttpGetAttribute>()!
        .Should()
        .NotBeNull();

    [Fact]
    public void Endpoint_Is_Controller()
        => typeof(GetEligibleFirmwareUpdateEndpoint)
        .GetCustomAttribute<ApiControllerAttribute>()
        .Should()
        .NotBeNull();

    [Fact]
    public void Endpoint_Has_Correct_Route_Template()
        => typeof(GetEligibleFirmwareUpdateEndpoint)
        .GetCustomAttribute<RouteAttribute>()!
        .Template
        .Should()
        .Be("/v1/products/packages/eligible");

    [Theory, AutoNSubstituteData]
    public async Task ExecuteAsync_Returns_OkResult_When_Packages_Are_Valid(
        [Frozen] IProductReader reader,
        GetEligibleFirmwareUpdateEndpoint sut,
        EligiblePackages expectedEligiblePackages)
    {
        reader.GetEligibleAsync(default!, default!, default)
            .ReturnsForAnyArgs(expectedEligiblePackages);

        var result = await sut.ExecuteAsync(
            reader,
            default!,
            default!,
            default);

        result
            .Should()
            .BeOfType<OkObjectResult>()
            .Which
            .Value
            .Should()
            .BeOfType<EligiblePackages>()
            .Which
            .Should()
            .Be(expectedEligiblePackages);
    }

    [Theory, AutoNSubstituteData]
    public async Task ExecuteAsync_Returns_NotFoundResult_When_Packages_Are_Null(
        [Frozen] IProductReader reader,
        GetEligibleFirmwareUpdateEndpoint sut)
        => (await sut.ExecuteAsync(
            reader,
                default!,
                default!,
                default))
        .Should()
        .BeOfType<NotFoundResult>();

    [Theory, AutoNSubstituteData]
    public async Task ExecuteAsync_Returns_NotFoundResult_When_Packages_Are_Empty(
        [Frozen] IProductReader reader,
        GetEligibleFirmwareUpdateEndpoint sut,
        EligiblePackages eligiblePackages)
    {
        eligiblePackages = eligiblePackages with
        {
            Packages = Array.Empty<EligiblePackage>(),
        };

        reader
            .GetEligibleAsync(
                default!,
                default!,
                default)
            .ReturnsForAnyArgs(eligiblePackages);

        var result = await sut.ExecuteAsync(
            reader,
            default!,
            default!,
            default);

        result
            .Should()
            .BeOfType<NotFoundResult>();
    }

    [Theory, AutoNSubstituteData]
    public async Task ExecuteAsync_Calls_Reader_With_Correct_Parameters(
        [Frozen] IProductReader reader,
        GetEligibleFirmwareUpdateEndpoint sut,
        string version,
        string vendor,
        CancellationToken cancellationToken)
    {
        await sut.ExecuteAsync(
            reader,
            version,
            vendor,
            cancellationToken);

        _ = reader
            .Received(1)
            .GetEligibleAsync(version, vendor, cancellationToken);
    }
}
