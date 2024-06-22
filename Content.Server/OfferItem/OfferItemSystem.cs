using Content.Server.Popups;
using Content.Shared.Hands.Components;
using Content.Shared.Alert;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.OfferItem;
using Content.Shared.IdentityManagement;
using Robust.Shared.Player;

namespace Content.Server.OfferItem;

public sealed class OfferItemSystem : SharedOfferItemSystem
{
    [Dependency] private readonly AlertsSystem _alertsSystem = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly PopupSystem _popup = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<OfferItemComponent>();
        while (query.MoveNext(out var uid, out var offerItem))
        {
            if (!TryComp<HandsComponent>(uid, out var hands) || hands.ActiveHand == null)
                continue;

            if (offerItem.Hand != null &&
                hands.Hands[offerItem.Hand].HeldEntity == null)
                UnOffer(uid, offerItem);

            if (!offerItem.IsInReceiveMode)
            {
                _alertsSystem.ClearAlert(uid, AlertType.Offer);
                continue;
            }

            _alertsSystem.ShowAlert(uid, AlertType.Offer);
        }
    }

    public void Receiving(EntityUid uid, OfferItemComponent? component = null)
    {
        if (!Resolve(uid, ref component) ||
            !TryComp<OfferItemComponent>(component.Target, out var offerItem) ||
            offerItem.Hand == null ||
            component.Target == null ||
            !TryComp<HandsComponent>(uid, out var hands))
            return;

        if (offerItem.Item != null && !_hands.TryPickup(component.Target.Value, offerItem.Item.Value,
                handsComp: hands))
        {
            _popup.PopupEntity(Loc.GetString("offer-item-full-hand"), component.Target.Value, component.Target.Value);
            return;
        }

        if (offerItem.Item != null)
        {
            _popup.PopupEntity(Loc.GetString("offer-item-give",
                ("item", Identity.Entity(offerItem.Item.Value, EntityManager)),
                ("target", Identity.Entity(uid, EntityManager))), component.Target.Value, component.Target.Value);
            _popup.PopupEntity(Loc.GetString("offer-item-give-other",
                    ("user", Identity.Entity(component.Target.Value, EntityManager)),
                    ("item", Identity.Entity(offerItem.Item.Value, EntityManager)),
                    ("target", Identity.Entity(uid, EntityManager)))
                , component.Target.Value, Filter.PvsExcept(component.Target.Value, entityManager: EntityManager), true);
        }

        offerItem.Item = null;
        UnOffer(uid, component);
    }
}