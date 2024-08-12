using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared._LostParadise.RCDFAP.Components;
using Robust.Shared.Timing;

namespace Content.Shared._LostParadise.RCDFAP.Systems;

public sealed class RCDFAPAmmoSystem : EntitySystem
{
    [Dependency] private readonly SharedChargesSystem _charges = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RCDFAPAmmoComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<RCDFAPAmmoComponent, AfterInteractEvent>(OnAfterInteract);
    }

    private void OnExamine(EntityUid uid, RCDFAPAmmoComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var examineMessage = Loc.GetString("rcdfap-ammo-component-on-examine", ("charges", comp.Charges));
        args.PushText(examineMessage);
    }

    private void OnAfterInteract(EntityUid uid, RCDFAPAmmoComponent comp, AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach || !_timing.IsFirstTimePredicted)
            return;

        if (args.Target is not { Valid: true } target ||
            !HasComp<RCDFAPComponent>(target) ||
            !TryComp<LimitedChargesComponent>(target, out var charges))
            return;

        var user = args.User;
        args.Handled = true;
        var count = Math.Min(charges.MaxCharges - charges.Charges, comp.Charges);
        if (count <= 0)
        {
            _popup.PopupClient(Loc.GetString("rcdfap-ammo-component-after-interact-full"), target, user);
            return;
        }

        _popup.PopupClient(Loc.GetString("rcdfap-ammo-component-after-interact-refilled"), target, user);
        _charges.AddCharges(target, count, charges);
        comp.Charges -= count;
        Dirty(uid, comp);

        // prevent having useless ammo with 0 charges
        if (comp.Charges <= 0)
            QueueDel(uid);
    }
}
