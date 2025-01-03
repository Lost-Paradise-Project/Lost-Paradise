using Content.Server.Construction.Components;
using Content.Server.Stack;
using Content.Shared.Construction;
using Content.Shared.Interaction;
using Content.Shared.Stacks;
using Content.Shared.Storage;
using SharedToolSystem = Content.Shared.Tools.Systems.SharedToolSystem;

namespace Content.Server.Construction;

public sealed class RefiningSystem : EntitySystem
{
    [Dependency] private readonly SharedToolSystem _toolSystem = default!;
    [Dependency] private readonly StackSystem _stackSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<WelderRefinableComponent, InteractUsingEvent>(OnInteractUsing);
        SubscribeLocalEvent<WelderRefinableComponent, WelderRefineDoAfterEvent>(OnDoAfter);
    }

    private void OnInteractUsing(EntityUid uid, WelderRefinableComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = _toolSystem.UseTool(args.Used, args.User, uid, component.RefineTime, component.QualityNeeded, new WelderRefineDoAfterEvent(), fuel: component.RefineFuel);
    }

    private void OnDoAfter(EntityUid uid, WelderRefinableComponent component, WelderRefineDoAfterEvent args)
    {
        if (args.Cancelled)
            return;

        // Get last owner coordinates and delete it
        var resultPosition = Transform(uid).Coordinates;
        EntityManager.DeleteEntity(uid);

        // Spawn each result after refine
        foreach (var ent in EntitySpawnCollection.GetSpawns(component.RefineResult ?? new()))
        {
            var droppedEnt = Spawn(ent, resultPosition);

            // TODO: If something has a stack... Just use a prototype with a single thing in the stack.
            // This is not a good way to do it.
            if (TryComp<StackComponent>(droppedEnt, out var stack))
                _stackSystem.SetCount(droppedEnt, 1, stack);
        }
    }
}
