using Game.Scripts.ScriptableObjects;
using UnityEngine;

namespace Game.Scripts.Box
{
    public class BoxController
    {
        private readonly BoxesScrollerService _boxesScrollerService;
        private readonly BoxesSpawnerService _boxesSpawnerService;
        private readonly float _hideDuration;

        public BoxController(
            BoxesScrollerService boxesScrollerService,
            BoxesSpawnerService boxesSpawnerService,
            GameConfig gameConfig)
        {
            _boxesScrollerService = boxesScrollerService;
            _boxesSpawnerService = boxesSpawnerService;
            _hideDuration = gameConfig.HideDuration;
        }

        public void HandleBeginDrag(BoxView boxView)
        {
            if (boxView.IsDestroyScheduled)
            {
                return;
            }

            if (boxView.PlacementPoint == BoxPlacementPoint.ScrollView)
            {
                DuplicateDraggedScrollBox(boxView);
            }

            _boxesScrollerService.SetScrollEnabled(false);
            boxView.SetDragState(true);
        }

        public void HandleDrag(BoxView boxView, Vector2 delta)
        {
            if (boxView.IsDestroyScheduled)
            {
                return;
            }

            boxView.MoveBy(delta);
        }

        public void HandleEndDrag(BoxView boxView)
        {
            _boxesScrollerService.SetScrollEnabled(true);

            if (boxView.IsDestroyScheduled)
            {
                return;
            }

            if (boxView.PlacementPoint != BoxPlacementPoint.Tower)
            {
                boxView.SmoothDestroy(_hideDuration);
                return;
            }

            boxView.SetDragState(false);
        }

        private void DuplicateDraggedScrollBox(BoxView draggedBox)
        {
            var siblingIndex = _boxesScrollerService.MoveBoxToDragLayer(draggedBox);
            var replacementBox = _boxesSpawnerService.CreateBox(draggedBox.Color);
            replacementBox.SetSiblingIndex(siblingIndex);
        }
    }
}
