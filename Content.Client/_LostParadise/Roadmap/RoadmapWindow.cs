using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using System.Numerics;

namespace Content.Client._LostParadise.Roadmap
{
    public sealed class RoadmapWindow : DefaultWindow
    {
        private readonly RoadmapManager _roadmapManager;

        public RoadmapWindow()
        {
            Title = "Roadmap";
            SetSize = new Vector2(600, 400);

            var vbox = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Vertical
            };

            // Получаем менеджер дорожной карты через IoC
            _roadmapManager = IoCManager.Resolve<RoadmapManager>();

            // Загружаем и отображаем информацию
            var roadmapEntries = _roadmapManager.LoadRoadmap();
            foreach (var entry in roadmapEntries)
            {
                var entryLabel = new Label
                {
                    Text = $"{entry.Name}\n{entry.Description}\nProgress: {entry.Progress}%\nRelease Date: {entry.ReleaseDate}\nStatus: {entry.Status}\n"
                };
                vbox.AddChild(entryLabel);
            }

            Contents.AddChild(vbox);
        }
    }
}
