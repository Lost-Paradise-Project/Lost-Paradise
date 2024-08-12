using Content.Shared._LostParadise.RCDFAP.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared._LostParadise.RCDFAP.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(RCDFAPAmmoSystem))]
public sealed partial class RCDFAPAmmoComponent : Component
{
    /// <summary>
    /// How many charges are contained in this ammo cartridge.
    /// Can be partially transferred into an RCDFAP, until it is empty then it gets deleted.
    /// </summary>
    [DataField("charges"), ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public int Charges = 130;
}
