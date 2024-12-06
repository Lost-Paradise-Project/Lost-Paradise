using Content.Server.Explosion.EntitySystems;
using Robust.Shared.Audio.Systems;
using Content.Shared.Timing;

namespace Content.Server.IgnitionSource;

/// <summary>
/// Handles toggling ignition on or off, with sound only for activation.
/// </summary>
public sealed class IgniteOnToggleSystem : EntitySystem
{
    [Dependency] private readonly IgnitionSourceSystem _source = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly UseDelaySystem _useDelay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IgniteOnToggleComponent, TriggerEvent>(OnTrigger);
    }

    private void OnTrigger(Entity<IgniteOnToggleComponent> ent, ref TriggerEvent args)
    {
        if (!TryComp(ent.Owner, out UseDelayComponent? useDelay) || _useDelay.IsDelayed((ent.Owner, useDelay)))
            return;

        if (!TryComp(ent.Owner, out IgnitionSourceComponent? source))
            return;

        var newState = !source.Ignited;
        _source.SetIgnited((ent.Owner, source), newState);

        if (newState)
        {
            _audio.PlayPvs(ent.Comp.IgniteSound, ent.Owner);
        }

        _useDelay.TryResetDelay((ent.Owner, useDelay));
    }
}
