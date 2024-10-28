using Content.Shared._LostParadise.Roadmap;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface;
using System.Numerics;

namespace Content.Client._LostParadise.Roadmap
{
    public class RoadmapControl : Control
    {
        private readonly RoadmapPrototype _prototype;
        private readonly ProgressBar _progressBar;

        public RoadmapControl(RoadmapPrototype prototype)
        {
            _prototype = prototype;
            _progressBar = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 100,
                Value = _prototype.Progress,
                HorizontalExpand = true
            };

            SetupUI();
        }

        private void SetupUI()
        {
            Margin = new Thickness(0, 20, 0, 0);

            var vBox = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Vertical
            };

            var nameButton = new Button
            {
                Text = _prototype.Name,
                StyleClasses = { "Caution" },
                HorizontalExpand = true
            };

            vBox.AddChild(nameButton);

            var descriptionLabel = new Label { Text = _prototype.Description, FontColorOverride = Color.LightGray };
            var progressLabel = new Label
            { 
                Text = Loc.GetString("roadmap-progress") + $": {_prototype.Progress}%",
                FontColorOverride = Color.White
            };

            var statusBox = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Horizontal,
                HorizontalExpand = true,
                SeparationOverride = 5
            };

            var statusLabel = new Label
            {
                Text = Loc.GetString("roadmap-status") + $": {Loc.GetString(_prototype.Status)}",
                FontColorOverride = GetStatusColor(),
            };

            statusBox.AddChild(statusLabel);
            statusBox.AddChild(new Control { HorizontalExpand = true });

            vBox.AddChild(descriptionLabel);
            vBox.AddChild(progressLabel);
            vBox.AddChild(_progressBar);
            vBox.AddChild(statusBox);

            var separator = new PanelContainer
            {
                Modulate = new Color(0.5f, 0.5f, 0.5f, 1f),
                MinSize = new Vector2(0, 2),
                HorizontalExpand = true
            };
            vBox.AddChild(separator);

            AddChild(vBox);
        }

        private Color GetStatusColor()
        {
            string status = _prototype.Status;
            return status switch
            {
                "roadmap-goal-completed" => new Color(0.0f, 1.0f, 0.0f),
                "roadmap-goal-progress" => new Color(1.0f, 1.0f, 0.0f),
                "roadmap-goal-waiting" => new Color(1.0f, 0.5f, 0.0f),
                _ => Color.White
            };
        }
    }
}
