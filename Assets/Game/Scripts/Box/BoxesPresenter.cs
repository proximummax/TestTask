using System.Collections.Generic;
using Game.Scripts.Storage;
using UniRx;
using VContainer.Unity;

namespace Game.Scripts.Box
{
    public class BoxesPresenter : IStartable
    {
        private readonly BoxesScrollerService _boxesScrollerService;
        private readonly BoxesSpawnerService _boxesSpawnerService;

        public BoxesPresenter(BoxesScrollerService boxesScrollerService, BoxesSpawnerService boxesSpawnerService)
        {
            _boxesScrollerService = boxesScrollerService;
            _boxesSpawnerService = boxesSpawnerService;
        }

        public void Start()
        {
            CreateScrollViewBoxesAndSubscribeEvents();
        }
        
        public BoxView[] CreateBoxesByLoadData(List<BoxSaveParameters> boxSaveParameters)
        {
            BoxView[] boxViews = new BoxView[boxSaveParameters.Count];
            for (int i = 0; i < boxSaveParameters.Count; i++)
            {
                var boxSaveParameter = boxSaveParameters[i];
                var box = _boxesSpawnerService.CreateBox(boxSaveParameter.Color);
                SubscribeBoxEvents(box);
                
                _boxesScrollerService.SetBoxNewParent(box, out var siblingIndex);
                boxViews[i] = box;
            }

            return boxViews;
        }

        private void CreateScrollViewBoxesAndSubscribeEvents()
        {
            var boxes = _boxesSpawnerService.CreateBoxes();
            foreach (var box in boxes)
            {
                SubscribeBoxEvents(box);
            }
        }

        private void SubscribeBoxEvents(BoxView boxView)
        {
            boxView.BeginDragTrigger.OnBeginDragAsObservable().Subscribe(_boxesScrollerService.OnBoxBeginDrag)
                .AddTo(boxView);
            boxView.EndDragTrigger.OnEndDragAsObservable().Subscribe(_boxesScrollerService.OnBoxEndDrag)
                .AddTo(boxView);
            boxView.Duplicate.Where(x => x != null)
                .Subscribe(PlaceBoxViewDuplicate).AddTo(boxView);
        }

        private void PlaceBoxViewDuplicate(BoxView boxView)
        {
            _boxesScrollerService.SetBoxNewParent(boxView, out var oldSiblingIndex);
            var newBox = _boxesSpawnerService.CreateBox(boxView.Color);
            SubscribeBoxEvents(newBox);
            newBox.transform.SetSiblingIndex(oldSiblingIndex);
        }
    }
}