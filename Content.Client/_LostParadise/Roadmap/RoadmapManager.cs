using System;
using System.Collections.Generic;
using Content.Shared.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Client._LostParadise.Roadmap
{
    [Prototype("roadmap")]
    public sealed partial class RoadmapPrototype : IPrototype
    {
        [IdDataField]
        public string ID { get; } = default!;
        [DataField("name")]
        public string Name { get; } = default!;

        [DataField("description")]
        public string Description { get; } = default!;

        [DataField("progress")]
        public int Progress { get; }

        [DataField("releaseDate")]
        public string ReleaseDate { get; } = default!;

        [DataField("status")]
        public string Status { get; } = default!;
    }
    public class RoadmapEntry
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Progress { get; set; }

        public string ReleaseDate { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
    public class RoadmapManager
    {
        private readonly IPrototypeManager _prototypeManager;

        public RoadmapManager(IPrototypeManager prototypeManager)
        {
            _prototypeManager = prototypeManager;
        }
        public List<RoadmapEntry> LoadRoadmap()
        {
            var roadmapEntries = new List<RoadmapEntry>();

            foreach (var prototype in _prototypeManager.EnumeratePrototypes<RoadmapPrototype>())
            {
                roadmapEntries.Add(new RoadmapEntry
                {
                    Name = prototype.Name,
                    Description = prototype.Description,
                    Progress = prototype.Progress,
                    ReleaseDate = prototype.ReleaseDate,
                    Status = prototype.Status
                });
            }

            return roadmapEntries;
        }
    }
}
