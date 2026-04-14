using Game.Scripts.Storage;
using System.Collections.Generic;

namespace Game.Scripts.Box
{
    public class BoxesPresenter
    {
        private readonly BoxesSpawnerService _boxesSpawnerService;
        private readonly BoxesScrollerService _boxesScrollerService;

        public BoxesPresenter(BoxesSpawnerService boxesSpawnerService, BoxesScrollerService boxesScrollerService)
        {
            _boxesSpawnerService = boxesSpawnerService;
            _boxesScrollerService = boxesScrollerService;
        }

        public void CreateInitialBoxes()
        {
            _boxesSpawnerService.CreateInitialBoxes();
        }

        public BoxView[] CreateBoxesByLoadData(IReadOnlyList<BoxSaveParameters> boxSaveParameters)
        {
            var boxViews = new BoxView[boxSaveParameters.Count];
            for (int i = 0; i < boxSaveParameters.Count; i++)
            {
                var boxSaveParameter = boxSaveParameters[i];
                var boxView = _boxesSpawnerService.CreateBox(boxSaveParameter.Color);
                _boxesScrollerService.MoveBoxToDragLayer(boxView);
                boxViews[i] = boxView;
            }

            return boxViews;
        }
    }
}
