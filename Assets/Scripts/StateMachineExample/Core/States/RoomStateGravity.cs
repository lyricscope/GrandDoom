using Epitome;
using UnityEngine;

public class RoomStateGravity : RoomState
{
    public override void Construct()
    {
        motor.player.ChangeState("State_Flying");
    }

    public override void Destruct()
    {
        motor.player.ChangeState("State_Walking");
    }
}
