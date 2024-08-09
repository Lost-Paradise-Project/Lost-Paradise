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

        [DataField("reagents")]
        public Dictionary<string, FixedPoint2> Reagents { get; private set; } = new();

        [DataField("solids")]
        public IReadOnlyDictionary<string, FixedPoint2> Solids => Solids;

        [DataField("products")]
        public Dictionary<string, FixedPoint2> Products { get; private set; } = new();

        public string Name => Loc.GetString(_name);

        [DataDefinition]
        public sealed partial class ReactantPrototype
        {
            [DataField("amount")]
            private FixedPoint2 _amount = FixedPoint2.New(1);

            public FixedPoint2 Amount => _amount;
        }
        public FixedPoint2 IngredientCount()
        {
            FixedPoint2 n = 0;
            n += Reagents.Count; // number of distinct reagents
            foreach (FixedPoint2 i in Solids.Values) // sum the number of solid ingredients
            {
                n += i;
            }
            return n;
        }
    }
}