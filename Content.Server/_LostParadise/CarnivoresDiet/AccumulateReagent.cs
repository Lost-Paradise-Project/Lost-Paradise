using Content.Server.Body.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using Content.Server.EntityEffects.Effects;
using Content.Shared.FixedPoint;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Chemistry.ReagentEffects
{
    // This class is basically copied from the sealed class AdjustReagent, but compensates a reagent's metabolism rates when adjusting its levels.
    // Was introduced in order to make theobromine accumulate faster than it metabolises.
    // While quite kludgy, it is still much safer and cleaner alternative to actively preventing the metabolism system from processing theobromine while it's still in the system.

    // TL;DR Это костыль, который тупо добавляет столько же реагента, сколько должно впитаться за данный тик.
    // Решение сомнительное, но оно гораздо чище, чем лезть в код метаболирования и добавлять туда группу-исключение с проверкой на наличие реагента в кровеносной системе.

    [UsedImplicitly]
    public sealed partial class AccumulateReagent : EntityEffect
    {
        /// <summary>
        ///     The reagent ID to accumulate. Only one of this and <see cref="Group"/> should be active.
        /// </summary>
        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<ReagentPrototype>), required: true)]
        public string? Reagent;

        /// <summary>
        /// Checks if the target has something that already adds the target reagent
        /// </summary>
        public bool ContainsPositiveAdjustEffect(IPrototypeManager prototypeMan, Solution solution, List<MetabolismGroupEntry> groups)
        {
            foreach (var quantity in solution.Contents)
            {
                var reagent = quantity.Reagent.Prototype;
                if (reagent == Reagent)
                    continue;

                if (!prototypeMan.TryIndex(reagent, out ReagentPrototype? reagentProto))
                    continue;

                if (reagentProto?.Metabolisms == null)
                    continue;

                // Ideally we should iterate over the body's MetabolismGroupEntry list.
                // But I have no idea why there's no Drink in its .MetabolismGroups property and how to fetch that.
                // So it will stay like this for now, but might cause unintended theobromine accumulation in some *very* unlikely and specific scenarios.
                foreach (var reagentEffectsEntry in reagentProto.Metabolisms.Values)
                {
                    foreach (var effect in reagentEffectsEntry.Effects)
                    {
                        if (effect is not AdjustReagent adjustReagent)
                            continue;

                        if (adjustReagent.Reagent == Reagent)
                            return true;
                    }
                }
            }

            return false;
        }

        public override void Effect(EntityEffectBaseArgs args)
        {
            if (args is not EntityEffectReagentArgs reagentArgs)
            {
                // Log or handle unexpected argument type
                return;
            }

            if (reagentArgs.Source == null)
                return;

            if (Reagent == null)
                return;

            var prototypeMan = IoCManager.Resolve<IPrototypeManager>();
            prototypeMan.TryIndex(Reagent, out ReagentPrototype? reagentProto);

            args.EntityManager.TryGetComponent(reagentArgs.OrganEntity, out MetabolizerComponent? metabolizer);

            if (metabolizer?.MetabolismGroups is not List<MetabolismGroupEntry> groups)
                return;

            if (!ContainsPositiveAdjustEffect(prototypeMan, reagentArgs.Source, groups))
                return;

            if (reagentProto?.Metabolisms == null)
                return;

            FixedPoint2 totalCompensationRate = 0;
            foreach (var group in groups)
            {
                if (!reagentProto.Metabolisms.TryGetValue(group.Id, out var reagentEffectsEntry))
                    continue;

                var groupRate = reagentEffectsEntry.MetabolismRate * group.MetabolismRateModifier;
                totalCompensationRate += groupRate;
            }

            reagentArgs.Source.AddReagent(Reagent, totalCompensationRate);
        }


        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        {
            if (Reagent != null && prototype.TryIndex(Reagent, out ReagentPrototype? reagentProto))
            {
                return Loc.GetString("reagent-effect-guidebook-accumulate-reagent-reagent",
                    ("reagent", reagentProto.LocalizedName));
            }

            throw new NotImplementedException();
        }
    }
}
