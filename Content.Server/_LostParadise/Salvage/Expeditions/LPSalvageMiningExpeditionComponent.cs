using Content.Shared._LostParadise.Salvage;

namespace Content.Server._LostParadise.Salvage.Expeditions;

/// <summary>
/// Tracks expedition data for <see cref="SalvageMissionType.Mining"/>
/// </summary>
[RegisterComponent, Access(typeof(LPSalvageSystem))]
public sealed partial class LPSalvageMiningExpeditionComponent : Component
{
    /// <summary>
    /// Entities that were present on the shuttle and match the loot tax.
    /// </summary>
    [DataField("exemptEntities")]
    public List<EntityUid> ExemptEntities = new();
}
