using Content.Client.Audio;
using Content.Shared._LostParadise.Salvage;
using Content.Shared._LostParadise.Salvage.Expeditions;
using Robust.Client.Player;
using Robust.Shared.GameStates;

namespace Content.Client._LostParadise.Salvage;

public sealed class LPSalvageSystem : SharedLPSalvageSystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly ContentAudioSystem _audio = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PlayAmbientMusicEvent>(OnPlayAmbientMusic);
        SubscribeLocalEvent<LPSalvageExpeditionComponent, ComponentHandleState>(OnExpeditionHandleState);
    }

    private void OnExpeditionHandleState(EntityUid uid, LPSalvageExpeditionComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not LPSalvageExpeditionComponentState state)
            return;

        component.Stage = state.Stage;

        if (component.Stage >= LPExpeditionStage.MusicCountdown)
        {
            _audio.DisableAmbientMusic();
        }
    }

    private void OnPlayAmbientMusic(ref PlayAmbientMusicEvent ev)
    {
        if (ev.Cancelled)
            return;

        var player = _playerManager.LocalEntity;

        if (!TryComp(player, out TransformComponent? xform) ||
            !TryComp<LPSalvageExpeditionComponent>(xform.MapUid, out var expedition) ||
            expedition.Stage < LPExpeditionStage.MusicCountdown)
        {
            return;
        }

        ev.Cancelled = true;
    }
}
