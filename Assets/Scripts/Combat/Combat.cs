using System;
using UnityEngine;

namespace Combat
{
    public class Combat : MonoBehaviour
    {
        [SerializeField] private Projectile projectile;
        [SerializeField] private Transform from;

        private void Update()
        {
            Attack();
        }

        private void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var proj = Instantiate(projectile, from.position, from.rotation);
            }
        }
    }
}

