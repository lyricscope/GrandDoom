using Epitome;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMotor : BaseMotor
{
    public NavMeshAgent enemy;
    public GameObject target;
    public UnityEngine.Vector3 offset = new UnityEngine.Vector3(0, 0, 0);
    protected override void UpdateMotor()
        {
            enemy.SetDestination(target.transform.position + offset);
        }
}
