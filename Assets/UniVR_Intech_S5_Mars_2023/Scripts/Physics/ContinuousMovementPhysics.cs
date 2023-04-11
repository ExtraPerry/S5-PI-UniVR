using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinuousMovementPhysics : MonoBehaviour
{
    // Main Parameters.
    public float moveSpeed = 1;
    public float turnSpeed = 60;
    public float jumpHeight = 1.5f;
    // Input Maps.
    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    public InputActionProperty jumpInputSource;
    // Reference Elements.
    public Rigidbody rb;
    public LayerMask groundLayer;
    public Transform directionSource;
    public Transform turnSource;
    public CapsuleCollider bodyCollider;
    // Registered Inputs.
    private Vector2 inputMoveAxis;
    private float inputTurnAxis;
    // Global Variables for use in next time frame.
    private Vector3 teleportMovePosition;
    // Global Checks.
    private bool isGrounded;
    private bool isPrimedToTeleport;

    // Start is called before the first frame update
    void Start()
    {
        isPrimedToTeleport = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Read inputs from the joysticks.
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;
        // Read input from jump button.
        bool inputJump = jumpInputSource.action.WasPerformedThisFrame();

        // Check if player is on the ground and jump if jump button is pressed.
        if (inputJump && isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
            rb.velocity = Vector3.up * jumpVelocity;
        }

    }

    private void FixedUpdate()
    {
        // Check if the player is grounded, but instance it for other potential uses later.
        isGrounded = checkIfGrounded();
        // Also check if the rigidbody is sleeping.
        if (isGrounded && !rb.IsSleeping())
        {
            // Define the direction the player is looking at.
            Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);

            // Calculate the angle by which the player will rotate.
            float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

            // Define what is Up.
            Vector3 axis = Vector3.up;

            // Calculate the Quaternion by which the player will rotate arround the Up axis, so 2 degrees of rotation instead of 3 (ignore up angle).
            Quaternion q = Quaternion.AngleAxis(angle, axis);

            // Update the player's rotation.
            rb.MoveRotation(rb.rotation * q);

            // If the player is not primed for a teleport then let them move normally.
            if (!isPrimedToTeleport)
            {
                // Define the direction of the movement.
                Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

                // Calculate the position to which the player will move.
                Vector3 targetMovePosition = rb.position + direction * (Time.fixedDeltaTime * moveSpeed);

                // Take into account the movement produced by the players rotation.
                Vector3 newMovePosition = q * (targetMovePosition - turnSource.position) + turnSource.position;

                // Update the player's position. (Use MovePosition() because it a movement).
                rb.MovePosition(newMovePosition);
            }
            else
            {
                // Take into account the movement produced by the players rotation.
                Vector3 newTeleportPosition = q * (teleportMovePosition - turnSource.position) + turnSource.position;
                
                // Update the player's position. (Use .position because it a teleport).
                rb.position = newTeleportPosition;

                // Reset the teleport primer.
                isPrimedToTeleport = false;
                teleportMovePosition = Vector3.zero;
            }
        }
    }
       

    public bool checkIfGrounded()
    {
        // Gets the center body location of the body collider in 3d space & define the size of the raycast based on the collider's size.
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = (bodyCollider.height / 2) - bodyCollider.radius + 0.05f;

        // Raycast downwards using the previous settings above.
        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        // If the raycast hit something return true else false.
        return hasHit;
    }

    public void teleportTo(Transform target)
    {
        rb.Sleep();
        teleportMovePosition = target.position;
        isPrimedToTeleport = true;
        rb.WakeUp();
    }
}

