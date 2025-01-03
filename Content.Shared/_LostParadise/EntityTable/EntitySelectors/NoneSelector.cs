using Robust.Shared.Prototypes;

namespace Content.Shared._LostParadise.EntityTable.EntitySelectors;

/// <summary>
/// Selects nothing.
/// </summary>
public sealed partial class LPNoneSelector : LPEntityTableSelector
{
    protected override IEnumerable<EntProtoId> GetSpawnsImplementation(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto)
    {
        yield break;
    }
}
