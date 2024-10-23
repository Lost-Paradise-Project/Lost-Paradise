using Robust.Shared.GameObjects;

namespace Content.Shared._LostParadise.Roadmap
{
    public sealed class RoadmapPhaseUpdateEvent : EntityEventArgs
    {
        public string PhaseId { get; }
        public float NewProgress { get; }

        public RoadmapPhaseUpdateEvent(string phaseId, float newProgress)
        {
            PhaseId = phaseId;
            NewProgress = newProgress;
        }
    }
}
