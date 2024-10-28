using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._LostParadise.Roadmap
{
    [Prototype("roadmap")]
    public class RoadmapPrototype : IPrototype
    {
        [IdDataField]
        public string ID { get; } = default!;

        [DataField("name")]
        public string Name { get; } = string.Empty;

        [DataField("description")]
        public string Description { get; } = string.Empty;

        [DataField("progress")]
        public float Progress { get; set; } = 0.0f;

        [DataField("status")]
        public string Status { get; set; } = "roadmap-goal-waiting";
        [DataField("order")]
        public int Order { get; set; }
    }
}
