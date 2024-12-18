using Content.Shared.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Explosion.EntitySystems;
using Content.Shared.Mobs;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Server.Popups;
using Content.Shared.Database;
using Content.Shared.Popups;
using Robust.Shared.Player;

namespace Content.Server._LostParadise.Salvage;

public sealed class LPSalvageMobRestrictionsSystem : EntitySystem
{
    [Dependency] private readonly BodySystem _body = default!;
    [Dependency] private readonly ExplosionSystem _explosion = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LPSalvageMobRestrictionsComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<LPSalvageMobRestrictionsComponent, ComponentRemove>(OnRemove);
        SubscribeLocalEvent<LPSalvageMobRestrictionsGridComponent, ComponentRemove>(OnRemoveGrid);
        SubscribeLocalEvent<LPSalvageMobRestrictionsComponent, MobStateChangedEvent>(OnMobState);
        SubscribeLocalEvent<LPSalvageMobRestrictionsComponent, EntParentChangedMessage>(OnParentChanged);
    }

    private void OnInit(EntityUid uid, LPSalvageMobRestrictionsComponent component, ComponentInit args)
    {
        var gridUid = Transform(uid).ParentUid;
        if (!EntityManager.EntityExists(gridUid))
        {
            // Give up, we were spawned improperly
            return;
        }
        // When this code runs, the system hasn't actually gotten ahold of the grid entity yet.
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
        }
    }

    private void OnMobState(EntityUid uid, LPSalvageMobRestrictionsComponent component, MobStateChangedEvent args)
    {
        // If this entity is being destroyed, no need to fiddle with components
        if (Terminating(uid))
            return;

        if (args.NewMobState == MobState.Dead)
        {
            EntityManager.AddComponents(uid, component.AddComponentsOnDeath);
            EntityManager.RemoveComponents(uid, component.RemoveComponentsOnDeath);
        }
        else if (args.OldMobState == MobState.Dead)
        {
            EntityManager.AddComponents(uid, component.AddComponentsOnRevival);
            EntityManager.RemoveComponents(uid, component.RemoveComponentsOnRevival);
        }
    }

    private void OnParentChanged(EntityUid uid, LPSalvageMobRestrictionsComponent component, ref EntParentChangedMessage args)
    {
        // If this entity is being destroyed, no need to fiddle with components
        if (Terminating(uid))
            return;

        var gridUid = Transform(uid).GridUid;
        var popupMessage = Loc.GetString(component.LeaveGridPopup);

        if (component.LinkedGridEntity == gridUid && HasComp<LPSalvageMobRestrictionsGridComponent>(gridUid))
        {
            EntityManager.AddComponents(uid, component.AddComponentsReturnGrid);
            EntityManager.RemoveComponents(uid, component.RemoveComponentsReturnGrid);

            if (!EntityManager.TryGetComponent(uid, out ActorComponent? actor))
                return;

            if (actor.PlayerSession.AttachedEntity == null)
                return;

            if (component.DespawnIfOffLinkedGrid)
                _adminLogger.Add(LogType.AdminMessage, LogImpact.Low, $"{ToPrettyString(actor.PlayerSession.AttachedEntity.Value):player} returned to dungeon grid");
        }
        else
        {
            EntityManager.AddComponents(uid, component.AddComponentsLeaveGrid);
            EntityManager.RemoveComponents(uid, component.RemoveComponentsLeaveGrid);

            if (!EntityManager.TryGetComponent(uid, out ActorComponent? actor))
                return;

            if (actor.PlayerSession.AttachedEntity == null)
                return;

            if (component.DespawnIfOffLinkedGrid)
            {
                _adminLogger.Add(LogType.AdminMessage, LogImpact.Low, $"{ToPrettyString(actor.PlayerSession.AttachedEntity.Value):player} left the dungeon grid");
                _popupSystem.PopupEntity(popupMessage, actor.PlayerSession.AttachedEntity.Value, actor.PlayerSession, PopupType.MediumCaution);
            }
        }
    }

    // Returns true if the given entity is invalid or terminating
    private bool Terminating(EntityUid uid)
    {
        return !TryComp(uid, out MetaDataComponent? meta) || meta.EntityLifeStage >= EntityLifeStage.Terminating;
    }
}

