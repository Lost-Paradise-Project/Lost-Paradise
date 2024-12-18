using Content.Shared._LostParadise.Salvage.Expeditions.Modifiers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared._LostParadise.Salvage.Expeditions;

[Prototype("LPsalvageFaction")]
public sealed partial class LPSalvageFactionPrototype : IPrototype, ILPSalvageMod
{
    [IdDataField] public string ID { get; } = default!;

    [DataField("desc")] public LocId Description { get; private set; } = string.Empty;

    /// <summary>
    /// Cost for difficulty modifiers.
    /// </summary>
    [DataField("cost")]
    public float Cost { get; private set; } = 0f;

    [ViewVariables(VVAccess.ReadWrite), DataField("groups", required: true)]
    public List<LPSalvageMobGroup> MobGroups = default!;

    /// <summary>
    /// Miscellaneous data for factions.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("configs")]
    public Dictionary<string, string> Configs = new();
}
