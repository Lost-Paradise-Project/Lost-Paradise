using Content.Shared._LostParadise.Salvage.Expeditions;
using JetBrains.Annotations;

namespace Content.Client._LostParadise.Salvage.UI;

[UsedImplicitly]
public sealed class LPSalvageExpeditionConsoleBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private LPSalvageExpeditionWindow? _window;

    public LPSalvageExpeditionConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = new LPSalvageExpeditionWindow();
        _window.ClaimMission += index =>
        {
            SendMessage(new ClaimLPSalvageMessage()
            {
                Index = index,
            });
        };
        _window.FinishMission += () => SendMessage(new FinishLPSalvageMessage()); // Frontier
        _window.OnClose += Close;
        _window?.OpenCenteredLeft();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _window?.Dispose();
        _window = null;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not LPSalvageExpeditionConsoleState current)
            return;

        _window?.UpdateState(current);
    }
}
