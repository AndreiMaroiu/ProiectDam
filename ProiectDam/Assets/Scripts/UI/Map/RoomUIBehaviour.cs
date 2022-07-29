using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI.Map
{
    public class RoomUIBehaviour : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _icon;

        private RectTransform _rect;


        public RectTransform Rect => _rect.IsNull() ? _rect = GetComponent<RectTransform>() : _rect;

        public void Set(Sprite icon)
        {
            _icon.sprite = icon;
            _icon.enabled = icon != null;
        }

        public void SetActive(Color color)
        {
            _image.color = color;
            gameObject.SetActive(true);
        }
    }
}
