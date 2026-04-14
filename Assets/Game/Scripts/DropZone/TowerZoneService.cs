using System.Collections.Generic;
using Game.Scripts.Box;
using Game.Scripts.Notifications;
using Game.Scripts.ScriptableObjects;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.DropZone
{
    public class TowerZoneService
    {
        private readonly INotificationService _notificationService;
        private readonly float _fallDownDuration;
        private readonly float _hideDuration;
        private readonly List<BoxView> _tower = new();
        private readonly float _topScreenY;
        private readonly bool _hasTopScreenBoundary;

        public IReadOnlyList<BoxView> Tower => _tower;

        public TowerZoneService(GameConfig gameConfig, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _fallDownDuration = gameConfig.FallDownDuration;
            _hideDuration = gameConfig.HideDuration;

            if (Camera.main != null)
            {
                _topScreenY = Camera.main
                    .ScreenToWorldPoint(new Vector3(0f, Screen.height, Mathf.Abs(Camera.main.transform.position.z))).y;
                _hasTopScreenBoundary = true;
            }
        }

        public void PlaceLoadedBox(BoxView box, Vector3 position)
        {
            PlaceBox(box, position, false);
        }

        public void TryPlaceBox(BoxView boxToPlace)
        {
            if (_tower.Count == 0)
            {
                PlaceBox(boxToPlace, boxToPlace.transform.localPosition, false);
                return;
            }

            if (TryGetPlacementAboveTower(boxToPlace, out var positionToMove))
            {
                PlaceBox(boxToPlace, positionToMove, true);
                return;
            }

            boxToPlace.SmoothDestroy(_hideDuration);
            _notificationService.Show(AppMessages.ErrorPlaced);
        }

        public void RemoveBoxFromTower(BoxView boxToRemove)
        {
            if (_tower.Count == 0)
            {
                return;
            }

            var removedBoxIndex = _tower.IndexOf(boxToRemove);
            if (removedBoxIndex < 0)
            {
                return;
            }

            DestroyTowerBoxAt(removedBoxIndex);
            MoveBlocksDown(removedBoxIndex);
        }

        private void MoveBlocksDown(int startBlock)
        {
            for (int i = startBlock; i < _tower.Count; i++)
            {
                var boxToPlace = _tower[i];
                if (TryGetFallPosition(boxToPlace, out var positionToMove))
                {
                    PlaceBox(boxToPlace, positionToMove, true);
                }
                else
                {
                    DestroyTowerBoxAt(i);
                    i--;
                }
            }
        }

        private void PlaceBox(BoxView box, Vector3 position, bool animate)
        {
            _notificationService.Show(AppMessages.BoxPlaced);

            if (animate)
            {
                box.MoveTo(position, _fallDownDuration, () => NotifyIfTopReached(box));
            }
            else
            {
                box.SnapTo(position);
                NotifyIfTopReached(box);
            }

            box.PlaceIntoTower(position);
            if (!_tower.Contains(box))
            {
                _tower.Add(box);
            }
        }

        private void DestroyTowerBoxAt(int index)
        {
            var boxToRemove = _tower[index];
            _tower.RemoveAt(index);
            boxToRemove.SmoothDestroy(_hideDuration);
            _notificationService.Show(AppMessages.BoxRemoved);
        }

        private bool TryGetPlacementAboveTower(BoxView boxToPlace, out Vector3 positionToMove)
        {
            positionToMove = default;

            RaycastHit2D hit = Physics2D.Raycast(boxToPlace.transform.position, Vector2.down);
            if (!hit || !hit.collider.gameObject.TryGetComponent(out BoxView underBox))
            {
                return false;
            }

            if (_tower.Count == 0 || _tower[^1] != underBox)
            {
                return false;
            }

            positionToMove = new Vector3(
                boxToPlace.transform.localPosition.x,
                underBox.PositionInTower.y + underBox.RectTransform.rect.height,
                boxToPlace.transform.localPosition.z);

            return true;
        }

        private bool TryGetFallPosition(BoxView boxToPlace, out Vector3 positionToMove)
        {
            positionToMove = default;

            RaycastHit2D hit = Physics2D.Raycast(boxToPlace.transform.position, Vector2.down);
            if (!hit || !hit.collider.gameObject.TryGetComponent(out BoxView underBox))
            {
                return false;
            }

            if (!_tower.Contains(underBox))
            {
                return false;
            }

            var boxToMoveIndex = _tower.IndexOf(boxToPlace);
            var boxUnderIndex = _tower.IndexOf(underBox);
            if (Mathf.Abs(boxToMoveIndex - boxUnderIndex) != 1)
            {
                return false;
            }

            positionToMove = new Vector3(
                boxToPlace.transform.localPosition.x,
                underBox.PositionInTower.y + underBox.RectTransform.rect.height,
                boxToPlace.transform.localPosition.z);

            return true;
        }

        private void NotifyIfTopReached(BoxView box)
        {
            if (IsBoxTouchTopOfScreen(box))
            {
                _notificationService.Show(AppMessages.TopScreenReached);
            }
        }

        private bool IsBoxTouchTopOfScreen(BoxView box)
        {
            return _hasTopScreenBoundary &&
                   box.transform.position.y + box.BoxCollider.bounds.extents.y >= _topScreenY;
        }
    }
}
