using Content.Server.Nutrition.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Content.Shared.Tag;
using Content.Shared.Whitelist;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Server.Body.Components
{
    [DataDefinition]
    public sealed partial class PoorlyDigestibleFood
    {
        /// <summary>
        /// Reagents that get multiplied by the Factor field.
        /// </summary>
        [DataField("reducedReagents")]
        public List<string>? ReducedReagents = null;

        /// <summary>
        /// Food containing at least one of these tags is affected as long as it doesn't hit any BlacklistTags.
        /// </summary>
        [DataField("whitelistTags", customTypeSerializer: typeof(PrototypeIdListSerializer<TagPrototype>))]
        public List<string>? WhitelistTags = null;

        /// <summary>
        /// Food containing any of these tags is not affected.
        /// </summary>
        [DataField("blacklistTags", customTypeSerializer: typeof(PrototypeIdListSerializer<TagPrototype>))]
        public List<string>? BlacklistTags = null;

        /// <summary>
        /// Factor that the reagent quantities are multiplied by.
        /// </summary>
        [DataField("factor")]
        public FixedPoint2 Factor = 1f;

        /// <summary>
        /// Reagent that is used to replace the volume taken away by the Factor multiplication.
        /// </summary>
        [DataField("replacementReagent")]
        public string? ReplacementReagentID = null;

        public bool AffectsFood(EntityUid foodEnt, IEntityManager entityManager)
        {
            if (WhitelistTags is null || WhitelistTags.Count == 0)
                return false;

            var tagSys = entityManager.System<TagSystem>();

            var whitelistProtoIds = WhitelistTags
                .Select(tag => new ProtoId<TagPrototype>(tag))
                .ToList();

            if (BlacklistTags is not null && BlacklistTags.Count > 0)
            {
                var blacklistProtoIds = BlacklistTags
                    .Select(tag => new ProtoId<TagPrototype>(tag))
                    .ToList();

                if (tagSys.HasAnyTag(foodEnt, blacklistProtoIds))
                    return false;
            }

            return tagSys.HasAnyTag(foodEnt, whitelistProtoIds);
        }

        public Solution ModifySolution(Solution solution)
        {
            // In case someone forgot to set the reagents being replaced, we do nothing. 
            if (ReducedReagents is null || Factor <= 0f)
                return solution;

            // Recreating the list since we can't set quentities in-place.
            List<ReagentQuantity> newReagents = new();
            FixedPoint2 removedQuantity = 0f;
            foreach (var quantity in solution.Contents)
            {
                if (!ReducedReagents.Contains(quantity.Reagent.Prototype))
                {
                    newReagents.Add(quantity);
                    continue;
                }

                removedQuantity += quantity.Quantity;
                newReagents.Add(new ReagentQuantity(quantity.Reagent, quantity.Quantity * Factor));
            }
            removedQuantity *= 1f - Factor;

            if (ReplacementReagentID is string reagentId)
                newReagents.Add(new ReagentQuantity(new ReagentId(reagentId, null), removedQuantity));

            solution.SetContents(newReagents);
            return solution;
        }
    }
}
