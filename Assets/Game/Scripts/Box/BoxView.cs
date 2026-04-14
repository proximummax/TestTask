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
        private BoxController _boxController;
        private BoxModel _model;

        public BoxCollider2D BoxCollider { get; private set; }
        public BoxPlacementPoint PlacementPoint => _model != null ? _model.PlacementPoint : BoxPlacementPoint.ScrollView;
        public RectTransform RectTransform { get; private set; }
        public Vector3 PositionInTower => _model != null ? _model.PositionInTower : Vector3.zero;
        public bool IsDestroyScheduled => _model != null && _model.IsDestroyScheduled;
        public Color Color => _model != null ? _model.Color : _image.color;

        [Inject]
        private void Init(Canvas gameFieldCanvas, BoxController boxController)
        {
            _scaleFactor = gameFieldCanvas.scaleFactor;
            _boxController = boxController;
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            BoxCollider = GetComponent<BoxCollider2D>();
            RectTransform = GetComponent<RectTransform>();
        }

        public void Bind(BoxModel model)
        {
            _model = model;
            _image.color = model.Color;
        }

        public void BackToTower(float duration)
        {
            MoveTo(PositionInTower, duration);
        }

        public void PlaceIntoTower(Vector3 point)
        {
            _model.PlaceIntoTower(point);
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

            _model.MarkDestroyScheduled();
            BoxCollider.enabled = false;
            _canvasGroup.blocksRaycasts = false;
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, duration)
                .OnComplete(delegate { Destroy(gameObject); });
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _boxController.HandleBeginDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _boxController.HandleDrag(this, eventData.delta / _scaleFactor);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _boxController.HandleEndDrag(this);
        }
    }
}
