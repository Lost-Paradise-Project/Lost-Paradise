using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._LostParadise.Roadmap
{
    [Prototype("roadmapPhase")]
    public class RoadmapPhasePrototype : IPrototype
    {
        [IdDataField]
        public string ID { get; } = default!;

        [DataField("name")]
        public string Name { get; } = string.Empty;

        [DataField("description")]
        public string Description { get; } = string.Empty;

        [DataField("progress")]
        public float Progress { get; set; } = 0.0f;

        [DataField("releaseDate")]
        public string ReleaseDate { get; set; } = string.Empty;

        [DataField("status")]
        public string Status { get; set; } = "In Progress";
    }
}
