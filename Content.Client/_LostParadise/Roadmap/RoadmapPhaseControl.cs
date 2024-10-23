using Content.Shared._LostParadise.Roadmap;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface;
using Robust.Shared.Maths;

namespace Content.Client._LostParadise.Roadmap
{
    public class RoadmapPhaseControl : Control
    {
        public RoadmapPhaseControl(RoadmapPhasePrototype phase)
        {
            var phaseContainer = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Vertical,
                SeparationOverride = 5
            };

            var headerLabel = new Label
            {
                Text = phase.Name,
                Align = Label.AlignMode.Center,
                VAlign = Label.VAlignMode.Top
            };
            phaseContainer.AddChild(headerLabel);

            var descriptionLabel = new Label
            {
                Text = phase.Description,
                Align = Label.AlignMode.Center,
                VAlign = Label.VAlignMode.Top
            };
            phaseContainer.AddChild(descriptionLabel);

            var progressBar = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 100,
                Value = phase.Progress,
                HorizontalExpand = true
            };
            phaseContainer.AddChild(progressBar);

            var statusLabel = new Label
            {
                Text = $"Статус: {phase.Status}",
                Align = Label.AlignMode.Center,
                VAlign = Label.VAlignMode.Top
            };
            phaseContainer.AddChild(statusLabel);

            AddChild(phaseContainer);
        }
    }
}
