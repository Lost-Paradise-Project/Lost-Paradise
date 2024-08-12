using Content.Shared.Hands.Components;
using Content.Shared.Interaction;
using Content.Shared._LostParadise.RCDFAP;
using Content.Shared._LostParadise.RCDFAP.Components;
using Content.Shared._LostParadise.RCDFAP.Systems;
using Robust.Client.Placement;
using Robust.Client.Player;
using Robust.Shared.Enums;

namespace Content.Client._LostParadise.RCDFAP;

public sealed class RCDFAPConstructionGhostSystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly RCDFAPSystem _rcdfapSystem = default!;
    [Dependency] private readonly IPlacementManager _placementManager = default!;

    private string _placementMode = typeof(AlignRCDFAPConstruction).Name;
    private Direction _placementDirection = default;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        // Get current placer data
        var placerEntity = _placementManager.CurrentPermission?.MobUid;
        var placerProto = _placementManager.CurrentPermission?.EntityType;
        var placerIsRCDFAP = HasComp<RCDFAPComponent>(placerEntity);

        // Exit if erasing or the current placer is not an RCDFAP (build mode is active)
        if (_placementManager.Eraser || (placerEntity != null && !placerIsRCDFAP))
            return;

        // Determine if player is carrying an RCDFAP in their active hand
        var player = _playerManager.LocalSession?.AttachedEntity;

        if (!TryComp<HandsComponent>(player, out var hands))
            return;

        var heldEntity = hands.ActiveHand?.HeldEntity;

        if (!TryComp<RCDFAPComponent>(heldEntity, out var rcdfap))
        {
            // If the player was holding an RCDFAP, but is no longer, cancel placement
            if (placerIsRCDFAP)
                _placementManager.Clear();

            return;
        }

        // Update the direction the RCDFAP prototype based on the placer direction
        if (_placementDirection != _placementManager.Direction)
        {
            _placementDirection = _placementManager.Direction;
            RaiseNetworkEvent(new RCDFAPConstructionGhostRotationEvent(GetNetEntity(heldEntity.Value), _placementDirection));
        }

        // If the placer has not changed, exit
        _rcdfapSystem.UpdateCachedPrototype(heldEntity.Value, rcdfap);

        if (heldEntity == placerEntity && rcdfap.CachedPrototype.Prototype == placerProto)
            return;

        // Create a new placer
        var newObjInfo = new PlacementInformation
        {
            MobUid = heldEntity.Value,
            PlacementOption = _placementMode,
            EntityType = rcdfap.CachedPrototype.Prototype,
            Range = (int) Math.Ceiling(SharedInteractionSystem.InteractionRange),
            UseEditorContext = false,
        };

        _placementManager.Clear();
        _placementManager.BeginPlacing(newObjInfo);
    }
}
