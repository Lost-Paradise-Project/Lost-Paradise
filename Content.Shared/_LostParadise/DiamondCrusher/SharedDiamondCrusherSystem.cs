using Content.Shared.Examine;
using Content.Shared.Storage.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Content.Shared.Emag.Systems;

namespace Content.Shared._LostParadise.DiamondCrusher;

/// <резюме>
/// Это обрабатывает логику, относящуюся к <see cref="DiamondCrusherComponent"/>
/// </резюме>
public abstract class SharedDiamondCrusherSystem : EntitySystem
{
    [Dependency] protected readonly SharedAppearanceSystem Appearance = default!;
    [Dependency] protected readonly SharedAudioSystem AudioSystem = default!;
    [Dependency] protected readonly SharedContainerSystem ContainerSystem = default!;

    /// <унаследование/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DiamondCrusherComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<DiamondCrusherComponent, StorageAfterOpenEvent>(OnStorageAfterOpen);
        SubscribeLocalEvent<DiamondCrusherComponent, StorageOpenAttemptEvent>(OnStorageOpenAttempt);
        SubscribeLocalEvent<DiamondCrusherComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<DiamondCrusherComponent, GotEmaggedEvent>(OnEmagged);
    }

    private void OnInit(Entity<DiamondCrusherComponent> ent, ref ComponentInit args)
    {
        ent.Comp.OutputContainer = ContainerSystem.EnsureContainer<Container>(ent, ent.Comp.OutputContainerName);
    }

    private void OnStorageAfterOpen(Entity<DiamondCrusherComponent> ent, ref StorageAfterOpenEvent args)
    {
        StopCrushing(ent);
        ContainerSystem.EmptyContainer(ent.Comp.OutputContainer);
    }

    private void OnEmagged(Entity<DiamondCrusherComponent> ent, ref GotEmaggedEvent args)
    {
        ent.Comp.AutoLock = true;
        args.Handled = true;
    }

    private void OnStorageOpenAttempt(Entity<DiamondCrusherComponent> ent, ref StorageOpenAttemptEvent args)
    {
        if (ent.Comp.AutoLock && ent.Comp.Crushing)
            args.Cancelled = true;
    }

    private void OnExamine(Entity<DiamondCrusherComponent> ent, ref ExaminedEvent args)
    {
        args.PushMarkup(ent.Comp.AutoLock ? Loc.GetString("diamond-crusher-examine-autolocks") : Loc.GetString("artifact-crusher-examine-no-autolocks"));
    }

    public void StopCrushing(Entity<DiamondCrusherComponent> ent, bool early = true)
    {
        var (_, crusher) = ent;

        if (!crusher.Crushing)
            return;

        crusher.Crushing = false;
        Appearance.SetData(ent, DiamondCrusherVisuals.Crushing, false);

        if (early)
        {
            AudioSystem.Stop(crusher.CrushingSoundEntity?.Item1, crusher.CrushingSoundEntity?.Item2);
            crusher.CrushingSoundEntity = null;
        }

        Dirty(ent, ent.Comp);
    }
}
