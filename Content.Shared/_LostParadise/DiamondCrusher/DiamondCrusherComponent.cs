using Content.Shared.Damage;
using Content.Shared.Stacks;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._LostParadise.DiamondCrusher;

/// <summary>
/// Это хранилище сущностей, которое при активации сжимает (не обработанные)алмазы внутри себя и выдает (обработанные)алмазы.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedDiamondCrusherSystem))]
public sealed partial class DiamondCrusherComponent : Component
{
    /// <summary>
    /// Независимо от того, находится ли дробилка в данный момент в процессе дробления чего-либо или нет.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Crushing;

    /// <summary>
    /// Когда закончится текущее дробление.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan CrushEndTime;

    /// <summary>
    /// В следующую секунду. Используется для нанесения урона с течением времени.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan NextSecond;

    /// <summary>
    /// Общая продолжительность дробления.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public TimeSpan CrushDuration = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Вайтлист предметов для которых, после дробления, будет давать фрагменты.
    /// </summary>
    [DataField]
    public EntityWhitelist CrushingWhitelist = new();

    /// <summary>
    /// Минимальное число фрагментов.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public int MinFragments = 5;

    /// <summary>
    /// Максимальное число фрагментов, не включительно
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public int MaxFragments = 15;

    /// <summary>
    /// Какой прототип будет являтся результатом дробления
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<StackPrototype> FragmentStackProtoId = "LPPProcessedDiamond";

    /// <summary>
    /// Контейнер, используемый для хранения осколков и крошки от дробления.
    /// </summary>
    [ViewVariables]
    public Container OutputContainer;

    /// <summary>
    /// Айди для <see cref="OutputContainer"/>
    /// </summary>
    [DataField]
    public string OutputContainerName = "output_container";

    /// <summary>
    /// Урон, наносимый каждую секунду существам, находящимся внутри, во время дробления.
    /// </summary>
    [DataField]
    public DamageSpecifier CrushingDamage = new();

    /// <summary>
    /// Звук воспроизводится в конце успешной обработки.
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier? CrushingCompleteSound = new SoundCollectionSpecifier("MetalCrunch");

    /// <summary>
    /// Звук воспроизводится на протяжении всего процесса дробления. Отключается, если он закончился раньше времени.
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier? CrushingSound = new SoundPathSpecifier("/Audio/Effects/hydraulic_press.ogg");

    /// <summary>
    /// Хранит сущность <see cref="CrushingSound"/> чтобы позволить покончить с этим пораньше.
    /// </summary>
    [DataField]
    public (EntityUid, AudioComponent)? CrushingSoundEntity;

    /// <summary>
    /// Когда этот параметр включен, устройство для обработки алмазов не открывается во время их измельчения.
    /// </summary>
    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public bool AutoLock = false;
}

[Serializable, NetSerializable]
public enum DiamondCrusherVisuals : byte
{
    Crushing
}
