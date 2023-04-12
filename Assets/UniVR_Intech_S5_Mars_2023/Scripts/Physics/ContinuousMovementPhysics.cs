using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class RigidbodyRig
{
    public Rigidbody head;
    public Rigidbody body;
    public Rigidbody leftHand;
    public Rigidbody rightHand;

    public void SetVelocity(Vector3 amount)
    {
        head.velocity = amount;
        body.velocity = amount;
        leftHand.velocity = amount;
        rightHand.velocity = amount;
    }
}

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
    public InputActionProperty respawnInputSource;
    // Reference Elements.
    public RigidbodyRig rig;
    public LayerMask groundLayer;
    public Transform directionSource;
    public Transform turnSource;
    public CapsuleCollider bodyCollider;
    public Transform worldSpawn;
    // Registered Inputs.
    private Vector2 inputMoveAxis;
    private float inputTurnAxis;
    // Global Variables for use in next time frame.
    private Vector3 teleportNextMovePosition;
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
        bool inputRespawn = respawnInputSource.action.WasPerformedThisFrame();

        // Check if player is on the ground and jump if jump button is pressed.
        if (inputJump && isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
            rig.body.velocity = Vector3.up * jumpVelocity;
        }

        if (inputRespawn)
        {
            PrepareTeleportTo(worldSpawn);
        }

    }

    private void FixedUpdate()
    {
        // Check if the player is grounded, but instance it for other potential uses later.
        isGrounded = CheckIfGrounded();
        //
        if (isGrounded && !isPrimedToTeleport)
        {
            MoveAndRotatePlayer();
        }
        //
        else if (isPrimedToTeleport)
        {
            TeleportPlayer();
        }
    }

    public void MoveAndRotatePlayer()
    {
        // Define the direction of the movement.
        Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
        Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

        // Calculate the move 3D vector by which the player will move.
        Vector3 targetMovePosition = rig.body.position + ((direction * Time.fixedDeltaTime) * moveSpeed);

        // Define the direction of the rotation.
        Vector3 axis = Vector3.up;
        float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

        // Calculate the Quaternion by which the player will rotate.
        Quaternion q = Quaternion.AngleAxis(angle, axis);

        // Update the player's rotation.
        rig.body.MoveRotation(rig.body.rotation * q);

        // Calculate the position change of the player that the rotation has induced (with the movement calculated before too).
        Vector3 newPosition = q * (targetMovePosition - turnSource.position) + turnSource.position;

        // Update the player's position.
        rig.body.MovePosition(newPosition);
    }

    public void TeleportPlayer()
    {
        // Reset all velocity.
        rig.SetVelocity(Vector3.zero);

        // Get the vector by which the player will move.
        Vector3 direction = teleportNextMovePosition - rig.body.transform.position;

        // Calculate movement that other rigidbodies will move by said direction.
        Vector3 newHeadPosition = rig.head.transform.position + direction;
        Vector3 newLeftHandPosition = rig.leftHand.transform.position + direction;
        Vector3 newRightHandPosition = rig.rightHand.transform.position + direction;

        // Update the player's position. (Use .position because it is a teleport).
        rig.head.position = newHeadPosition;
        rig.body.position = teleportNextMovePosition;
        rig.leftHand.position = newLeftHandPosition;
        rig.rightHand.position = newRightHandPosition;

        // Reset the teleport primer.
        isPrimedToTeleport = false;
    }

    public bool CheckIfGrounded()
    {
        // Gets the center body location of the body collider in 3d space & define the size of the raycast based on the collider's size.
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = (bodyCollider.height / 2) - bodyCollider.radius + 0.05f;

        // Raycast downwards using the previous settings above.
        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        // If the raycast hit something return true else false.
        return hasHit;
    }

    public void PrepareTeleportTo(Transform target)
    {
        if (!isPrimedToTeleport)
        {
            teleportNextMovePosition = target.position;
            isPrimedToTeleport = true;
        }
        else
        {
            Debug.Log("A teleportation sequence is already in progress !");
        }
    }
}

