using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Box
{
    [RequireComponent(typeof(ScrollRect))]
    public class BoxesScrollerView : MonoBehaviour
    {
        private ScrollRect _scrollRect;

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        public void SetScrollEnabled(bool isEnabled)
        {
            _scrollRect.enabled = isEnabled;
        }
    }
}
