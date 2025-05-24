using UnityEngine;

namespace Epitome
{
    public class State_Walking : BaseState
    {
        public override void Construct()
        {
            base.Construct();
            motor.AirExhausted = false;
        }

        public override void Destruct()
        {
            base.Destruct();
        }

        public override Vector3 ProcessMotion(Vector3 input)
        {
            MotorHelper.KillVector(ref input, motor.WallVector);
            MotorHelper.FollowVector(ref input, motor.SlopeNormal);
            MotorHelper.ApplySpeed(ref input, motor.Speed);
            MotorHelper.SmoothLanding(ref motor.VerticalVelocity, -3, 0.1f);

            input.y = motor.VerticalVelocity;
            return input;
        }

        public override void PlayerTransition()
        {
            base.PlayerTransition();

            if (!motor.IsGrounded)
                motor.ChangeState("State_Falling");

            if (InputManager.Instance.Jump)
                motor.ChangeState("State_Jumping");
        }
    }
}