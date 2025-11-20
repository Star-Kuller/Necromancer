using UnityEngine;

namespace View
{
    public class PlayerMovementView : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private SpriteRenderer _render;
        private Animator _animator;

        private void Start()
        {
            _render = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var moveHorizontal = Input.GetAxis("Horizontal");
            var absMoveHorizontal = Mathf.Abs(moveHorizontal);
            var absMoveVertical = Mathf.Abs(Input.GetAxis("Vertical"));
            var isMoving = absMoveHorizontal > 0.2 || absMoveVertical > 0.1;
            _animator.SetBool(IsMoving, isMoving);
            
            _render.flipX = moveHorizontal switch
            {
                > 0 => false,
                < 0 => true,
                _ => _render.flipX
            };
        }
    }
}
