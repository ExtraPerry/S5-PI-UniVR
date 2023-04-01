using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ContinuousMovementPhysics : MonoBehaviour
{
    // Main Parameters.
    public float moveSpeed = 1;
    public float turnSpeed = 60;
    // Inputs.
    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    // Reference Elements.
    public Rigidbody rb;
    public LayerMask groundLayer;
    public Transform directionSource;
    public Transform turnSource;
    public CapsuleCollider bodyCollider;
    // Private attributes.
    private Vector2 inputMoveAxis;
    private float inputTurnAxis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;

    }

    private void FixedUpdate()
    {
        bool isGrounded = checkIfGrounded();

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
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = (bodyCollider.height / 2) - bodyCollider.radius + 0.05f;

        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return hasHit;
    }
}

