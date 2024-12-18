namespace Content.Shared._LostParadise.Salvage.Expeditions.Modifiers;

public interface ILPBiomeSpecificMod : ILPSalvageMod
{
    /// <summary>
    /// Whitelist for biomes. If null then any biome is allowed.
    /// </summary>
    List<string>? Biomes { get; }
}
