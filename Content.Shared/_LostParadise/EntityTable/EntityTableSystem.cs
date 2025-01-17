using Content.Shared._LostParadise.EntityTable.EntitySelectors;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared._LostParadise.EntityTable;

public sealed class LPEntityTableSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public IEnumerable<EntProtoId> GetSpawns(LPEntityTableSelector? table, System.Random? rand = null)
    {
        if (table == null)
            return new List<EntProtoId>();

        rand ??= _random.GetRandom();
        return table.GetSpawns(rand, EntityManager, _prototypeManager);
    }
}
