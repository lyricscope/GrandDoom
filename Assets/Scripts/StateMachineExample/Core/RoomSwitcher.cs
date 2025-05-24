using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{
    public RoomMotor motor;
    public RoomState state;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            motor.ChangeState(state);
        }
    }
}
