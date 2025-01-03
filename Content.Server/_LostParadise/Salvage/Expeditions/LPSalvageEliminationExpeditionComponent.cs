using Content.Shared._LostParadise.Salvage;

namespace Content.Server._LostParadise.Salvage.Expeditions.Structure;

/// <summary>
/// Tracks expedition data for <see cref="SalvageMissionType.Elimination"/>
/// </summary>
[RegisterComponent, Access(typeof(LPSalvageSystem), typeof(SpawnLPSalvageMissionJob))]
public sealed partial class LPSalvageEliminationExpeditionComponent : Component
{
    /// <summary>
    /// List of mobs that need to be killed for the mission to be complete.
    /// </summary>
    [DataField("megafauna")]
    public List<EntityUid> Megafauna = new();
}
