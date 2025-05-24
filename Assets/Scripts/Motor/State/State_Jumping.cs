using UnityEngine;

namespace Epitome
{
    public class State_Jumping : BaseState
    {
	    public override void Construct ()
	    {
		    base.Construct ();
		    motor.VerticalVelocity = motor.JumpForce;
            motor.AirInfluence = motor.DirectionVector * motor.Speed;
            immuneTime = 0.1f;
	    }

	    public override void Destruct ()
	    {
		    base.Destruct ();
	    }

	    public override Vector3 ProcessMotion (Vector3 input)
	    {
            MotorHelper.KillVector(ref input, motor.WallVector);
            MotorHelper.ApplySpeed(ref input, motor.Speed);
            MotorHelper.ApplyGravity(ref input, ref motor.VerticalVelocity, motor.Gravity, motor.TerminalVelocity);
            MotorHelper.InfluenceAirVelocity(ref input,ref motor.AirInfluence, 0.92f);

            return input;
	    }

	    public override void PlayerTransition()
	    {
		    base.PlayerTransition();

            if (motor.VerticalVelocity < 0)
       		    motor.ChangeState ("State_Falling");
        }
    }
}