using System;
using UnityEngine;

namespace Game.Scripts.Storage
{
    [Serializable]
    public struct BoxSaveParameters
    {
        public BoxSaveParameters(Color color, Vector3 position)
        {
            Color = color;
            Position = position;
        }

        public Color Color;
        public Vector3 Position;
    }
}
