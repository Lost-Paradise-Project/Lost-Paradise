/*
* Delta-V - This file is licensed under AGPLv3
* Copyright (c) 2024 Delta-V Contributors
* See AGPLv3.txt for details.
*/

using Robust.Shared.GameStates;

namespace Content.Shared.DeltaV.Lamiae
{
    /// <summary>
    /// Lamia segment
    /// </summary>
    [RegisterComponent]
    [NetworkedComponent]
    public sealed partial class LamiaSegmentComponent : Component
    {
        public EntityUid AttachedToUid = default!;
        public float DamageModifyFactor = default!;
        public float OffsetSwitching = default!;
        public float ScaleFactor = default!;
        public float DamageModifierCoefficient = default!;
        public float ExplosiveModifyFactor = default!;
        public float OffsetConstant = default!;
        public EntityUid Lamia = default!;
        public bool BulletPassover = default!;
        public int MaxSegments = default!;
        public int SegmentNumber = default!;
        public float DamageModifierConstant = default!;
        [DataField("segmentId")]
        public string? segmentId;
    }
}