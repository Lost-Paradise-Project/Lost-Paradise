using Robust.Shared.Prototypes;

namespace Content.Shared._LostParadise.EntityTable.ValueSelector;

/// <summary>
/// Gives a constant value.
/// </summary>
public sealed partial class LPConstantNumberSelector : LPNumberSelector
{
    [DataField]
    public float Value = 1;

    public LPConstantNumberSelector(float value)
    {
        Value = value;
    }

    public override float Get(System.Random rand, IEntityManager entMan, IPrototypeManager proto)
    {
        return Value;
    }
}
