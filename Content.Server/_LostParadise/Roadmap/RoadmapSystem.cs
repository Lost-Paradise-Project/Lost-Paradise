using Content.Shared.Administration.Logs;
using Content.Shared._LostParadise.Roadmap;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Prototypes;

namespace Content.Server._LostParadise.Roadmap
{
    public class RoadmapSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _protoManager = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<RoadmapComponent, RoadmapPhaseUpdateEvent>(OnPhaseUpdate);
        }

        private void OnPhaseUpdate(EntityUid uid, RoadmapComponent component, RoadmapPhaseUpdateEvent args)
        {
            if (!_protoManager.TryIndex<RoadmapPhasePrototype>(args.PhaseId, out var phase))
            {
                Logger.ErrorS("roadmap", $"Phase {args.PhaseId} not found.");
                return;
            }

            component.CurrentPhase = phase.ID;
            component.Progress = args.NewProgress;

            Dirty(uid, component);
        }
    }
}
