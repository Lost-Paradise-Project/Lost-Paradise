using Robust.Shared.Prototypes;
using Content.Shared.Actions;
using Content.Shared.Polymorph;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;
using Robust.Shared.Serialization;

namespace Content.Shared._LostParadise.AngelDust.Components;

/// <summary>
///     Доступ к панели РП взаимодействий
/// </summary>

[RegisterComponent]
public sealed partial class AngelDustComponent : Component
{
    [DataField]
    public EntProtoId TestAction = "LPPAngelDustPolymorph";

    [DataField("morph")]
    public ProtoId<PolymorphPrototype> AngelDustPolymorphId = "LPPAngelDustYukiDeMorph";

    [DataField]
    public EntityUid? Action;
}

[Serializable]
public sealed partial class AngelDustPolyEvent : InstantActionEvent { }
