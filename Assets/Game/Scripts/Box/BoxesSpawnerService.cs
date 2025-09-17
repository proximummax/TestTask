using System;
using System.Collections.Generic;
using Game.Scripts.Notifications;
using Game.Scripts.ScriptableObjects;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Box
{
    public class BoxesSpawnerService
    {
        private readonly NotificationService _notificationService;
        private readonly Color[] _boxColors;
        private readonly int _boxCount;
        private readonly Func<Color, BoxView> _boxFactory;

        private List<BoxView> _boxes;

        public BoxesSpawnerService(NotificationService notificationService,
            GameConfig config,
            Func<Color, BoxView> boxFactory)
        {
            _notificationService = notificationService;
            _boxColors = config.BoxColors;
            _boxCount = config.BoxesCount;
            _boxFactory = boxFactory;
        }

        public BoxView CreateBox(Color color)
        {
            return _boxFactory(color);
        }

        public BoxView[] CreateBoxes()
        {
            BoxView[] boxes = new BoxView[_boxCount];
            for (int i = 0; i < _boxCount; i++)
            {
                boxes[i] = CreateBox(_boxColors[i % _boxColors.Length]);
            }

            _notificationService.NotificationMessage.Value = AppMessages.BOX_CREATED_MESSAGE;
            return boxes;
        }
    }
}