using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ContinuousMovementPhysics : MonoBehaviour
{
    [Header("Player :")]
    [SerializeField]
    private PlayerRig playerRig;

    [Header("Inputs :")]
    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    public InputActionProperty jumpInputSource;
    public InputActionProperty respawnInputSource;

    [Header("Settings :")]
    public float moveSpeed = 1;
    public float turnSpeed = 60;
    public float jumpHeight = 1.5f;
    [SerializeField]
    private LayerMask groundLayer;

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

        // Check if playerRig is on the ground and jump if jump button is pressed.
        if (inputJump && isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
            playerRig.rigidbodyRig.body.velocity = Vector3.up * jumpVelocity;
        }

        if (inputRespawn)
        {
            PrepareTeleportTo(playerRig.respawnLocation.Get());
        }

    }

    private void FixedUpdate()
    {
        if (isPrimedToTeleport)
        {
            TeleportPlayer();
            return;
        }

        isGrounded = CheckIfGrounded();

        if (isGrounded)
        {
            MoveAndRotatePlayer();
        }
    }

    public void MoveAndRotatePlayer()
    {
        // Define the direction of the movement.
        Quaternion yaw = Quaternion.Euler(0, playerRig.xrRig.headset.transform.eulerAngles.y, 0);
        Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

        // Calculate the move 3D vector by which the playerRig will move.
        Vector3 targetMovePosition = playerRig.rigidbodyRig.body.position + ((direction * Time.fixedDeltaTime) * moveSpeed);

        // Define the direction of the rotation.
        Vector3 axis = Vector3.up;
        float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

        // Calculate the Quaternion by which the playerRig will rotate.
        Quaternion q = Quaternion.AngleAxis(angle, axis);

        // Update the playerRig's rotation.
        playerRig.rigidbodyRig.body.MoveRotation(playerRig.rigidbodyRig.body.rotation * q);

        // Calculate the position change of the playerRig that the rotation has induced (with the movement calculated before too).
        Vector3 newPosition = q * (targetMovePosition - playerRig.xrRig.headset.transform.position) + playerRig.xrRig.headset.transform.position;

        // Update the playerRig's position.
        playerRig.rigidbodyRig.body.MovePosition(newPosition);
    }

    public void OnPrepareTeleportTo(Component sender, object data)
    {
        if (sender is not MainMenu || data is not Transform) return;
        Transform target = (Transform)data;
        PrepareTeleportTo(target);
    }

    private void PrepareTeleportTo(Transform target)
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

    private void TeleportPlayer()
    {
        // Reset all velocity.
        playerRig.rigidbodyRig.SetVelocity(Vector3.zero);

        // Get the vector by which the playerRig will move.
        Vector3 direction = teleportNextMovePosition - playerRig.rigidbodyRig.body.transform.position;

        // Calculate movement that other rigidbodies will move by said direction.
        Vector3 newHeadPosition = playerRig.rigidbodyRig.head.transform.position + direction;
        Vector3 newLeftHandPosition = playerRig.rigidbodyRig.leftHand.transform.position + direction;
        Vector3 newRightHandPosition = playerRig.rigidbodyRig.rightHand.transform.position + direction;

        // Update the playerRig's position. (Use .position because it is a teleport).
        playerRig.rigidbodyRig.head.position = newHeadPosition;
        playerRig.rigidbodyRig.body.position = teleportNextMovePosition;
        playerRig.rigidbodyRig.leftHand.position = newLeftHandPosition;
        playerRig.rigidbodyRig.rightHand.position = newRightHandPosition;

        // Reset the teleport primer.
        isPrimedToTeleport = false;
    }

    public bool CheckIfGrounded()
    {
        // Gets the center body location of the body collider in 3d space & define the size of the raycast based on the collider's size.
        Vector3 start = playerRig.colliderRig.body.transform.TransformPoint(playerRig.colliderRig.body.center);
        float rayLength = (playerRig.colliderRig.body.height / 2) - playerRig.colliderRig.body.radius + 0.05f;

        // Raycast downwards using the previous settings above.
        bool hasHit = Physics.SphereCast(start, playerRig.colliderRig.body.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        // If the raycast hit something return true else false.
        return hasHit;
    }
}

