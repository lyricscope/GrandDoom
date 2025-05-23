# basic-3rd-person-movement-script-for-unity
The code is a script for moving a third-person character in Unity using the CharacterController component. The script consists of several parts:

Input handling: The horizontal and vertical inputs are obtained using the Input.GetAxisRaw method, which returns the value of a virtual axis (-1, 0, or 1). 
These inputs are then combined into a single direction vector, which represents the desired movement direction of the character.

Rotation handling: If the direction vector has a magnitude greater than or equal to 0.1, the script rotates the character towards the direction it should be facing.
The rotation is smoothed using the Mathf.SmoothDampAngle method to make the movement more fluid.

Movement handling: If the direction vector has a magnitude greater than or equal to 0.1, the script moves the character in the desired direction by calling the controller.
Move method. The controller.Move method takes a Vector3 as an argument and moves the CharacterController component in that direction.

Jump handling: If the character is grounded (i.e. touching the ground) and the jump button is pressed, the script calculates the initial upward velocity 
required to reach the desired jump height. This calculation is based on the equation of motion for an object under constant acceleration 
(in this case, the acceleration due to gravity). The upward velocity is then added to the character's current velocity, and the controller.
Move method is called to move the character.

Gravity handling: The script continuously updates the velocity.y component to simulate the effects of gravity. The value of the velocity.y component is decreased
by the product of the gravity constant and the time since the last frame, resulting in the character gradually accelerating downwards.

Overall, the script combines several different physics-based calculations to produce a believable and smooth movement system for a third-person character.
