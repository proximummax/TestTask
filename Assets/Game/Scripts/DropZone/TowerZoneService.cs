using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Box;
using Game.Scripts.Notifications;
using Game.Scripts.ScriptableObjects;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.DropZone
{
    public class TowerZoneService
    {
        private readonly NotificationService _notificationService;
        private readonly float _fallDownDuration;
        private readonly float _hideDuration;

        private readonly Vector3 _topOfScreen = Vector3.zero;
        public List<BoxView> Tower { get; private set; } = new();

        public TowerZoneService(GameConfig gameConfig, NotificationService notificationService)
        {
            _notificationService = notificationService;
            _fallDownDuration = gameConfig.FallDownDuration;
            _hideDuration = gameConfig.HideDuration;

            if (Camera.main != null)
            {
                _topOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            }
        }

        public void PlaceBoxInTower(BoxView box, Vector3 position, bool permanent)
        {
            _notificationService.NotificationMessage.Value = AppMessages.BOX_PLACED_MESSAGE;

            if (permanent)
            {
                box.transform.localPosition = position;
                if (IsBoxTouchTopOfScreen(box))
                {
                    _notificationService.NotificationMessage.Value = AppMessages.TOP_SCREEN_MESSAGE;
                }
            }
            else
            {
                box.transform.DOLocalMove(position, _fallDownDuration).OnComplete(delegate
                {
                    if (IsBoxTouchTopOfScreen(box))
                    {
                        _notificationService.NotificationMessage.Value = AppMessages.TOP_SCREEN_MESSAGE;
                    }
                });
            }

            box.PlacementPoint = BoxView.EBoxPlacementPoint.Tower;
            box.SaveMoveEndPoint(position);

            if (!Tower.Contains(box))
            {
                Tower.Add(box);
            }
        }

        private bool IsBoxTouchTopOfScreen(BoxView box)
        {
            return box.transform.position.y + box.BoxCollider.bounds.extents.y >= _topOfScreen.y;
        }

        public void TryPlaceBox(BoxView boxToPlace)
        {
            if (Tower.Count <= 0)
            {
                PlaceBoxInTower(boxToPlace, boxToPlace.transform.localPosition, true);
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(boxToPlace.transform.position, -Vector2.up);

                if (hit && hit.collider.gameObject.TryGetComponent(out BoxView underBox) &&
                    IsBoxCanPlaceAtTopOfTower(boxToPlace, underBox))
                {
                    Vector3 positionToMove = new Vector3(boxToPlace.transform.localPosition.x,
                        underBox.transform.localPosition.y + underBox.RectTransform.rect.height,
                        boxToPlace.transform.localPosition.z);

                    PlaceBoxInTower(boxToPlace, positionToMove, false);
                }
                else
                {
                    boxToPlace.SmoothDestroy(_hideDuration);
                    _notificationService.NotificationMessage.Value = AppMessages.ERROR_PLACED_MESSAGE;
                }
            }
        }

        public void RemoveBoxFromTower(BoxView boxToRemove)
        {
            if (Tower.Count <= 0)
                return;

            RemoveBoxFromTower(boxToRemove, out int stashedBoxIndex);
            MoveBlocksDown(stashedBoxIndex);
        }

        private void RemoveBoxFromTower(BoxView boxToRemove, out int removedBoxIndex)
        {
            removedBoxIndex = Tower.IndexOf(boxToRemove);
            Tower.RemoveAt(removedBoxIndex);
            boxToRemove.SmoothDestroy(_hideDuration);

            _notificationService.NotificationMessage.Value = AppMessages.BOX_REMOVED_MESSAGE;
        }

        private void MoveBlocksDown(int startBlock)
        {
            for (int i = startBlock; i < Tower.Count; i++)
            {
                var boxToPlace = Tower[i];

                RaycastHit2D hit = Physics2D.Raycast(boxToPlace.transform.position, -Vector2.up);

                if (hit && hit.collider.gameObject.TryGetComponent(out BoxView underBox) &&
                    CanBoxMoveDown(boxToPlace, underBox))
                {
                    float posY = underBox.PositionInTower == Vector3.zero
                        ? hit.transform.localPosition.y + underBox.RectTransform.rect.height
                        : underBox.PositionInTower.y + underBox.RectTransform.rect.height;

                    Vector3 positionToMove = new Vector3(boxToPlace.transform.localPosition.x, posY,
                        boxToPlace.transform.localPosition.z);

                    PlaceBoxInTower(boxToPlace, positionToMove, false);
                }
                else
                {
                    RemoveBoxFromTower(boxToPlace, out int removedBoxIndex);
                    i--;
                }
            }
        }


        private bool CanBoxMoveDown(BoxView boxToMove, BoxView boxUnder)
        {
            int boxToMoveIndex = Tower.IndexOf(boxToMove);
            int boxUnderIndex = Tower.IndexOf(boxUnder);

            return IsBoxesCanPlaceInTower() && Tower.Contains(boxUnder) &&
                   Mathf.Abs(boxToMoveIndex - boxUnderIndex) == 1;
        }

        private bool IsBoxCanPlaceAtTopOfTower(BoxView boxToPlace, BoxView boxUnder)
        {
            return IsBoxesCanPlaceInTower() && Tower[^1] == boxUnder;
            //TODO: For example check the color
        }

        private bool IsBoxesCanPlaceInTower()
        {
            return Tower != null;
        }
    }
}