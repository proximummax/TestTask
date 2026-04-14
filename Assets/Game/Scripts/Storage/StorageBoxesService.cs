using System.Collections.Generic;
using Game.Scripts.Box;
using Game.Scripts.DropZone;
using System;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.Storage
{
    public class StorageBoxesService : IStartable, IDisposable
    {
        private readonly TowerZoneService _towerZoneService;
        private readonly BoxesPresenter _boxesPresenter;
        private readonly IBoxesPersistence _persistence;

        private const string SaveFileName = "save";

        public StorageBoxesService(TowerZoneService towerZoneService, BoxesPresenter boxesPresenter, IBoxesPersistence persistence)
        {
            _boxesPresenter = boxesPresenter;
            _towerZoneService = towerZoneService;
            _persistence = persistence;
        }

        public void Start()
        {
            Application.quitting += SaveGame;

            var boxesParameters = _persistence.Load(SaveFileName);
            if (boxesParameters.Count == 0)
            {
                _boxesPresenter.CreateInitialBoxes();
                return;
            }

            var boxes = _boxesPresenter.CreateBoxesByLoadData(boxesParameters);
            for (int i = 0; i < boxes.Length; i++)
            {
                _towerZoneService.PlaceLoadedBox(boxes[i], boxesParameters[i].Position);
            }
        }

        public void SaveGame()
        {
            var saveData = new List<BoxSaveParameters>(_towerZoneService.Tower.Count);
            for (int i = 0; i < _towerZoneService.Tower.Count; i++)
            {
                var box = _towerZoneService.Tower[i];
                saveData.Add(new BoxSaveParameters(box.Color, box.PositionInTower));
            }

            _persistence.Save(SaveFileName, saveData);
        }

        public void Dispose()
        {
            Application.quitting -= SaveGame;
        }
    }
}
