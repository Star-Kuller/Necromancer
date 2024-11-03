using UnityEngine;

namespace Models
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5.0f;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var moveHorizontal = Input.GetAxis("Horizontal");
            var moveVertical = Input.GetAxis("Vertical");
        
            var movement = new Vector2(moveHorizontal, moveVertical).normalized;
        
            _rb.velocity = movement * speed;
        }
    }
}
