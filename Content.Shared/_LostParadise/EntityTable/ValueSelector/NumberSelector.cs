using Content.Shared._LostParadise.EntityTable.EntitySelectors;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Shared._LostParadise.EntityTable.ValueSelector;

/// <summary>
/// Used for implementing custom value selection for <see cref="EntityTableSelector"/>
/// </summary>
[ImplicitDataDefinitionForInheritors, UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract partial class LPNumberSelector
{
    public abstract float Get(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto);
}
