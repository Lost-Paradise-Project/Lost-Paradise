using Content.Shared._LostParadise.Roadmap;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using System.Numerics;

namespace Content.Client._LostParadise.Roadmap
{
    public class RoadmapPhaseControl : Control
    {
        private readonly RoadmapPhasePrototype _prototype;
        private readonly ProgressBar _progressBar; // Прогресс-бар как поле класса

        public RoadmapPhaseControl(RoadmapPhasePrototype prototype)
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
            Margin = new Thickness(0, 20, 0, 0); // Отступ для spacing

            var vBox = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Vertical
            };

            var nameBackground = new PanelContainer
            {
                // Используем PanelContainer как фон
                Margin = new Thickness(0, 0, 0, 10) // Отступ между именем и другими элементами
            };

            var nameLabel = new Label
            {
                Text = _prototype.Name,
                FontColorOverride = Color.White
            };

            nameBackground.AddChild(nameLabel); // Добавление имени в фон
            vBox.AddChild(nameBackground); // Добавление фона в контейнер

            // Остальные элементы
            var descriptionLabel = new Label { Text = _prototype.Description, FontColorOverride = Color.LightGray };
            var progressLabel = new Label { Text = $"Прогресс: {_prototype.Progress}%", FontColorOverride = Color.White };
            var releaseDateLabel = new Label { Text = $"Дата релиза: {_prototype.ReleaseDate}", FontColorOverride = Color.LightGray };

            var statusBox = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Horizontal,
                HorizontalExpand = true,
                SeparationOverride = 5 // Расстояние между статусом и цветным прямоугольником
            };

            var statusLabel = new Label
            {
                Text = $"Статус: {_prototype.Status}",
                FontColorOverride = GetStatusColor(),
                HorizontalAlignment = HAlignment.Right // Сдвигаем текст вправо
            };

            var statusIndicator = new PanelContainer
            {
                // Установка цвета через Modulate
                HorizontalExpand = true,
                MinSize = new Vector2(20, 20), // Установка минимального размера для цветного прямоугольника
                VerticalExpand = false // Прямоугольник не будет растягиваться по вертикали
            };

            // Задаем цвет в зависимости от статуса
            statusIndicator.Modulate = GetStatusColor();

            statusBox.AddChild(statusLabel);
            statusBox.AddChild(statusIndicator);

            // Добавление остальных элементов
            vBox.AddChild(descriptionLabel);
            vBox.AddChild(progressLabel);
            vBox.AddChild(_progressBar);
            vBox.AddChild(releaseDateLabel);
            vBox.AddChild(statusBox); // Добавление статуса в контейнер

            // Добавление разделителя
            var separator = new PanelContainer
            {
                // Создание разделителя без StyleBox
                Modulate = new Color(0.5f, 0.5f, 0.5f, 1f), // Цвет разделителя
                MinSize = new Vector2(0, 2), // Высота разделителя
                HorizontalExpand = true
            };
            vBox.AddChild(separator);

            AddChild(vBox); // Добавление вертикального контейнера в класс
        }

        private Color GetStatusColor()
        {
            return _prototype.Status switch
            {
                "roadmap-goal-completed" => new Color(0.0f, 1.0f, 0.0f),
                "roadmap-goal-progress" => new Color(1.0f, 1.0f, 0.0f),
                "roadmap-goal-waiting" => new Color(1.0f, 0.5f, 0.0f),
                _ => Color.White
            };
        }
    }
}
