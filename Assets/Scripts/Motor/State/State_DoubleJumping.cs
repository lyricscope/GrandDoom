using UnityEngine;

namespace Epitome
{
    public class State_DoubleJumping : BaseState
    {
        public override void Construct()
        {
            base.Construct();
            motor.VerticalVelocity = motor.JumpForce;
            motor.AirExhausted = true;
            immuneTime = 0.1f;
        }

        public override void Destruct()
        {
            base.Destruct();
        }

        public override Vector3 ProcessMotion(Vector3 input)
        {
            MotorHelper.KillVector(ref input, motor.WallVector);
            MotorHelper.ApplySpeed(ref input, motor.Speed);
            MotorHelper.ApplyGravity(ref input, ref motor.VerticalVelocity, motor.Gravity, motor.TerminalVelocity);

            return input;
        }

        public override void PlayerTransition()
        {
            base.PlayerTransition();

            if (motor.VerticalVelocity < 0)
                motor.ChangeState("State_Falling");
        }
    }
}