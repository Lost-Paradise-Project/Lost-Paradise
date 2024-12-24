using Content.Shared._LostParadise.Salvage;

namespace Content.Server._LostParadise.Salvage.Expeditions.Structure;

/// <summary>
/// Tracks expedition data for <see cref="SalvageMissionType.Structure"/>
/// </summary>
[RegisterComponent, Access(typeof(LPSalvageSystem), typeof(SpawnLPSalvageMissionJob))]
public sealed partial class LPSalvageStructureExpeditionComponent : Component
{
    [DataField("structures")]
    public List<EntityUid> Structures = new();
}
