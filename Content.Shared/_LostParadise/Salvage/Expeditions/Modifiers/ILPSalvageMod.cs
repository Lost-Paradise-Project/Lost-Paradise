namespace Content.Shared._LostParadise.Salvage.Expeditions.Modifiers;

public interface ILPSalvageMod
{
    /// <summary>
    /// Player-friendly version describing this modifier.
    /// </summary>
    LocId Description { get; }

    /// <summary>
    /// Cost for difficulty modifiers.
    /// </summary>
    float Cost { get; }
}
