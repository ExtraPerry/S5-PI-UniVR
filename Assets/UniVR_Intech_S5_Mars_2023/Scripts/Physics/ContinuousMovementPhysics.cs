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
    // Global Checks.
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
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
        // Check if the player is grounded.
        isGrounded = checkIfGrounded();

        // If the player is grounded then let them be able to move arround.
        if (isGrounded)
        {
            // Define the direction of the movement.
            Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
            Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

            // Calculate the move 3D vector by which the player will move.
            Vector3 targetMovePosition = rb.position + ((direction * Time.fixedDeltaTime) * moveSpeed);

            // Define the direction of the rotation.
            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

            // Calculate the Quaternion by which the player will rotate.
            Quaternion q = Quaternion.AngleAxis(angle, axis);

            // Update the player's rotation.
            rb.MoveRotation(rb.rotation * q);

            // Calculate the position change of the player that the rotation has induced (with the movement calculated before too).
            Vector3 newPosition = q * (targetMovePosition - turnSource.position) + turnSource.position;

            // Update the player's position.
            rb.MovePosition(newPosition);
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

    public void setJumpHeight(float height)
    {
        jumpHeight = height;
    }
}

