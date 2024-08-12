using Content.Shared._LostParadise.RCDFAP.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Physics;
using Robust.Shared.Prototypes;

namespace Content.Shared._LostParadise.RCDFAP.Components;

/// <summary>
/// Main component for the RCDFAP
/// Optionally uses LimitedChargesComponent.
/// Charges can be refilled with RCDFAP ammo
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(RCDFAPSystem))]
public sealed partial class RCDFAPComponent : Component
{
    /// <summary>
    /// List of RCDFAP prototypes that the device comes loaded with
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<ProtoId<RCDFAPPrototype>> AvailablePrototypes { get; set; } = new();

    /// <summary>
    /// Sound that plays when a RCDFAP operation successfully completes
    /// </summary>
    [DataField]
    public SoundSpecifier SuccessSound { get; set; } = new SoundPathSpecifier("/Audio/Items/deconstruct.ogg");

    /// <summary>
    /// The ProtoId of the currently selected RCDFAP prototype
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<RCDFAPPrototype> ProtoId { get; set; } = "Invalid";

    /// <summary>
    /// A cached copy of currently selected RCDFAP prototype
    /// </summary>
    /// <remarks>
    /// If the ProtoId is changed, make sure to update the CachedPrototype as well
    /// </remarks>
    [ViewVariables(VVAccess.ReadOnly)]
    public RCDFAPPrototype CachedPrototype { get; set; } = default!;

    /// <summary>
    /// The direction constructed entities will face upon spawning
    /// </summary>
    [DataField, AutoNetworkedField]
    public Direction ConstructionDirection
    {
        get
        {
            return _constructionDirection;
        }
        set
        {
            _constructionDirection = value;
            ConstructionTransform = new Transform(new(), _constructionDirection.ToAngle());
        }
    }

    private Direction _constructionDirection = Direction.South;

    /// <summary>
    /// Returns a rotated transform based on the specified ConstructionDirection
    /// </summary>
    /// <remarks>
    /// Contains no position data
    /// </remarks>
    [ViewVariables(VVAccess.ReadOnly)]
    public Transform ConstructionTransform { get; private set; } = default!;
}
