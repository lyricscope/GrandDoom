using Epitome;
using UnityEngine;

public class RoomMotor : MonoBehaviour
{
    public RoomState initialState;
    public RoomState activeState;

    public PlayerMotor player;

    private void Start()
    {
        activeState = initialState;
        activeState.Construct();
    }

    private void Update()
    {
        activeState.UpdateState();
    }

    public void ChangeState(RoomState state)
    {
        activeState.Destruct();
        activeState = state;
        activeState.Construct();
    }
}
