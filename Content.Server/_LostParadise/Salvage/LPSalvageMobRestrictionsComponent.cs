namespace Content.Server._LostParadise.Salvage;

/// <summary>
///     This component exists as a sort of stateful marker for a
///     killswitch meant to keep salvage mobs from doing stuff they
///     really shouldn't (attacking station).
///     The main thing is that adding this component ties the mob to
///     whatever it's currently parented to.
/// </summary>
[RegisterComponent]
public sealed partial class LPSalvageMobRestrictionsComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public EntityUid LinkedGridEntity = EntityUid.Invalid;

    /// <summary>
    /// If set to false, this mob will not be despawned when its linked entity is despawned.
    /// Useful for event ghost roles, for instance.
    /// </summary>
    [DataField]
    public bool DespawnIfOffLinkedGrid = true;
}
