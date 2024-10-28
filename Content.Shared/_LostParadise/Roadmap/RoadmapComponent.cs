using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Shared._LostParadise.Roadmap
{
    [RegisterComponent]
    public sealed partial class RoadmapComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("roadmap")]
        public string CurrentPhase { get; set; } = string.Empty;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("progress")]
        public float Progress { get; set; } = 0.0f;
    }
}
