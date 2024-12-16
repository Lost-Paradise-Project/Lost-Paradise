using Robust.Shared.GameObjects;

namespace Content.Shared._LostParadise.Roadmap
{
    public sealed class RoadmapUpdateEvent : EntityEventArgs
    {
        public string RoadmapId { get; }
        public float NewProgress { get; }

        public RoadmapUpdateEvent(string roadmapId, float newProgress)
        {
            RoadmapId = roadmapId;
            NewProgress = newProgress;
        }
    }
}
