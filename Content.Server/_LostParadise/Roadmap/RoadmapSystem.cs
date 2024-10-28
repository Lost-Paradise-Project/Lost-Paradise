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
            SubscribeLocalEvent<RoadmapComponent, RoadmapUpdateEvent>(OnPhaseUpdate);
        }

        private void OnPhaseUpdate(EntityUid uid, RoadmapComponent component, RoadmapUpdateEvent args)
        {
            if (!_protoManager.TryIndex<RoadmapPrototype>(args.RoadmapId, out var phase))
            {
                Logger.ErrorS("roadmap", $"Roadmap {args.RoadmapId} not found.");
                return;
            }

            component.CurrentPhase = phase.ID;
            component.Progress = args.NewProgress;

            Dirty(uid, component);
        }
    }
}
