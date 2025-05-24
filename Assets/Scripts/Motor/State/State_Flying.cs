using UnityEngine;
namespace Epitome
{
    public class State_Flying : BaseState
    {
        public float overrideSpeed = 10.0f;

        public override Vector3 ProcessMotion(Vector3 input)
        {
            input.y = Input.GetKey(KeyCode.Space) ? 1 : (Input.GetKey(KeyCode.LeftControl)) ? -1 : 0;

            MotorHelper.KillVector(ref input, motor.WallVector);
            MotorHelper.ApplySpeed(ref input, overrideSpeed);

            return input;
        }
    }
}