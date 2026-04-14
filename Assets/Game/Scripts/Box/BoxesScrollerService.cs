using UnityEngine;

namespace Game.Scripts.Box
{
    public class BoxesScrollerService
    {
        private readonly BoxesScrollerView _boxesScrollerView;
        private readonly Transform _boxesParentAfterDrag;

        public BoxesScrollerService(BoxesScrollerView boxesScrollerView, Transform boxesParentAfterDrag)
        {
            _boxesScrollerView = boxesScrollerView;
            _boxesParentAfterDrag = boxesParentAfterDrag;
        }

        public void SetScrollEnabled(bool isEnabled)
        {
            _boxesScrollerView.SetScrollEnabled(isEnabled);
        }

        public int MoveBoxToDragLayer(BoxView boxView)
        {
            var siblingIndex = boxView.GetSiblingIndex();
            boxView.SetParent(_boxesParentAfterDrag);
            return siblingIndex;
        }
    }
}
