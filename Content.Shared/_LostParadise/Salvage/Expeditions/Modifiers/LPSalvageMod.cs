using Robust.Shared.Prototypes;

namespace Content.Shared._LostParadise.Salvage.Expeditions.Modifiers;

/// <summary>
/// Generic modifiers with no additional data
/// </summary>
[Prototype("LPsalvageMod")]
public sealed partial class LPSalvageMod : IPrototype, ILPSalvageMod
{
    [IdDataField] public string ID { get; } = default!;

    [DataField("desc")] public LocId Description { get; private set; } = string.Empty;

    /// <summary>
    /// Cost for difficulty modifiers.
    /// </summary>
    [DataField("cost")]
    public float Cost { get; private set; } = 0f;
}
