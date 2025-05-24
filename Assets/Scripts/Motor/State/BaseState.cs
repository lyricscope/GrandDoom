using UnityEngine;

namespace Epitome
{
    public class BaseState : MonoBehaviour
    {
        // Refs
        protected BaseMotor motor;
        protected float startTime;

        [SerializeField] protected float immuneTime;

        private void Awake()
        {
            motor = GetComponent<BaseMotor>();
        }

        public virtual void Construct()
        {
            startTime = Time.time;
        }
        public virtual void Destruct()
        {

        }
        public virtual void PlayerTransition()
        {
            // Don't let state change if state is immune
            if (Time.time - startTime < immuneTime)
                return;
        }
        public virtual void Transition()
        {
            // Don't let state change if state is immune
            if (Time.time - startTime < immuneTime)
                return;
        }
        public virtual void ProcessAnimation(Animator animator)
        {
            Vector3 dir = motor.MoveVector;
            MotorHelper.KillVertical(ref dir);
        }
        public virtual Vector3 ProcessMotion(Vector3 input)
        {
            Debug.Log("Process Motion not implemented in " + this.ToString());
            return input;
        }
        public virtual Quaternion ProcessRotation(Vector3 input)
        {
            return MotorHelper.FaceDirection(motor.MoveVector);
        }
        public virtual void AnimationEnded()
        {

        }
    }
}
