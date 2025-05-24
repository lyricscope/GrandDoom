using UnityEngine;

public abstract class RoomState : MonoBehaviour
{
    public RoomMotor motor;

    public virtual void Construct()
    {

    }
    public virtual void Destruct()
    {

    }
    public virtual void UpdateState()
    {

    }
}
