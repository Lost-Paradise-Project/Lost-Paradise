using Content.Shared._LostParadise.Roadmap;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using System.Numerics;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.Controllers;

namespace Content.Client._LostParadise.Roadmap
{
    public sealed class RoadmapUI : UIController
    {
        private DefaultWindow _roadmapWindow; 

        public RoadmapUI()
        {
            _roadmapWindow = new DefaultWindow
            {
                Title = "Roadmap",
                SetSize = new Vector2(600, 400)
            };

            var phaseList = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Vertical,
                SeparationOverride = 10 // Отступ между элементами
            };
            _roadmapWindow.AddChild(phaseList);

            RefreshUI(phaseList);
        }

        private void RefreshUI(BoxContainer phaseList)
        {
            phaseList.RemoveAllChildren();

            var roadmapSystem = IoCManager.Resolve<IPrototypeManager>();
            foreach (var phase in roadmapSystem.EnumeratePrototypes<RoadmapPhasePrototype>())
            {
                var phaseControl = new RoadmapPhaseControl(phase);
                phaseList.AddChild(phaseControl);
            }
        }

        public void UpdatePhase(RoadmapPhasePrototype phase)
        {
            RefreshUI((BoxContainer)_roadmapWindow.GetChild(0));
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
