using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._LostParadise.Salvage.Expeditions;

[NetworkedComponent]
public abstract partial class SharedLPSalvageExpeditionComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("stage")]
    public LPExpeditionStage Stage = LPExpeditionStage.Added;
}

[Serializable, NetSerializable]
public sealed class LPSalvageExpeditionComponentState : ComponentState
{
    public LPExpeditionStage Stage;
}
