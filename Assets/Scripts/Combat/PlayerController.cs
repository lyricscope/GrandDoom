using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 12f;
    
    [Header("Camera")]
    public CinemachineFreeLook freeLookCamera;
    
    private CharacterController controller;
    private Vector2 inputDirection;
    private float currentSpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void HandleInput()
    {
        inputDirection = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;
    }

    private void MoveCharacter()
    {
        if (inputDirection.magnitude > 0.1f)
        {
            // Получаем направление относительно камеры
            Vector3 moveDirection = GetCameraRelativeDirection();
            
            // Поворот персонажа
            RotateTowards(moveDirection);
            
            // Движение
            controller.Move(moveDirection * (currentSpeed * Time.fixedDeltaTime));
        }
        
        UpdateSpeed();
    }

    private Vector3 GetCameraRelativeDirection()
    {
        Vector3 cameraForward = freeLookCamera.transform.forward;
        Vector3 cameraRight = freeLookCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        
        return (cameraForward * inputDirection.y + cameraRight * inputDirection.x).normalized;
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    private void UpdateSpeed()
    {
        var targetSpeed = inputDirection.magnitude > 0.1f ? moveSpeed : 0f;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 10f * Time.fixedDeltaTime);
    }
}