using Content.Client.Pinpointer.UI;
using Content.Shared._Backmen.StationAI;
using Robust.Client.Graphics;
using Robust.Shared.Map.Components;

namespace Content.Client._Backmen.StationAI.UI;

public sealed class CameraNavMapControl : NavMapControl
{
    public CameraNavMapControl() : base()
    {
        // Set colors
        TileColor = new Color(30, 57, 67);
        WallColor = new Color(102, 164, 217);
        BackgroundColor = Color.FromSrgb(TileColor.WithAlpha(BackgroundOpacity));
    }
}
