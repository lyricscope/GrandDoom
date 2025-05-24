using System;
using System.Collections;
using Cinemachine;
using Epitome;
using UnityEngine;

namespace Combat
{
    public class Combat : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
        [SerializeField] private Projectile projectile;
        [SerializeField] private Transform from;
        [SerializeField] private PlayerMotor motor;
        [SerializeField] private Transform child;

        private void Update()
        {
            Attack();
        }

        private void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var quaternion = Quaternion.Euler(0, cinemachineFreeLook.State.FinalOrientation.eulerAngles.y, 0);
                child.rotation = quaternion;
                Instantiate(projectile, from.position, quaternion);
            }
        }
    }
}

