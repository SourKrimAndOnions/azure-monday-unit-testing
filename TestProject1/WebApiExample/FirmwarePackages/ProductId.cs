using Atc.Cosmos.EventStore.Cqrs;

namespace Clever.Firmware.Domain.FirmwarePackages;

public sealed class ProductId : EventStreamId, IEquatable<ProductId?>
{
    public const string TypeName = "firmware-product";
    public const string FilterIncludeAllEvents = TypeName + ".*";

    public ProductId(string productId)
        : base(TypeName, productId)
        => Id = productId;

    public ProductId(EventStreamId id)
        : base(id.Parts.ToArray())
        => Id = id.Parts[1];

    public string Id { get; }

    public override bool Equals(object? obj)
        => Equals(obj as ProductId);

    public bool Equals(ProductId? other)
        => other != null && Value == other.Value;

    public override int GetHashCode()
        => HashCode.Combine(Value);
}
