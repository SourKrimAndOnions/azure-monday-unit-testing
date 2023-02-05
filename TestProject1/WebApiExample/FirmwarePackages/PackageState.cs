namespace Clever.Firmware.Contracts.FirmwarePackages;

public enum PackageState
{
    /// <summary>
    /// New firmware package without binary.
    /// </summary>
    Draft,

    /// <summary>
    /// Firmware package with binary ready for use.
    /// </summary>
    Ready,

    /// <summary>
    /// Firmware package approved for roll-out.
    /// </summary>
    Approved,

    /// <summary>
    /// Firmware package no longer in use.
    /// </summary>
    Retired,
}