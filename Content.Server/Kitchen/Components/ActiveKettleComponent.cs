using Content.Shared.Kitchen;

namespace Content.Server.Kitchen.Components;

/// <summary>
/// Attached to a microwave that is currently in the process of cooking
/// </summary>
[RegisterComponent]
public sealed partial class ActiveKettleComponent : Component
{
    [ViewVariables]
    public (DrinkRecipePrototype?, int) PortionedRecipe;
}
