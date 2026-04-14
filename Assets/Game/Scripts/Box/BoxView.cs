using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Game.Scripts.Box
{
    [RequireComponent(typeof(Image), typeof(CanvasGroup), typeof(BoxCollider2D), typeof(RectTransform))]
    public class BoxView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Image _image;
        private CanvasGroup _canvasGroup;
        private float _scaleFactor;
        private BoxInteractionService _boxInteractionService;

        public BoxCollider2D BoxCollider { get; private set; }
        public BoxPlacementPoint PlacementPoint { get; private set; } = BoxPlacementPoint.ScrollView;
        public RectTransform RectTransform { get; private set; }
        public Vector3 PositionInTower { get; private set; } = Vector3.zero;
        public bool IsDestroyScheduled { get; private set; }
        public Color Color => _image.color;

        [Inject]
        private void Init(Canvas gameFieldCanvas, BoxInteractionService boxInteractionService)
        {
            _scaleFactor = gameFieldCanvas.scaleFactor;
            _boxInteractionService = boxInteractionService;
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            BoxCollider = GetComponent<BoxCollider2D>();
            RectTransform = GetComponent<RectTransform>();
        }

        public void SetColor(Color color)
        {
            _image.color = color;
        }

        public void SetPlacementPoint(BoxPlacementPoint placementPoint)
        {
            PlacementPoint = placementPoint;
        }

        public void BackToTower(float duration)
        {
            MoveTo(PositionInTower, duration);
        }

        public void PlaceIntoTower(Vector3 point)
        {
            PlacementPoint = BoxPlacementPoint.Tower;
            PositionInTower = point;
        }

        public void MoveBy(Vector2 delta)
        {
            RectTransform.anchoredPosition += delta;
        }

        public void MoveTo(Vector3 point, float duration, Action onComplete = null)
        {
            transform.DOLocalMove(point, duration).OnComplete(() => onComplete?.Invoke());
        }

        public void SnapTo(Vector3 point)
        {
            transform.localPosition = point;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public int GetSiblingIndex()
        {
            return transform.GetSiblingIndex();
        }

        public void SetSiblingIndex(int siblingIndex)
        {
            transform.SetSiblingIndex(siblingIndex);
        }

        public void SetDragState(bool isDragging)
        {
            if (IsDestroyScheduled)
            {
                return;
            }

            _canvasGroup.alpha = isDragging ? 0.8f : 1.0f;
            _canvasGroup.blocksRaycasts = !isDragging;
        }

        public void SmoothDestroy(float duration)
        {
            if (IsDestroyScheduled)
            {
                return;
            }

            IsDestroyScheduled = true;
            BoxCollider.enabled = false;
            _canvasGroup.blocksRaycasts = false;
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, duration)
                .OnComplete(delegate { Destroy(gameObject); });
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _boxInteractionService.HandleBeginDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _boxInteractionService.HandleDrag(this, eventData.delta / _scaleFactor);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _boxInteractionService.HandleEndDrag(this);
        }
    }
}
