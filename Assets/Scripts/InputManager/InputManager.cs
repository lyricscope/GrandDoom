using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager>
{
    #region Public properties, to be accessed by external scripts
    // Vector values
    private Vector3 moveInputDirection;
    public Vector3 MoveInputDirection { get { return moveInputDirection; } }

    // Buttons
    private bool jump;
    public bool Jump { get { return jump; } }
    #endregion

    #region UnityEngine.Input
    public InputSettings settings; // To know which update loop we're using
    #endregion

    #region Mono implementation
    private void Update()
    {
        if (settings.updateMode == InputSettings.UpdateMode.ProcessEventsInDynamicUpdate)
            ResetInputEveryFrame();
    }
    private void FixedUpdate()
    {
        if (settings.updateMode == InputSettings.UpdateMode.ProcessEventsInFixedUpdate)
            ResetInputEveryFrame();
    }
    private void ResetInputEveryFrame()
    {
        // Buttons
        jump = false;
    }
    #endregion

    #region Actions
    public void OnMovement(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();
        moveInputDirection = new Vector3(value.x, 0, value.y);
    }

    public void OnJump()
    {
        jump = true;
    }
    #endregion
}