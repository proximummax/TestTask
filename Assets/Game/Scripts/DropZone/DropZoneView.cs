using Game.Scripts.Box;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Game.Scripts.DropZone
{
    public class DropZoneView : MonoBehaviour, IDropHandler
    {
        [SerializeField] private DropZoneType _zoneType;
        private DropZoneInteractionService _dropZoneInteractionService;

        [Inject]
        private void Init(DropZoneInteractionService dropZoneInteractionService)
        {
            _dropZoneInteractionService = dropZoneInteractionService;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag || !eventData.pointerDrag.TryGetComponent(out BoxView boxView))
            {
                return;
            }

            _dropZoneInteractionService.HandleDrop(_zoneType, boxView);
        }
    }
}
