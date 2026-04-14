using UnityEngine;

namespace Game.Scripts.Box
{
    public class BoxModel
    {
        public BoxModel(Color color)
        {
            Color = color;
            PlacementPoint = BoxPlacementPoint.ScrollView;
            PositionInTower = Vector3.zero;
        }

        public Color Color { get; }
        public BoxPlacementPoint PlacementPoint { get; private set; }
        public Vector3 PositionInTower { get; private set; }
        public bool IsDestroyScheduled { get; private set; }

        public void PlaceIntoTower(Vector3 point)
        {
            PlacementPoint = BoxPlacementPoint.Tower;
            PositionInTower = point;
        }

        public void MarkDestroyScheduled()
        {
            IsDestroyScheduled = true;
        }
    }
}
