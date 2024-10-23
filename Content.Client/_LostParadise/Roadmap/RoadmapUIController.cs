using Robust.Client.UserInterface.Controllers;

namespace Content.Client._LostParadise.Roadmap
{
    public sealed class RoadmapUIController : UIController
    {
        private RoadmapWindow? _roadmapWindow;

        public void ToggleRoadmap()
        {
            if (_roadmapWindow == null)
            {
                _roadmapWindow = UIManager.CreateWindow<RoadmapWindow>();
            }

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