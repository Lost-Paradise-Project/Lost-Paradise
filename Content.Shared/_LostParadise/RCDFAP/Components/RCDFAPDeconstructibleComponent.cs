using Content.Shared._LostParadise.RCDFAP.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._LostParadise.RCDFAP.Components;

[RegisterComponent, NetworkedComponent]
[Access(typeof(RCDFAPSystem))]
public sealed partial class RCDFAPDeconstructableComponent : Component
{
    /// <summary>
    /// Number of charges consumed when the deconstruction is completed
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int Cost = 1;

    /// <summary>
    /// The length of the deconstruction
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Delay = 1f;

    /// <summary>
    /// The visual effect that plays during deconstruction
    /// </summary>
    [DataField("fx"), ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId? Effect = null;

    /// <summary>
    /// Toggles whether this entity is deconstructable or not
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Deconstructable = true;
}
