using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class controls player movement
/// </summary>
public class Controller : MonoBehaviour
{
    [Header("GameObject/Component References")]
    [Tooltip("The animator controller used to animate the player.")]
    public RuntimeAnimatorController animator = null;
    [Tooltip("The Rigidbody2D component to use in \"Astroids Mode\".")]
    public Rigidbody2D myRigidbody = null;

    [Header("Movement Variables")]
    [Tooltip("The speed at which the player will move.")]
    public float moveSpeed = 10.0f;
    [Tooltip("The speed at which the player rotates in asteroids movement mode")]
    public float rotationSpeed = 60f;

    // Speed boost variables
    private float currentSpeed;  // The current speed (affected by boost)
    private bool isSpeedBoostActive = false;  // Whether speed boost is active
    private float speedBoostEndTime = 0f;  // When the speed boost will end

    //The InputManager to read input from
    private InputManager inputManager;

    /// <summary>
    /// Enum which stores different aiming modes
    /// </summary>
    public enum AimModes { AimTowardsMouse, AimForwards };

    [Tooltip("The aim mode in use by this player:\n" +
        "Aim Towards Mouse: Player rotates to face the mouse\n" +
        "Aim Forwards: Player aims the direction they face (doesn't face towards the mouse)")]
    public AimModes aimMode = AimModes.AimTowardsMouse;

    /// <summary>
    /// Enum to handle different movement modes for the player
    /// </summary>
    public enum MovementModes { MoveHorizontally, MoveVertically, FreeRoam, Astroids };

    [Tooltip("The movmeent mode used by this controller:\n" +
        "Move Horizontally: Player can only move left/right\n" +
        "Move Vertically: Player can only move up/down\n" +
        "FreeRoam: Player can move in any direction and can aim\n" +
        "Astroids: Player moves forward/back in the direction they are facing and rotates with horizontal input")]
    public MovementModes movementMode = MovementModes.FreeRoam;

    private void Start()
    {
        SetupInput();
        currentSpeed = moveSpeed;  // Set the initial speed to moveSpeed
    }

    void Update()
    {
        HandleInput();
        SignalAnimator();

        // Check if speed boost should end
        if (isSpeedBoostActive && Time.time >= speedBoostEndTime)
        {
            DeactivateSpeedBoost();  // Deactivate speed boost when time ends
        }
    }

    private void SetupInput()
    {
        if (inputManager == null)
        {
            inputManager = InputManager.instance;
        }
        if (inputManager == null)
        {
            Debug.LogWarning("There is no player input manager in the scene, there needs to be one for the Controller to work");
        }
    }

    private void HandleInput()
    {
        Vector2 lookPosition = GetLookPosition();
        Vector3 movementVector = new Vector3(inputManager.horizontalMoveAxis, inputManager.verticalMoveAxis, 0);
        MovePlayer(movementVector);
        LookAtPoint(lookPosition);
    }

    private void SignalAnimator()
    {
        // Handle Animation (if necessary)
    }

    public Vector2 GetLookPosition()
    {
        Vector2 result = transform.up;
        if (aimMode != AimModes.AimForwards)
        {
            result = new Vector2(inputManager.horizontalLookAxis, inputManager.verticalLookAxis);
        }
        else
        {
            result = transform.up;
        }
        return result;
    }

    private void MovePlayer(Vector3 movement)
    {
        if (movementMode == MovementModes.Astroids)
        {
            if (myRigidbody == null)
            {
                myRigidbody = GetComponent<Rigidbody2D>();
            }

            Vector2 force = transform.up * movement.y * Time.deltaTime * currentSpeed;
            myRigidbody.AddForce(force);

            Vector3 newRotationEulars = transform.rotation.eulerAngles;
            float zAxisRotation = transform.rotation.eulerAngles.z;
            float newZAxisRotation = zAxisRotation - rotationSpeed * movement.x * Time.deltaTime;
            newRotationEulars = new Vector3(newRotationEulars.x, newRotationEulars.y, newZAxisRotation);
            transform.rotation = Quaternion.Euler(newRotationEulars);
        }
        else
        {
            transform.position += movement * Time.deltaTime * currentSpeed;
        }
    }

    private void LookAtPoint(Vector3 point)
    {
        if (Time.timeScale > 0)
        {
            Vector2 lookDirection = Camera.main.ScreenToWorldPoint(point) - transform.position;

            if (aimMode == AimModes.AimTowardsMouse)
            {
                transform.up = lookDirection;
            }
        }
    }

    // Function to activate speed boost
    public void ActivateSpeedBoost(float speedMultiplier, float duration)
    {
        if (!isSpeedBoostActive)
        {
            currentSpeed *= speedMultiplier;  // Increase speed
            speedBoostEndTime = Time.time + duration;  // Set end time
            isSpeedBoostActive = true;

            Debug.Log("Speed boost activated!");
        }
    }

    // Function to deactivate speed boost
    private void DeactivateSpeedBoost()
    {
        currentSpeed = moveSpeed;  // Reset to normal speed
        isSpeedBoostActive = false;

        Debug.Log("Speed boost deactivated.");
    }
}
