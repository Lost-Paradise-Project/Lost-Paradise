using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.Kitchen
{
    /// <summary>
    ///    Kettle Recipes, teas and stuff
    /// </summary>

    [Prototype("kettleDrinkRecipe")]
    public sealed partial class DrinkRecipePrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;


        [DataField("minTemp")]
        public float MinimumTemperature = 0.0f;

        [DataField("name")]
        private string _name = string.Empty;

        [DataField("reagents", customTypeSerializer: typeof(PrototypeIdDictionarySerializer<FixedPoint2, ReagentPrototype>))]
        private Dictionary<string, FixedPoint2> _ingsReagents = new();

        [DataField("solids", customTypeSerializer: typeof(PrototypeIdDictionarySerializer<FixedPoint2, EntityPrototype>))]
        private Dictionary<string, FixedPoint2> _ingsSolids = new ();

        [DataField("product", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string Product { get; private set; } = string.Empty;
        public string Name => Loc.GetString(_name);

        public IReadOnlyDictionary<string, FixedPoint2> IngredientsReagents => _ingsReagents;
        public IReadOnlyDictionary<string, FixedPoint2> IngredientsSolids => _ingsSolids;
        public FixedPoint2 IngredientCount()
        {
            FixedPoint2 n = 0;
            n += _ingsReagents.Count; // number of distinct reagents
            foreach (FixedPoint2 i in _ingsSolids.Values) // sum the number of solid ingredients
            {
                n += i;
            }
            return n;
        }
    }
}