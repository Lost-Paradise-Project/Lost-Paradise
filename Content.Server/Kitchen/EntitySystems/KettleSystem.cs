using Content.Server.Body.Systems;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Construction;
using Content.Server.Explosion.EntitySystems;
using Content.Server.DeviceLinking.Events;
using Content.Server.DeviceLinking.Systems;
using Content.Server.Hands.Systems;
using Content.Server.Kitchen.Components;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Temperature.Components;
using Content.Server.Temperature.Systems;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Construction.EntitySystems;
using Content.Shared.Destructible;
using Content.Shared.FixedPoint;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Robust.Shared.Random;
using Robust.Shared.Audio;
using Content.Server.Lightning;
using Content.Shared.Item;
using Content.Shared.Kitchen;
using Content.Shared.Kitchen.Components;
using Content.Shared.Popups;
using Content.Shared.Power;
using Content.Shared.Tag;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Player;
using System.Linq;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.Kitchen.EntitySystems
{
    public sealed class KettleSystem : EntitySystem
    {
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
        [Dependency] private readonly SharedAudioSystem _audio = default!;
        [Dependency] private readonly SharedContainerSystem _container = default!;
        [Dependency] private readonly RecipeManager _recipeManager = default!;
        [Dependency] private readonly TemperatureSystem _temperature = default!;
        [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
        [Dependency] private readonly SharedItemSystem _item = default!;
        [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;
        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<ActiveKettleComponent, ComponentStartup>(OnBoilStart);
            SubscribeLocalEvent<ActiveKettleComponent, ComponentShutdown>(OnBoilStop);
        }

        public void OnBoilStart(Entity<ActiveKettleComponent> ent, ref ComponentStartup args)
        {
            if (!TryComp<KettleComponent>(ent, out var kettleComponent))
                return;
            SetAppearance(ent.Owner, KettleVisualState.Boiling, kettleComponent);

            kettleComponent.PlayingStream =
            _audio.PlayPvs(kettleComponent.LoopingSound, ent, AudioParams.Default.WithLoop(true).WithMaxDistance(5)).Value.Entity;
        }

        private void OnBoilStop(Entity<ActiveKettleComponent> ent, ref ComponentShutdown args)
        {
            if (!TryComp<KettleComponent>(ent, out var kettleComponent))
                return;

            SetAppearance(ent.Owner, KettleVisualState.Idle, kettleComponent);
            kettleComponent.PlayingStream = _audio.Stop(kettleComponent.PlayingStream);
        }

        private void AddTemperature(KettleComponent component, float time)
        {
            var heatToAdd = time * component.BaseHeatMultiplier;
            foreach (var entity in component.Storage.ContainedEntities)
            {
                if (TryComp<TemperatureComponent>(entity, out var tempComp))
                    _temperature.ChangeHeat(entity, heatToAdd * component.ObjectHeatMultiplier, false, tempComp);

                if (!TryComp<SolutionContainerManagerComponent>(entity, out var solutions))
                    continue;
                foreach (var (_, soln) in _solutionContainer.EnumerateSolutions((entity, solutions)))
                {
                    var solution = soln.Comp.Solution;
                    if (solution.Temperature > component.TemperatureUpperThreshold)
                        continue;

                    _solutionContainer.AddThermalEnergy(soln, heatToAdd);
                }
            }
        }

        private void SubtractContents(KettleComponent component, DrinkRecipePrototype recipe)
        {

            var totalReagentsToRemove = new Dictionary<string, FixedPoint2>(recipe.IngredientsReagents);

            // yoinked this spaghetti from microwaves :p
            foreach (var item in component.Storage.ContainedEntities)
            {
                if (!TryComp<SolutionContainerManagerComponent>(item, out var solMan))
                    continue;

                // go over every solution
                foreach (var (_, soln) in _solutionContainer.EnumerateSolutions((item, solMan)))
                {
                    var solution = soln.Comp.Solution;
                    foreach (var (reagent, _) in recipe.IngredientsReagents)
                    {
                        // removed everything
                        if (!totalReagentsToRemove.ContainsKey(reagent))
                            continue;

                        var quant = solution.GetTotalPrototypeQuantity(reagent);

                        if (quant >= totalReagentsToRemove[reagent])
                        {
                            quant = totalReagentsToRemove[reagent];
                            totalReagentsToRemove.Remove(reagent);
                        }
                        else
                        {
                            totalReagentsToRemove[reagent] -= quant;
                        }

                        _solutionContainer.RemoveReagent(soln, reagent, quant);
                    }
                }
            }

            foreach (var recipeSolid in recipe.IngredientsSolids)
            {
                for (var i = 0; i < recipeSolid.Value; i++)
                {
                    foreach (var item in component.Storage.ContainedEntities)
                    {
                        var metaData = MetaData(item);
                        if (metaData.EntityPrototype == null)
                        {
                            continue;
                        }

                        if (metaData.EntityPrototype.ID == recipeSolid.Key)
                        {
                            _container.Remove(item, component.Storage);
                            EntityManager.DeleteEntity(item);
                            break;
                        }
                    }
                }
            }
        }

        public static bool HasContents(KettleComponent component)
        {
            return component.Storage.ContainedEntities.Any();
        }
        private void OnInit(Entity<KettleComponent> ent, ref ComponentInit args)
        {
            // this does have to be in ComponentInit, do I know why? No :3
            ent.Comp.Storage = _container.EnsureContainer<Container>(ent, "storagebase");
        }

        public void SetAppearance(EntityUid uid, KettleVisualState state, KettleComponent? component = null, AppearanceComponent? appearanceComponent = null)
        {
            if (!Resolve(uid, ref component, ref appearanceComponent, false))
                return;
            _appearance.SetData(uid, PowerDeviceVisuals.VisualState, state, appearanceComponent);
        }
    }
}