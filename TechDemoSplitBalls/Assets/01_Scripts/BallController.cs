using System;

namespace UnityEngine
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallController : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        [SerializeField] 
        private float _maxSpeed;
        public Rigidbody Rigidbody;
        public Collider Collider;
        
        private Vector3 _velocity;

        private void Awake()
        {
            //Rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // Rigidbody.AddForce(Vector3.right*InputManager.Instance.DragValue*_speed);
            // Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, _maxSpeed);
            _velocity = Rigidbody.velocity;
            _velocity.x = InputManager.Instance.DragValue*_speed;
            Rigidbody.velocity = _velocity;
        }
    }
}