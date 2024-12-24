using Robust.Shared.GameStates;
using Content.Shared.Interaction;

namespace Content.Shared._LostParadise.Interaction.Components;

/// <summary>
/// This is used for identifying entities as being able to use complex interactions with the environment.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedInteractionSystem))]
public sealed partial class ComplexInteractionComponent : Component;
