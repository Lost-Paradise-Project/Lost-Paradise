using Content.Shared.Storage;

namespace Content.Shared._LostParadise.Salvage.Expeditions;

[DataDefinition]
public partial record struct LPSalvageMobGroup()
{
    // A mob may be cheap but rare or expensive but frequent.

    /// <summary>
    /// Probability to spawn this group. Summed with everything else for the faction.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("prob")]
    public float Prob = 1f;

    [ViewVariables(VVAccess.ReadWrite), DataField("entries", required: true)]
    public List<EntitySpawnEntry> Entries = new();
}
