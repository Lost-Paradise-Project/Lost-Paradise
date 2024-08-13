using Content.Shared.Actions;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Backmen.StationAI.Events;
public sealed partial class ToggleArmNukeEvent : InstantActionEvent
{

}

public sealed partial class InnateAfterInteractActionEvent : EntityTargetActionEvent
{
    [DataField("item", required:true)]
    public EntProtoId Item;
}

public sealed partial class InnateBeforeInteractActionEvent : EntityTargetActionEvent
{
    [DataField("item", required:true)]
    public EntProtoId Item;
}
