using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Epitome
{
    [RequireComponent(typeof(CharacterController))]
    public class BaseMotor : MonoBehaviour
    {
        // CONST - Will affect ALL the actors using this system
	    private const float DISTANCE_GROUNDED = 0.5f;
	    private const float INNER_OFFSET_GROUNDED = 0.05f;

	    // Tweakable fields
	    public float baseSpeed = 7f;
	    public float baseGravity = 15f;
	    public float baseJumpForce = 5f;
	    public float terminalVelocity = 152.0f;
	    public float slopeTreshold = 0.55f;

        // References, set and used by the motor
	    protected CharacterController controller;
	    protected BaseState state;
        protected BaseState[] availableStates;
	    protected float speedModifier;
	    protected float gravityModifier;
	    protected float jumpForceModifier;

	    // Moving Platform Support
	    private Transform activePlatform;
	    private Vector3 activeLocalPoint;
	    private Vector3 activeGlobalPoint;
	    private Vector3 lastPlatformVelocity;

	    // Properties
        public float HorizontalVelocity { set; get; }
	    public Vector3 MoveVector{set;get;}
        public Vector3 LastDirection { set; get; }
	    public Quaternion RotationQuaternion{set;get;}
	    public Vector3 InputVector{ set; get;}
        public Vector3 DirectionVector { set; get; }
	    [HideInInspector] // Not a property so we can use in ref / out
	    public float VerticalVelocity;
        [HideInInspector] // Not a property so we can use in ref / out
        public Vector3 AirInfluence;
        public Vector3 WallVector{ set; get;}
	    public Vector3 SlopeNormal{set;get;}
	    public Animator Anim{ set; get;}
	    public CollisionFlags ColFlags{ set; get;}
        public bool AirExhausted { set; get; }
        public bool IsGrounded { set; get; }

	    #region Start
	    public virtual void Start()
	    {
            Anim = GetComponentInChildren<Animator> ();
		    controller = GetComponent<CharacterController> ();

            availableStates = GetComponents<BaseState>();

            // Initial state set on Walking
            state = GetComponent<State_Walking> ();
		    state.Construct ();
	    }
	    #endregion

	    #region Update
	    private void Update()
	    {
		    UpdateMotor ();
	    }
	    protected virtual void UpdateMotor()
	    {
            // Are we grounded?
            IsGrounded = Grounded();

            // Ask Mobility state to Calculate Motion
            MoveVector = state.ProcessMotion(MoveVector);
		    RotationQuaternion = state.ProcessRotation(MoveVector);

		    // Check for StateTransitions
		    state.Transition();

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

            // Tell the animator state machine how we're doing
            Anim?.SetFloat("Speed", HorizontalVelocity);
            Anim?.SetBool("IsGrounded", IsGrounded);
        }
	    protected virtual void Move()
	    {
		    ColFlags = controller.Move (MoveVector * Time.deltaTime);
		    WallVector = (((ColFlags & CollisionFlags.Sides) != 0) ? WallVector : Vector3.zero);
	    }
	    protected virtual void Rotate()
	    {
		    transform.rotation = RotationQuaternion;
	    }
	    #endregion

	    #region Getters
	    public float Speed{ get { return baseSpeed + speedModifier;} }
	    public float Gravity{ get { return baseGravity + gravityModifier;} }
	    public float JumpForce{ get { return baseJumpForce + jumpForceModifier;} }
	    public float SlopeTreshold{get{ return slopeTreshold;}}
	    public float TerminalVelocity{get{ return terminalVelocity;}}
	    #endregion

	    #region Methods
	    protected virtual bool Grounded()
	    {
		    float yRay = controller.bounds.center.y - (controller.height * 0.5f) + 0.3f;
		    RaycastHit hit;

		    // Mid
		    if(Physics.Raycast(new Vector3(controller.bounds.center.x,yRay,controller.bounds.center.z),-Vector3.up,out hit,DISTANCE_GROUNDED))
		    {
			    SlopeNormal = hit.normal;
			    return (SlopeNormal.y > slopeTreshold)?true:false;
		    }

		    // Front-Right
		    if(Physics.Raycast(new Vector3(controller.bounds.center.x + (controller.bounds.extents.x - INNER_OFFSET_GROUNDED),yRay,controller.bounds.center.z + (controller.bounds.extents.z - INNER_OFFSET_GROUNDED)),-Vector3.up,out hit,DISTANCE_GROUNDED))
		    {
			    SlopeNormal = hit.normal;
			    return (SlopeNormal.y > slopeTreshold) ?true:false;
		    }

		    // Front-Left
		    if(Physics.Raycast(new Vector3(controller.bounds.center.x - (controller.bounds.extents.x - INNER_OFFSET_GROUNDED),yRay,controller.bounds.center.z + (controller.bounds.extents.z - INNER_OFFSET_GROUNDED)),-Vector3.up,out hit,DISTANCE_GROUNDED))
		    {
			    SlopeNormal = hit.normal;
			    return (SlopeNormal.y > slopeTreshold) ?true:false;
		    }
		    // Back Right
		    if(Physics.Raycast(new Vector3(controller.bounds.center.x + (controller.bounds.extents.x - INNER_OFFSET_GROUNDED),yRay,controller.bounds.center.z - (controller.bounds.extents.z - INNER_OFFSET_GROUNDED)),-Vector3.up,out hit,DISTANCE_GROUNDED))
		    {
			    SlopeNormal = hit.normal;
			    return (SlopeNormal.y > slopeTreshold) ?true:false;
		    }
		    // Back Left
		    if(Physics.Raycast(new Vector3(controller.bounds.center.x - (controller.bounds.extents.x - INNER_OFFSET_GROUNDED),yRay,controller.bounds.center.z - (controller.bounds.extents.z - INNER_OFFSET_GROUNDED)),-Vector3.up,out hit,DISTANCE_GROUNDED))
		    {
			    SlopeNormal = hit.normal;
			    return (SlopeNormal.y > slopeTreshold) ?true:false;
		    }

		    return false;
	    }
	    protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
	    {
		    if (hit.moveDirection.y < 0.9f && hit.normal.y > 0.5f)
			    activePlatform = hit.collider.transform;

            if (VerticalVelocity > 0 && ((ColFlags & CollisionFlags.Above) != 0))
                VerticalVelocity = 0;

            if ((ColFlags & CollisionFlags.Sides) != 0)
                WallVector = hit.normal;
        }
        public void OnAnimationEnd()
        {
            state.AnimationEnded();
        }
	    public virtual void ChangeState(string stateName)
	    {
            stateName = "Epitome." + stateName;
            foreach (BaseState s in availableStates)
            {
                if (s.GetType().FullName != stateName)
                    continue;

                state.Destruct();
                state = s;
                state.Construct();

                return;
            }

            Debug.Log("Unable to find state " + stateName);
	    }
        public virtual void ChangeState(BaseState s)
        {
            state.Destruct();
            state = s;
            state.Construct();
        }
	    protected void MovingPlatformPreMove()
	    {
		    if (activePlatform != null) 
		    {
			    var newGlobalPlatformPoint = activePlatform.TransformPoint (activeLocalPoint);
			    var moveDistance = (newGlobalPlatformPoint - activeGlobalPoint);
			    if (moveDistance != Vector3.zero)
				    controller.Move (moveDistance);

			    lastPlatformVelocity = moveDistance / Time.deltaTime;
		    } 
		    else
		    {
			    lastPlatformVelocity = Vector3.zero;
		    }
	    }
	    protected void MovingPlatformPostMove()
	    {
		    if (activePlatform != null)
		    {
			    activeGlobalPoint = transform.position;
			    activeLocalPoint = activePlatform.InverseTransformPoint (activeGlobalPoint);
		    }
	    }
	    #endregion
    }
}