using UnityEngine;
namespace Epitome
{
    public class State_Falling : BaseState
    {
        public override void Construct()
        {
            base.Construct();
            motor.AirInfluence = motor.DirectionVector * motor.Speed;
        }

        public override void Destruct()
        {

        }

        public override Vector3 ProcessMotion(Vector3 input)
        {
            MotorHelper.KillVector(ref input, motor.WallVector);
            MotorHelper.ApplySpeed(ref input, motor.Speed);
            MotorHelper.ApplyGravity(ref input, ref motor.VerticalVelocity, motor.Gravity, motor.TerminalVelocity);
            MotorHelper.InfluenceAirVelocity(ref input, ref motor.AirInfluence, 0.92f);

            return input;
        }

        public override void PlayerTransition()
        {
            base.PlayerTransition();

            if (motor.IsGrounded)
                motor.ChangeState("State_Walking");
        }
    }
}