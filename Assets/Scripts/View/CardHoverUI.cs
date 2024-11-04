using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View
{
    public class CardHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _image;
        private Vector3 _position;
        private Color _originalColor;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _originalColor = _image.color;
            _image.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.color = _originalColor;
        }
    }
}