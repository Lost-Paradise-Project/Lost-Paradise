using Robust.Shared.Prototypes;

namespace Content.Shared._LostParadise.EntityTable.EntitySelectors;

/// <summary>
/// Gets spawns from all of the child selectors
/// </summary>
public sealed partial class LPAllSelector : LPEntityTableSelector
{
    [DataField(required: true)]
    public List<LPEntityTableSelector> Children;

    protected override IEnumerable<EntProtoId> GetSpawnsImplementation(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto)
    {
        foreach (var child in Children)
        {
            foreach (var spawn in child.GetSpawns(rand, entMan, proto))
            {
                yield return spawn;
            }
        }
    }
}
