using Robust.Shared.Audio;

namespace Content.Server.IgnitionSource;

/// <summary>
/// Toggles ignition state when triggered, with sound only for activation.
/// Requires <see cref="IgnitionSourceComponent"/>.
/// </summary>
[RegisterComponent, Access(typeof(IgniteOnToggleSystem))]
public sealed partial class IgniteOnToggleComponent : Component
{
    /// <summary>
    /// Sound to play when toggling on.
    /// </summary>
    [DataField]
    public SoundSpecifier IgniteSound = new SoundCollectionSpecifier("WelderOn");
}
