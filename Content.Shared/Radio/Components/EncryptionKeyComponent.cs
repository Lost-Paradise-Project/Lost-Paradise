using Content.Shared.Chat;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Shared.Radio.Components;

/// <summary>
///     This component is currently used for providing access to channels for "HeadsetComponent"s.
///     It should be used for intercoms and other radios in future.
/// </summary>
[RegisterComponent]
public sealed partial class EncryptionKeyComponent : Component
{
    [DataField("channels", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<RadioChannelPrototype>))]
    public HashSet<string> Channels = new();

    /// <summary>
    ///     This is the channel that will be used when using the default/department prefix (<see cref="SharedChatSystem.DefaultChannelKey"/>).
    /// </summary>
    [DataField("defaultChannel", customTypeSerializer: typeof(PrototypeIdSerializer<RadioChannelPrototype>))]
    public string? DefaultChannel;

    // IPC-Start
    /// <summary>
    ///     Whether or not the headset can be examined to see the encryption keys while the keys aren't accessible.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("examineWhileLocked")]
    public bool ExamineWhileLocked = true;

    /// <summary>
    ///     Whether or not encryption keys can be removed from the headset.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("keysUnlocked")]
    public bool KeysUnlocked = true;
    // IPC-End
}
