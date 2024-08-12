using Content.Server.Actions;
using Content.Shared._LostParadise.AngelDust.Components;
using Content.Server.Polymorph.Systems;

namespace Content.Server._LostParadise.AngelDust.Systems;

public sealed class AngelDustSystem : EntitySystem
{
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly PolymorphSystem _polymorphSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AngelDustComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<AngelDustComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<AngelDustComponent, AngelDustPolyEvent>(OnMorhpAngelDust);
    }

    private void OnMorhpAngelDust(EntityUid uid, AngelDustComponent component, AngelDustPolyEvent args) //при нажатии кнопки перевоплощения
    {
        //Log.Warning($"Morphing: {component.AngelDustPolymorphId}");
        _polymorphSystem.PolymorphEntity(uid, component.AngelDustPolymorphId);
        /*if (component.AngelDustPolymorphId == "LPPAngelDustYukiMorph")
            component.AngelDustPolymorphId = "LPPAngelDustYukiDeMorph";
        else
            component.AngelDustPolymorphId = "LPPAngelDustYukiMorph";*/
    }

    private void OnShutdown(EntityUid uid, AngelDustComponent component, ComponentShutdown args)
    {
        _actions.RemoveAction(uid, component.Action);
    }

    private void OnMapInit(EntityUid uid, AngelDustComponent component, MapInitEvent args)
    {
        _actions.AddAction(uid, ref component.Action, component.TestAction);
    }
}
