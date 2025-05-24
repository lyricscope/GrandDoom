using UnityEngine;

namespace Combat
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private int health = 20;

        private void Attack()
        {
            
        }

        public void TakeDamage(int dam)
        {
            if (damage > 0)
            {
                health -= dam;
            }

            if (health <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            Destroy(gameObject);
        }
    }
}