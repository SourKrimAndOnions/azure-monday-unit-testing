namespace Clever.Firmware.Api.Endpoints;

public static class AuthRoles
{
    public const string FirmwareAdministrator
        = "firmware.readwrite.all";

    public const string FirmwareAll
        = "firmware.readwrite.all,firmware.read.all";
}
