using Content.Server.Station.Components;
using Content.Shared.Popups;
using Content.Shared.Shuttles.Components;
using Content.Shared._LostParadise.Salvage.Expeditions;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using Content.Server._LostParadise.Salvage.Expeditions; // Frontier
using Content.Shared._LostParadise.CCVar; // Frontier
using Content.Shared.Mind.Components; // Frontier
using Content.Shared.Mobs.Components; // Frontier
using Content.Shared.IdentityManagement; // Frontier
using Content.Shared.NPC; // Frontier
using Content.Server.NPC.Components;
using Content.Server._LostParadise.Salvage; // Frontier

namespace Content.Server._LostParadise.Salvage;

public sealed partial class LPSalvageSystem
{
    [ValidatePrototypeId<EntityPrototype>]
    public const string CoordinatesDisk = "CoordinatesDisk";

    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;

    private const float ShuttleFTLMassThreshold = 50f;
    private const float ShuttleFTLRange = 150f;

    private void OnSalvageClaimMessage(EntityUid uid, LPSalvageExpeditionConsoleComponent component, ClaimLPSalvageMessage args)
    {
        var station = _station.GetOwningStation(uid);

        // Frontier
        if (!TryComp<LPSalvageExpeditionDataComponent>(station, out var data) || data.Claimed) // Moved up before the active expedition count
            return;

        var activeExpeditionCount = 0;
        var expeditionQuery = AllEntityQuery<LPSalvageExpeditionDataComponent, MetaDataComponent>();
        while (expeditionQuery.MoveNext(out var expeditionUid, out _, out _))
            if (TryComp<LPSalvageExpeditionDataComponent>(expeditionUid, out var expeditionData) && expeditionData.Claimed)
                activeExpeditionCount++;

        if (activeExpeditionCount >= _configurationManager.GetCVar(AccVars.SalvageExpeditionMaxActive))
        {
            PlayDenySound(uid, component);
            _popupSystem.PopupEntity(Loc.GetString("shuttle-ftl-too-many"), uid, PopupType.MediumCaution);
            UpdateConsoles(station.Value, data);
            return;
        }
        // End Frontier

        if (!data.Missions.TryGetValue(args.Index, out var missionparams))
            return;

        // Frontier: FTL travel is currently restricted to expeditions and such, and so we need to put this here
        // until FTL changes for us in some way.
        if (!component.Debug) // Skip the test
        {
            if (!TryComp<StationDataComponent>(station, out var stationData))
                return;
            if (_station.GetLargestGrid(stationData) is not { Valid: true } grid)
                return;
            if (!TryComp<MapGridComponent>(grid, out var gridComp))
                return;

            var xform = Transform(grid);
            var bounds = xform.WorldMatrix.TransformBox(gridComp.LocalAABB).Enlarged(ShuttleFTLRange);
            var bodyQuery = GetEntityQuery<PhysicsComponent>();
            foreach (var other in _mapManager.FindGridsIntersecting(xform.MapID, bounds))
            {
                if (grid == other.Owner ||
                    !bodyQuery.TryGetComponent(other.Owner, out var body) ||
                    body.Mass < ShuttleFTLMassThreshold)
                {
                    continue;
                }

                PlayDenySound(uid, component);
                _popupSystem.PopupEntity(Loc.GetString("shuttle-ftl-proximity"), uid, PopupType.MediumCaution);
                UpdateConsoles(station.Value, data);
                return;
            }
            // end of Frontier proximity check
        }
        // End Frontier

        // Frontier  change - disable coordinate disks for expedition missions
        //var cdUid = Spawn(CoordinatesDisk, Transform(uid).Coordinates);
        SpawnMission(missionparams, station.Value, null);

        data.ActiveMission = args.Index;
        var mission = GetMission(missionparams.MissionType, missionparams.Difficulty, missionparams.Seed);
        data.NextOffer = _timing.CurTime + mission.Duration + TimeSpan.FromSeconds(1);

        // Frontier  change - disable coordinate disks for expedition missions
        //_labelSystem.Label(cdUid, GetFTLName(_prototypeManager.Index<DatasetPrototype>("names_borer"), missionparams.Seed));
        //_audio.PlayPvs(component.PrintSound, uid);

        UpdateConsoles(station.Value, data); // Frontier: add station
    }

    // Frontier: early expedition end
    private void OnSalvageFinishMessage(EntityUid entity, LPSalvageExpeditionConsoleComponent component, FinishLPSalvageMessage e)
    {
        var station = _station.GetOwningStation(entity);
        if (!TryComp<LPSalvageExpeditionDataComponent>(station, out var data) || !data.CanFinish)
            return;

        // Based on SalvageSystem.Runner:OnConsoleFTLAttempt
        if (!TryComp(entity, out TransformComponent? xform)) // Get the console's grid (if you move it, rip you)
        {
            PlayDenySound(entity, component);
            _popupSystem.PopupEntity(Loc.GetString("salvage-expedition-shuttle-not-found"), entity, PopupType.MediumCaution);
            UpdateConsoles(station.Value, data);
            return;
        }

        // Frontier: check if any player characters or friendly ghost roles are outside
        var query = EntityQueryEnumerator<MindContainerComponent, MobStateComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var mindContainer, out var _, out var mobXform))
        {
            if (mobXform.MapUid != xform.MapUid)
                continue;

            // Not player controlled (ghosted)
            if (!mindContainer.HasMind)
                continue;

            // NPC, definitely not a person
            if (HasComp<ActiveNPCComponent>(uid) || HasComp<LPSalvageMobRestrictionsComponent>(uid))
                continue;

            // Hostile ghost role, continue
            if (TryComp(uid, out NpcFactionMemberComponent? npcFaction))
            {
                var hostileFactions = npcFaction.HostileFactions;
                if (hostileFactions.Contains("NanoTrasen")) // Nasty - what if we need pirate expeditions?
                    continue;
            }

            // Okay they're on salvage, so are they on the shuttle.
            if (mobXform.GridUid != xform.GridUid)
            {
                PlayDenySound(entity, component);
                _popupSystem.PopupEntity(Loc.GetString("salvage-expedition-not-everyone-aboard", ("target", Identity.Entity(uid, EntityManager))), entity, PopupType.MediumCaution);
                UpdateConsoles(station.Value, data);
                return;
            }
        }
        // End SalvageSystem.Runner:OnConsoleFTLAttempt

        data.CanFinish = false;
        UpdateConsoles(station.Value, data);

        var map = Transform(entity).MapUid;

        if (!TryComp<LPSalvageExpeditionComponent>(map, out var expedition))
            return;

        const int departTime = 20;
        var newEndTime = _timing.CurTime + TimeSpan.FromSeconds(departTime);

        if (expedition.EndTime <= newEndTime)
            return;

        expedition.EndTime = newEndTime;
        expedition.Stage = LPExpeditionStage.FinalCountdown;

        Announce(map.Value, Loc.GetString("salvage-expedition-announcement-early-finish", ("departTime", departTime)));
    }
    // End Frontier: early expedition end

    private void OnSalvageConsoleInit(Entity<LPSalvageExpeditionConsoleComponent> console, ref ComponentInit args)
    {
        UpdateConsole(console);
    }

    private void OnSalvageConsoleParent(Entity<LPSalvageExpeditionConsoleComponent> console, ref EntParentChangedMessage args)
    {
        UpdateConsole(console);
    }

    private void UpdateConsoles(EntityUid stationUid, LPSalvageExpeditionDataComponent component)
    {
        var state = GetState(component);

        var query = AllEntityQuery<LPSalvageExpeditionConsoleComponent, UserInterfaceComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var uiComp, out var xform))
        {
            var station = _station.GetOwningStation(uid, xform);

            if (station != stationUid)
                continue;

            _ui.SetUiState((uid, uiComp), LPSalvageConsoleUiKey.Expedition, state);
        }
    }

    private void UpdateConsole(Entity<LPSalvageExpeditionConsoleComponent> component)
    {
        var station = _station.GetOwningStation(component);
        LPSalvageExpeditionConsoleState state;

        if (TryComp<LPSalvageExpeditionDataComponent>(station, out var dataComponent))
        {
            state = GetState(dataComponent);
        }
        else
        {
            state = new LPSalvageExpeditionConsoleState(TimeSpan.Zero, false, true, false, 0, new List<LPSalvageMissionParams>()); // Frontier: add false as 4th param
        }

        _ui.SetUiState(component.Owner, LPSalvageConsoleUiKey.Expedition, state);
    }

    private void PlayDenySound(EntityUid uid, LPSalvageExpeditionConsoleComponent component)
    {
        _audio.PlayPvs(_audio.GetSound(component.ErrorSound), uid);
    }
}
