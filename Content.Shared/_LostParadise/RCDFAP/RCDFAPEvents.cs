using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._LostParadise.RCDFAP;

[Serializable, NetSerializable]
public sealed class RCDFAPSystemMessage : BoundUserInterfaceMessage
{
    public ProtoId<RCDFAPPrototype> ProtoId;

    public RCDFAPSystemMessage(ProtoId<RCDFAPPrototype> protoId)
    {
        ProtoId = protoId;
    }
}

[Serializable, NetSerializable]
public sealed class RCDFAPConstructionGhostRotationEvent : EntityEventArgs
{
    public readonly NetEntity NetEntity;
    public readonly Direction Direction;

    public RCDFAPConstructionGhostRotationEvent(NetEntity netEntity, Direction direction)
    {
        NetEntity = netEntity;
        Direction = direction;
    }
}

[Serializable, NetSerializable]
public enum RcdfapUiKey : byte
{
    Key
}
