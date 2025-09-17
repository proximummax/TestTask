using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Game.Scripts.Box
{
    public class BoxView : MonoBehaviour
    {
        public enum EBoxPlacementPoint
        {
            ScrollView,
            Tower
        }

        private Image _image;
        private CanvasGroup _canvasGroup;
        private float _scaleFactor;
        
        public BoxCollider2D BoxCollider { get; private set; }
        public EBoxPlacementPoint PlacementPoint { get; set; }
        public RectTransform RectTransform { get; private set; }
        public Vector3 PositionInTower { get; private set; } = Vector3.zero;
        public Color Color
        {
            get => _image.color;
            set => _image.color = value;
        }

        public ObservableBeginDragTrigger BeginDragTrigger { get; private set; }
        public ObservableEndDragTrigger EndDragTrigger { get; private set; }
        public ReactiveProperty<BoxView> Duplicate { get; private set; } = new();

        [Inject]
        private void Init(Canvas gameFieldCanvas)
        {
            _scaleFactor = gameFieldCanvas.scaleFactor;
            _image = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            BoxCollider = GetComponent<BoxCollider2D>();

            RectTransform = GetComponent<RectTransform>();
            BeginDragTrigger = this.AddComponent<ObservableBeginDragTrigger>();
            BeginDragTrigger.OnBeginDragAsObservable().Subscribe(OnBeginDrag).AddTo(this);

            EndDragTrigger = this.AddComponent<ObservableEndDragTrigger>();
            EndDragTrigger.OnEndDragAsObservable().Subscribe(OnEndDrag).AddTo(this);
            this.AddComponent<ObservableDragTrigger>().OnDragAsObservable().Subscribe(OnDrag).AddTo(this);
        }
        
        public void BackToTower(float duration)
        {
            transform.DOLocalMove(PositionInTower, duration);
        }

        public void SaveMoveEndPoint(Vector3 point)
        {
            PositionInTower = point;
        }
        
        public void SmoothDestroy(float duration)
        {
            BoxCollider.enabled = false;
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, duration)
                .OnComplete(delegate { Destroy(gameObject); });
        }

        private void OnBeginDrag(PointerEventData eventData)
        {
            if (PlacementPoint == EBoxPlacementPoint.ScrollView)
            {
                Duplicate.Value = this;
            }

            _canvasGroup.alpha = 0.8f;
            _canvasGroup.blocksRaycasts = false;
        }

        private void OnEndDrag(PointerEventData eventData)
        {
            if (PlacementPoint != EBoxPlacementPoint.Tower)
            {
                SmoothDestroy(1.5f);
                return;
            }

            _canvasGroup.alpha = 1.0f;
            _canvasGroup.blocksRaycasts = true;
        }

        private void OnDrag(PointerEventData eventData)
        {
            RectTransform.anchoredPosition += eventData.delta / _scaleFactor;
        }
    }
}