using UnityEngine;
using UnityEngine.InputSystem;

namespace Epitome
{
    public class PlayerMotor : BaseMotor
    {
        [Header("Player Config")]
        [SerializeField] private bool lockCursor = true;

	    public Transform CameraTransform{ set; get;}

	    public override void Start ()
	    {
            base.Start();

		    // Initialize the player's Camera
		    CameraTransform = Camera.main.transform;

            if(lockCursor)
                Cursor.lockState = CursorLockMode.Locked;
        }

        protected override void UpdateMotor ()
	    {
            // Are we grounded?
            IsGrounded = Grounded();

            // Get the Joystick Input, store them in MoveVector
            Vector3 input = PoolInput ();
		    InputVector = input;

		    // Rotate the MoveVector with the camera's direction
		    MotorHelper.RotateWithView (ref input,CameraTransform);
            DirectionVector = input;

            // Transfer inputs to the MoveVector before processing it
            MoveVector = input;

		    // Ask Mobility state to Calculate Motion
		    MoveVector = state.ProcessMotion(MoveVector);
	    	RotationQuaternion = state.ProcessRotation(MoveVector);

		    // Check for StateTransitions
		    state.PlayerTransition();

		    // Moving Platform Pre-move
		    MovingPlatformPreMove();

		    // Move the Controller
		    Move();
		    Rotate();

		    // Moving Platform Post-move
		    MovingPlatformPostMove();

            // Storing Velocity data for next frame
            LastDirection = new Vector3(MoveVector.x, 0, MoveVector.z);
            HorizontalVelocity = LastDirection.magnitude;

            // Tell the animator state machine how fast we're going
            Anim?.SetFloat("Speed", HorizontalVelocity);
            Anim?.SetBool("IsGrounded", Grounded());
        }

	    private Vector3 PoolInput()
	    {
            Vector3 dir = InputManager.Instance.MoveInputDirection;
            
            if (dir.sqrMagnitude > 1)
			    dir.Normalize ();

            return dir;
	    }
    }
}
