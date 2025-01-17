using Content.Shared.Weather;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared._LostParadise.Salvage.Expeditions.Modifiers;

[Prototype("LPsalvageWeatherMod")]
public sealed partial class LPSalvageWeatherMod : IPrototype, ILPBiomeSpecificMod
{
    [IdDataField] public string ID { get; } = default!;

    [DataField("desc")] public LocId Description { get; private set; } = string.Empty;

    /// <inheritdoc/>
    [DataField("cost")]
    public float Cost { get; private set; } = 0f;

    /// <inheritdoc/>
    [DataField("biomes", customTypeSerializer: typeof(PrototypeIdListSerializer<LPSalvageBiomeMod>))]
    public List<string>? Biomes { get; private set; } = null;

    /// <summary>
    /// Weather prototype to use on the planet.
    /// </summary>
    [DataField("weather", required: true, customTypeSerializer:typeof(PrototypeIdSerializer<WeatherPrototype>))]
    public string WeatherPrototype = string.Empty;
}
