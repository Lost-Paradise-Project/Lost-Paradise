using Content.Shared.Damage;
using Content.Shared.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Explosion.EntitySystems;

namespace Content.Server._LostParadise.Salvage;

public sealed class LPSalvageMobRestrictionsSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;
    [Dependency] private readonly BodySystem _body = default!;
    [Dependency] private readonly ExplosionSystem _explosion = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LPSalvageMobRestrictionsComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<LPSalvageMobRestrictionsComponent, ComponentRemove>(OnRemove);
        SubscribeLocalEvent<LPSalvageMobRestrictionsGridComponent, ComponentRemove>(OnRemoveGrid);
    }

    private void OnInit(EntityUid uid, LPSalvageMobRestrictionsComponent component, ComponentInit args)
    {
        var gridUid = Transform(uid).ParentUid;
        if (!EntityManager.EntityExists(gridUid))
        {
            // Give up, we were spawned improperly
            return;
        }
        // When this code runs, the salvage magnet hasn't actually gotten ahold of the entity yet.
        // So it therefore isn't in a position to do this.
        if (!TryComp(gridUid, out LPSalvageMobRestrictionsGridComponent? rg))
        {
            rg = AddComp<LPSalvageMobRestrictionsGridComponent>(gridUid);
        }
        rg!.MobsToKill.Add(uid);
        component.LinkedGridEntity = gridUid;
    }

    private void OnRemove(EntityUid uid, LPSalvageMobRestrictionsComponent component, ComponentRemove args)
    {
        if (TryComp(component.LinkedGridEntity, out LPSalvageMobRestrictionsGridComponent? rg))
        {
            rg.MobsToKill.Remove(uid);
        }
    }

    private void OnRemoveGrid(EntityUid uid, LPSalvageMobRestrictionsGridComponent component, ComponentRemove args)
    {
        foreach (EntityUid target in component.MobsToKill)
        {
            // Don't destroy yourself, don't destroy things being destroyed.
            if (uid == target || MetaData(target).EntityLifeStage >= EntityLifeStage.Terminating)
                continue;

            if (TryComp(target, out BodyComponent? body))
            {
                // Creates a pool of blood on death, but remove the organs.
                var gibs = _body.GibBody(target, body: body, gibOrgans: true);
                foreach (var gib in gibs)
                    Del(gib);
            }
            else
            {
                // No body, probably a robot - explode it and delete the body
                _explosion.QueueExplosion(target, ExplosionSystem.DefaultExplosionPrototypeId, 5, 10, 5);
                Del(target);
            }
            // Old implementation
            //else if (TryComp(target, out DamageableComponent? dc))
            //{
            //    _damageableSystem.SetAllDamage(target, dc, 200);
            //}
        }
    }
}

