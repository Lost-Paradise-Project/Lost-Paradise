using Content.Shared._LostParadise.Roadmap;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using System.Numerics;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.Controllers;
using System.Linq;

namespace Content.Client._LostParadise.Roadmap
{
    public sealed class RoadmapUI : UIController
    {
        private DefaultWindow _roadmapWindow;

        public RoadmapUI()
        {
            _roadmapWindow = new DefaultWindow
            {
                Title = Loc.GetString("roadmap-plan-LLP"),
                SetSize = new Vector2(600, 600),
                Resizable = false
            };

            var panelContainer = new PanelContainer
            {
                MinSize = new Vector2(580, 580),
                ModulateSelfOverride = Color.Transparent,
                Margin = new Thickness(10)
            };

            var scrollContainer = new ScrollContainer
            {
                HorizontalExpand = true,
                VerticalExpand = true,
                Margin = new Thickness(0, 20, 0, 0)
            };

            var phaseList = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Vertical,
                SeparationOverride = 10
            };

            scrollContainer.AddChild(phaseList);
            panelContainer.AddChild(scrollContainer);
            _roadmapWindow.AddChild(panelContainer);

            RefreshUI(phaseList);
        }

        private void RefreshUI(BoxContainer phaseList)
        {
            phaseList.RemoveAllChildren();

            var roadmapSystem = IoCManager.Resolve<IPrototypeManager>();

            var roadmapPhases = roadmapSystem.EnumeratePrototypes<RoadmapPrototype>()
                                            .OrderBy<RoadmapPrototype, int>(phase => phase.Order);

            foreach (var phase in roadmapPhases)
            {
                var phaseControl = new RoadmapControl(phase);
                phaseList.AddChild(phaseControl);
            }
        }

        public void ToggleRoadmap()
        {
            if (_roadmapWindow.IsOpen)
            {
                _roadmapWindow.Close();
            }
            else
            {
                _roadmapWindow.OpenCentered();
            }
        }
    }
}
