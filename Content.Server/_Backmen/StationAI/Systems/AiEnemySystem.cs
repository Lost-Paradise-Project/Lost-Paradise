using Content.Server.NPC.Components;
using Content.Server.NPC.Systems;
using Content.Shared._Backmen.StationAI;
using Content.Shared._Backmen.StationAI.Components;
using Content.Shared._Backmen.StationAI.Systems;
using Content.Shared.Examine;
using Content.Shared.Mobs.Systems;
using Content.Shared.NPC;
using Content.Shared.Verbs;

namespace Content.Server._Backmen.StationAI.Systems;

public sealed class AiEnemySystem : SharedAiEnemySystem
{
    [Dependency] private readonly NpcFactionSystem _faction = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AIEnemyNTComponent, MapInitEvent>(OnAdd);
        SubscribeLocalEvent<AIEnemyNTComponent, ComponentShutdown>(OnRemove);
    }

    protected override void ToggleEnemy(EntityUid u, EntityUid target)
    {
        if (!EntityQuery.HasComponent(u))
            return;
        if (!HasComp<StationAIComponent>(u))
            return;

        if (_mobState.IsDead(u))
            return;

        var core = u;
        if (TryComp<AIEyeComponent>(core, out var eyeComponent))
        {
            if (eyeComponent.AiCore == null)
                return;
            core = eyeComponent.AiCore.Value.Owner;
        }

        if (!core.Valid)
        {
            return;
        }

        var xform = Transform(core);
        if (xform.GridUid != Transform(target).GridUid || !xform.Anchored)
        {
            return;
        }

        if (HasComp<AIEnemyNTComponent>(target))
            RemCompDeferred<AIEnemyNTComponent>(target);
        else
            EnsureComp<AIEnemyNTComponent>(target).Source = core;
    }

    [ValidatePrototypeId<NpcFactionPrototype>]
    private const string AiEnemyFaction = "AiEnemy";

    private void OnRemove(Entity<AIEnemyNTComponent> ent, ref ComponentShutdown args)
    {
        if (TryComp<NpcFactionMemberComponent>(ent, out var npcFactionMemberComponent))
        {
            _faction.RemoveFaction(ent.Owner, AiEnemyFaction);
        }

    }

    private void OnAdd(Entity<AIEnemyNTComponent> ent, ref MapInitEvent args)
    {
        if (TryComp<NpcFactionMemberComponent>(ent, out var npcFactionMemberComponent))
        {
            _faction.AddFaction(ent.Owner, AiEnemyFaction);
        }
    }
}
