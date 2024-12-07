using Content.Shared._LostParadise.EntityTable.ValueSelector;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared._LostParadise.EntityTable.EntitySelectors;

[ImplicitDataDefinitionForInheritors, UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract partial class LPEntityTableSelector
{
    /// <summary>
    /// The number of times this selector is run
    /// </summary>
    [DataField]
    public LPNumberSelector Rolls = new LPConstantNumberSelector(1);

    /// <summary>
    /// A weight used to pick between selectors.
    /// </summary>
    [DataField]
    public float Weight = 1;

    /// <summary>
    /// A simple chance that the selector will run.
    /// </summary>
    [DataField]
    public double Prob = 1;

    public IEnumerable<EntProtoId> GetSpawns(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto)
    {
        var rolls = Rolls.Get(rand, entMan, proto);
        for (var i = 0; i < rolls; i++)
        {
            if (!rand.Prob(Prob))
                continue;

            foreach (var spawn in GetSpawnsImplementation(rand, entMan, proto))
            {
                yield return spawn;
            }
        }
    }

    protected abstract IEnumerable<EntProtoId> GetSpawnsImplementation(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto);
}
