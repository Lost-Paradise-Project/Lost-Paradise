using System.Linq;
using System.Threading;
using Content.Server.Cargo.Components;
using Content.Server.Cargo.Systems;
using Content.Server._LostParadise.Salvage;
using Content.Server._LostParadise.Salvage.Expeditions;
using Content.Server._LostParadise.Salvage.Expeditions.Structure;
using Content.Shared._LostParadise.CCVar;
using Content.Shared.Examine;
using Content.Shared.Random.Helpers;
using Content.Shared._LostParadise.Salvage.Expeditions;
using Robust.Shared.Audio;
using Robust.Shared.CPUJob.JobQueues;
using Robust.Shared.CPUJob.JobQueues.Queues;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Shared.Coordinates;
using Content.Shared.Procedural;
using System.Linq;
using System.Threading;
using Content.Shared._LostParadise.Salvage;
using Content.Shared._LostParadise.Salvage.Expeditions;
using Robust.Shared.GameStates;
using Robust.Shared.Random;
using Robust.Shared.Map;
using Content.Shared.Shuttles.Components; // Frontier

namespace Content.Server._LostParadise.Salvage;

public sealed partial class LPSalvageSystem
{
    /*
     * Handles setup / teardown of salvage expeditions.
     */

    private const int MissionLimit = 5;
    [Dependency] private readonly StationSystem _stationSystem = default!;

    private readonly JobQueue _salvageQueue = new();
    private readonly List<(SpawnLPSalvageMissionJob Job, CancellationTokenSource CancelToken)> _salvageJobs = new();
    private const double SalvageJobTime = 0.002;

    private float _cooldown;
    private float _failedCooldown;

    private void InitializeExpeditions()
    {
        SubscribeLocalEvent<LPSalvageExpeditionConsoleComponent, ComponentInit>(OnSalvageConsoleInit);
        SubscribeLocalEvent<LPSalvageExpeditionConsoleComponent, EntParentChangedMessage>(OnSalvageConsoleParent);
        SubscribeLocalEvent<LPSalvageExpeditionConsoleComponent, ClaimLPSalvageMessage>(OnSalvageClaimMessage);
        SubscribeLocalEvent<LPSalvageExpeditionDataComponent, LPExpeditionSpawnCompleteEvent>(OnExpeditionSpawnComplete); // Frontier: more gracefully handle expedition generation failures
        SubscribeLocalEvent<LPSalvageExpeditionConsoleComponent, FinishLPSalvageMessage>(OnSalvageFinishMessage); // Frontier: For early finish

        SubscribeLocalEvent<LPSalvageExpeditionComponent, MapInitEvent>(OnExpeditionMapInit);
//        SubscribeLocalEvent<SalvageExpeditionDataComponent, EntityUnpausedEvent>(OnDataUnpaused); // Frontier

        SubscribeLocalEvent<LPSalvageExpeditionComponent, ComponentShutdown>(OnExpeditionShutdown);
//        SubscribeLocalEvent<SalvageExpeditionComponent, EntityUnpausedEvent>(OnExpeditionUnpaused); // Frontier
        SubscribeLocalEvent<LPSalvageExpeditionComponent, ComponentGetState>(OnExpeditionGetState);

        SubscribeLocalEvent<LPSalvageStructureComponent, ExaminedEvent>(OnStructureExamine);

        Subs.CVar(_configurationManager, AccVars.SalvageExpeditionCooldown, SetCooldownChange, true); // Frontier
        Subs.CVar(_configurationManager, AccVars.SalvageExpeditionFailedCooldown, SetFailedCooldownChange, true); // Frontier
    }

    private void OnExpeditionGetState(EntityUid uid, LPSalvageExpeditionComponent component, ref ComponentGetState args)
    {
        args.State = new LPSalvageExpeditionComponentState()
        {
            Stage = component.Stage
        };
    }

    // Frontier
    private void ShutdownExpeditions()
    {
        _configurationManager.UnsubValueChanged(AccVars.SalvageExpeditionCooldown, SetCooldownChange);
        _configurationManager.UnsubValueChanged(AccVars.SalvageExpeditionFailedCooldown, SetFailedCooldownChange);
    }
    // End Frontier

    private void SetCooldownChange(float obj)
    {
        // Update the active cooldowns if we change it.
        var diff = obj - _cooldown;

        var query = AllEntityQuery<LPSalvageExpeditionDataComponent>();

        while (query.MoveNext(out var comp))
        {
            comp.NextOffer += TimeSpan.FromSeconds(diff);
        }

        _cooldown = obj;
    }

    private void SetFailedCooldownChange(float obj)
    {
        var diff = obj - _failedCooldown;

        var query = AllEntityQuery<LPSalvageExpeditionDataComponent>();

        while (query.MoveNext(out var comp))
        {
            comp.NextOffer += TimeSpan.FromSeconds(diff);
        }

        _failedCooldown = obj;
    }

    private void OnExpeditionMapInit(EntityUid uid, LPSalvageExpeditionComponent component, MapInitEvent args)
    {
        var selectedFile = _audio.GetSound(component.Sound);
        component.SelectedSong = new SoundPathSpecifier(selectedFile, component.Sound.Params);
    }

    private void OnExpeditionShutdown(EntityUid uid, LPSalvageExpeditionComponent component, ComponentShutdown args)
    {
        component.Stream = _audio.Stop(component.Stream);

        foreach (var (job, cancelToken) in _salvageJobs.ToArray())
        {
            if (job.Station == component.Station)
            {
                cancelToken.Cancel();
                _salvageJobs.Remove((job, cancelToken));
            }
        }

        if (Deleted(component.Station))
            return;

        // Finish mission
        if (TryComp<LPSalvageExpeditionDataComponent>(component.Station, out var data))
        {
            FinishExpedition(data, uid, component, component.Station); // Frontier: null<component.Station
        }
    }

    private void OnDataUnpaused(EntityUid uid, LPSalvageExpeditionDataComponent component, ref EntityUnpausedEvent args)
    {
        component.NextOffer += args.PausedTime;
    }

    private void OnExpeditionUnpaused(EntityUid uid, LPSalvageExpeditionComponent component, ref EntityUnpausedEvent args)
    {
        component.EndTime += args.PausedTime;
    }

    private void UpdateExpeditions()
    {
        var currentTime = _timing.CurTime;
        _salvageQueue.Process();

        foreach (var (job, cancelToken) in _salvageJobs.ToArray())
        {
            switch (job.Status)
            {
                case JobStatus.Finished:
                    _salvageJobs.Remove((job, cancelToken));
                    break;
            }
        }

        var query = EntityQueryEnumerator<LPSalvageExpeditionDataComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            // Update offers
            if (comp.NextOffer > currentTime || comp.Claimed)
                continue;

            //comp.NextOffer += TimeSpan.FromSeconds(_cooldown); // Frontier
            comp.NextOffer = currentTime + TimeSpan.FromSeconds(_cooldown); // Frontier
            GenerateMissions(comp);
            UpdateConsoles(uid, comp);
        }
    }

    private void FinishExpedition(LPSalvageExpeditionDataComponent component, EntityUid uid, LPSalvageExpeditionComponent expedition, EntityUid? shuttle)
    {
        component.NextOffer = _timing.CurTime + TimeSpan.FromSeconds(_cooldown);
        Announce(uid, Loc.GetString("salvage-expedition-mission-completed"));
        // Finish mission cleanup.
        switch (expedition.MissionParams.MissionType)
        {
            // Handles the mining taxation.
            case LPSalvageMissionType.Mining:
                expedition.Completed = true;

                if (shuttle != null && TryComp<LPSalvageMiningExpeditionComponent>(uid, out var mining))
                {
                    var xformQuery = GetEntityQuery<TransformComponent>();
                    var entities = new List<EntityUid>();
                    MiningTax(entities, shuttle.Value, mining, xformQuery);

                    var tax = GetMiningTax(expedition.MissionParams.Difficulty);
                    _random.Shuffle(entities);

                    // TODO: urgh this pr is already taking so long I'll do this later
                    for (var i = 0; i < Math.Ceiling(entities.Count * tax); i++)
                    {
                        // QueueDel(entities[i]);
                    }
                }

                break;
        }

        // Handle payout after expedition has finished
        if (expedition.Completed)
        {
            Log.Debug($"Completed mission {expedition.MissionParams.MissionType} with seed {expedition.MissionParams.Seed}");
            component.NextOffer = _timing.CurTime + TimeSpan.FromSeconds(_cooldown);
            Announce(uid, Loc.GetString("salvage-expedition-mission-completed"));
            GiveRewards(expedition);
        }
        else
        {
            Log.Debug($"Failed mission {expedition.MissionParams.MissionType} with seed {expedition.MissionParams.Seed}");
            component.NextOffer = _timing.CurTime + TimeSpan.FromSeconds(_failedCooldown);
            Announce(uid, Loc.GetString("salvage-expedition-mission-failed"));
        }


        component.ActiveMission = 0;
        component.Cooldown = true;
        if (shuttle != null) // Frontier
            UpdateConsoles(shuttle.Value, component); // Frontier
    }

    /// <summary>
    /// Deducts ore tax for mining.
    /// </summary>
    private void MiningTax(List<EntityUid> entities, EntityUid entity, LPSalvageMiningExpeditionComponent mining, EntityQuery<TransformComponent> xformQuery)
    {
        if (!mining.ExemptEntities.Contains(entity))
        {
            entities.Add(entity);
        }

        var xform = xformQuery.GetComponent(entity);
        var children = xform.ChildEnumerator;

        while (children.MoveNext(out var child))
        {
            MiningTax(entities, child, mining, xformQuery);
        }
    }

    private void GenerateMissions(LPSalvageExpeditionDataComponent component)
    {
        component.Missions.Clear();
        var configs = Enum.GetValues<LPSalvageMissionType>().ToList();

        // Temporarily removed coz it SUCKS
        configs.Remove(LPSalvageMissionType.Mining);

        // this doesn't support having more missions than types of ratings
        // but the previous system didn't do that either.
        var allDifficulties = Enum.GetValues<DifficultyRating>();
        _random.Shuffle(allDifficulties);
        var difficulties = allDifficulties.Take(MissionLimit).ToList();
        difficulties.Sort();

        if (configs.Count == 0)
            return;

        for (var i = 0; i < MissionLimit; i++)
        {
            _random.Shuffle(configs);
            var rating = difficulties[i];

            foreach (var config in configs)
            {
                var mission = new LPSalvageMissionParams
                {
                    Index = component.NextIndex,
                    MissionType = config,
                    Seed = _random.Next(),
                    Difficulty = rating,
                };

                component.Missions[component.NextIndex++] = mission;
                break;
            }
        }
    }

    private LPSalvageExpeditionConsoleState GetState(LPSalvageExpeditionDataComponent component)
    {
        var missions = component.Missions.Values.ToList();
        //return new SalvageExpeditionConsoleState(component.NextOffer, component.Claimed, component.Cooldown, component.ActiveMission, missions);
        return new LPSalvageExpeditionConsoleState(component.NextOffer, component.Claimed, component.Cooldown, component.CanFinish, component.ActiveMission, missions); // Frontier
    }

    private void SpawnMission(LPSalvageMissionParams missionParams, EntityUid station, EntityUid? coordinatesDisk)
    {
        var cancelToken = new CancellationTokenSource();
        var job = new SpawnLPSalvageMissionJob(
            SalvageJobTime,
            EntityManager,
            _timing,
            _mapManager,
            _prototypeManager,
            _anchorable,
            _biome,
            _dungeon,
            _shuttle,
            _stationSystem,
            _metaData,
            this,
            _transform,
            _mapSystem,
            station,
            coordinatesDisk,
            missionParams,
            cancelToken.Token);

        _salvageJobs.Add((job, cancelToken));
        _salvageQueue.EnqueueJob(job);
    }

    private void OnStructureExamine(EntityUid uid, LPSalvageStructureComponent component, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("salvage-expedition-structure-examine"));
    }

    private void GiveRewards(LPSalvageExpeditionComponent comp)
    {
        var palletList = new List<EntityUid>();
        var pallets = EntityQueryEnumerator<CargoPalletComponent>();
        while (pallets.MoveNext(out var pallet, out var palletComp))
        {
            if (_stationSystem.GetOwningStation(pallet) == comp.Station)
            {
                palletList.Add(pallet);
            }
        }

        if (!(palletList.Count > 0))
            return;

        foreach (var reward in comp.Rewards)
        {
            Spawn(reward, (Transform(_random.Pick(palletList)).MapPosition));
        }
    }

    // Frontier: handle exped spawn job failures gracefully - reset the console
    private void OnExpeditionSpawnComplete(EntityUid uid, LPSalvageExpeditionDataComponent component, LPExpeditionSpawnCompleteEvent ev)
    {
        if (component.ActiveMission == ev.MissionIndex && !ev.Success)
        {
            component.ActiveMission = 0;
            component.Cooldown = false;
            UpdateConsoles(uid, component);
        }
    }
    // End Frontier
}
