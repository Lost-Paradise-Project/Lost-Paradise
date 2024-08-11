using Content.Shared.Construction.Prototypes;
using Content.Shared.DeviceLinking;
using Content.Shared.Item;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Kitchen.Components
{
    [RegisterComponent]
    public sealed partial class KettleComponent : Component
    {

        [DataField("baseHeatMultiplier"), ViewVariables(VVAccess.ReadWrite)]
        public float BaseHeatMultiplier = 100;

        public Container Storage = default!;

        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public int Capacity = 3;

        [DataField("objectHeatMultiplier"), ViewVariables(VVAccess.ReadWrite)]
        public float ObjectHeatMultiplier = 100;

        [DataField("failureResult", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string BadRecipeEntityId = "FoodBadRecipe";

        #region  audio
        [DataField("beginBoilSound")]
        public SoundSpecifier StartBoilingSound = new SoundPathSpecifier("/Audio/Items/Kettle/kettle_switch_on.ogg");

        [DataField("boilDoneSound")]
        public SoundSpecifier BoilDoneSound = new SoundPathSpecifier("/Audio/Machines/kettle_done.ogg");

        public EntityUid? PlayingStream;

        [DataField("loopingSound")]
        public SoundSpecifier LoopingSound = new SoundPathSpecifier("/Audio/Machines/kettle_boiling_water.ogg");
        #endregion


        /// <summary>
        ///     Max Temp the kettle can heat to
        /// </summary>
        [DataField("temperatureLimit")]
        public float TemperatureUpperThreshold = 373.15f; // boiling point of water :v

    }

    public sealed class BeingBoiledEvent : HandledEntityEventArgs
    {
        public EntityUid Kettle;
        public EntityUid? User;

        public BeingBoiledEvent(EntityUid kettle, EntityUid? user)
        {
            Kettle = kettle;
            User = user;
        }
    }
}
