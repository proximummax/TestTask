using UnityEngine;
using UnityEngine.EventSystems;

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

        public void OnBoxBeginDrag(PointerEventData eventData)
        {
            SetScrollAvailableState(false);
        }

        public void OnBoxEndDrag(PointerEventData eventData)
        {
            SetScrollAvailableState(true);
        }

        public void SetBoxNewParent(BoxView boxView,out int oldSiblingIndex)
        {
            oldSiblingIndex = boxView.transform.GetSiblingIndex();
            boxView.transform.SetParent(_boxesParentAfterDrag);
        }
        
        private void SetScrollAvailableState(bool available)
        {
            _boxesScrollerView.SetScrollAvailableState(available);
        }
    }
}