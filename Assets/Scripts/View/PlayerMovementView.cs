using UnityEngine;

namespace View
{
    public class PlayerMovementView : MonoBehaviour
    {
        private SpriteRenderer _render;

        private void Start()
        {
            _render = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var moveHorizontal = Input.GetAxis("Horizontal");

            _render.flipX = moveHorizontal switch
            {
                > 0 => false,
                < 0 => true,
                _ => _render.flipX
            };
        }
    }
}
