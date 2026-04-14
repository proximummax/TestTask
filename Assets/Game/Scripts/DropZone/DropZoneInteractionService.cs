using Game.Scripts.Box;
using Game.Scripts.Notifications;
using Game.Scripts.ScriptableObjects;
using Game.Scripts.Utils;

namespace Game.Scripts.DropZone
{
    public class DropZoneInteractionService
    {
        private readonly TowerZoneService _towerZoneService;
        private readonly INotificationService _notificationService;
        private readonly float _hideDuration;
        private readonly float _backToTowerDuration;

        public DropZoneInteractionService(
            GameConfig gameConfig,
            TowerZoneService towerZoneService,
            INotificationService notificationService)
        {
            _towerZoneService = towerZoneService;
            _notificationService = notificationService;
            _hideDuration = gameConfig.HideDuration;
            _backToTowerDuration = gameConfig.BackToTowerDuration;
        }

        public void HandleDrop(DropZoneType zoneType, BoxView boxView)
        {
            switch (zoneType)
            {
                case DropZoneType.Tower:
                    HandleTowerDrop(boxView);
                    break;
                case DropZoneType.Stash:
                    HandleStashDrop(boxView);
                    break;
                case DropZoneType.ScrollView:
                    HandleScrollViewDrop(boxView);
                    break;
            }
        }

        private void HandleTowerDrop(BoxView boxView)
        {
            if (boxView.PlacementPoint == BoxPlacementPoint.Tower)
            {
                boxView.BackToTower(_backToTowerDuration);
                _notificationService.Show(AppMessages.AlreadyConnected);
                return;
            }

            _towerZoneService.TryPlaceBox(boxView);
        }

        private void HandleStashDrop(BoxView boxView)
        {
            if (boxView.PlacementPoint != BoxPlacementPoint.Tower)
            {
                boxView.SmoothDestroy(_hideDuration);
                return;
            }

            _towerZoneService.RemoveBoxFromTower(boxView);
        }

        private void HandleScrollViewDrop(BoxView boxView)
        {
            if (boxView.PlacementPoint == BoxPlacementPoint.Tower)
            {
                boxView.BackToTower(_backToTowerDuration);
            }
            else
            {
                boxView.SmoothDestroy(_hideDuration);
            }

            _notificationService.Show(AppMessages.ErrorPlaced);
        }
    }
}
