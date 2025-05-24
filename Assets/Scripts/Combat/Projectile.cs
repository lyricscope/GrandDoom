using System;
using UnityEngine;



namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int damage = 10;
        [SerializeField] private float moveSpeed = 100f;
        private Rigidbody _rb;
        
        public int Damage { get { return damage; } set { damage = value; } }

        private float _currentTime = 0f;
        private const float DestroyTime = 10f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();   
        }

        private void FixedUpdate()
        {
            Push();
        }

        private void Update()
        {
            if (_currentTime < DestroyTime)
            {
                _currentTime += Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        private void Push()
        {
            var forward = transform.forward;
            _rb.linearVelocity = forward * (Time.fixedDeltaTime * moveSpeed);
        }
    }
}