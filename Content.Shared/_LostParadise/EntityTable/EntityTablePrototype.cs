using Content.Shared._LostParadise.EntityTable.EntitySelectors;
using Robust.Shared.Prototypes;

namespace Content.Shared._LostParadise.EntityTable;

/// <summary>
/// This is a prototype for...
/// </summary>
[Prototype]
public sealed partial class LPEntityTablePrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; } = default!;

    [DataField(required: true)]
    public LPEntityTableSelector Table = default!;
}
