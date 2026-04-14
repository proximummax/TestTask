using Game.Scripts.Notifications;
using Game.Scripts.ScriptableObjects;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Box
{
    public class BoxesSpawnerService
    {
        private readonly INotificationService _notificationService;
        private readonly Color[] _boxColors;
        private readonly int _boxCount;
        private readonly System.Func<BoxView> _boxFactory;

        public BoxesSpawnerService(
            INotificationService notificationService,
            GameConfig config,
            System.Func<BoxView> boxFactory)
        {
            _notificationService = notificationService;
            _boxColors = config.BoxColors;
            _boxCount = config.BoxesCount;
            _boxFactory = boxFactory;
        }

        public BoxView CreateBox(Color color)
        {
            var boxView = _boxFactory();
            boxView.SetColor(color);
            boxView.SetPlacementPoint(BoxPlacementPoint.ScrollView);
            return boxView;
        }

        public void CreateInitialBoxes()
        {
            for (int i = 0; i < _boxCount; i++)
            {
                CreateBox(_boxColors[i % _boxColors.Length]);
            }

            _notificationService.Show(AppMessages.BoxesCreated);
        }
    }
}
