using Content.Shared.Random.Helpers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared._LostParadise.EntityTable.EntitySelectors;

/// <summary>
/// Gets the spawns from one of the child selectors, based on the weight of the children
/// </summary>
public sealed partial class LPGroupSelector : LPEntityTableSelector
{
    [DataField(required: true)]
    public List<LPEntityTableSelector> Children = new();

    [Dependency] private readonly IRobustRandom _robustRandom = default!;

    protected override IEnumerable<EntProtoId> GetSpawnsImplementation(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto)
    {
        var children = new Dictionary<LPEntityTableSelector, float>(Children.Count);
        foreach (var child in Children)
        {
            children.Add(child, child.Weight);
        }

        var pick = SharedRandomExtensions.Pick(_robustRandom, children);

        return pick.GetSpawns(rand, entMan, proto);
    }
}
