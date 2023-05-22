using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabPhysics : MonoBehaviour
{
    // Input Maps.
    public InputActionProperty grabInputSource;
    // Settings.
    public float radius = 0.1f;
    public LayerMask grabLayer;
    // Active Joint (the link between the hand and the grabbed object).
    private FixedJoint fixedJoint;
    // Value that defines if something is being grabbed.
    private bool isGrabbing = false;


    void FixedUpdate()
    {
        // Read the input.
        bool isGrabButtonPressed = grabInputSource.action.ReadValue<float>() > 0.1f;

        // Check if the hand is trying to grab and if it hasn't already grabbed something.
        if (isGrabButtonPressed && !isGrabbing)
        {
            // Look for nearby colliders, filtering with the selected layer.
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, radius, grabLayer, QueryTriggerInteraction.Ignore);

            // Check if something was detected.
            if (nearbyColliders.Length > 0)
            {
                Rigidbody nearbyRigidbody = nearbyColliders[0].attachedRigidbody;

                fixedJoint = gameObject.AddComponent<FixedJoint>();
                fixedJoint.autoConfigureConnectedAnchor = false;

                // Check if it has a rigibody or not to define the type of interaction.
                // (Can you hold it in your hand ? / Is it a climbing element to attach yourself to ?)
                if (nearbyRigidbody)
                {
                    // Lock the object to the hand.
                    fixedJoint.connectedBody = nearbyRigidbody;
                    fixedJoint.connectedAnchor = nearbyRigidbody.transform.InverseTransformPoint(transform.position);

                    // If specified then grab to point.
                    GrabbablePoint grabPoint = nearbyRigidbody.gameObject.GetComponent<GrabbablePoint>();
                    if (grabPoint)
                    {
                        Side handSide = Side.Unspecified;
                        if (gameObject.CompareTag("Left Hand"))
                        {
                            handSide = Side.Left;
                        }
                        if (gameObject.CompareTag("Right Hand"))
                        {
                            handSide = Side.Right;
                        }
                        grabPoint.MatchGrabPoint(handSide, transform);
                    }
                }
                else
                {
                    // Anchor the hand to the world.
                    fixedJoint.connectedAnchor = transform.position;
                }

                // Tell the system this hand is currently grabbing something.
                isGrabbing = true;
            }
        }
        // CHeck if the hand is not grabbing anymore and if something is still attached to the hand.
        else if (!isGrabButtonPressed && isGrabbing)
        {
            // Tell the system this hand is currently no longer grabbing something.
            isGrabbing = false;

            // Check if the hand has an active joint.
            if (fixedJoint)
            {
                // Delete the joint.
                Destroy(fixedJoint);
            }
        }
    }


    /**
    // Input Maps.
    public InputActionProperty grabInputSource;
    // Settings.
    public float maxAngularVelocity = 20;
    public float radius = 0.1f;
    public LayerMask grabLayer;
    // Grabbed Rigibody.
    private Rigidbody holdingTarget;

    void FixedUpdate()
    {
        // Read the input.
        bool isGrabButtonPressed = grabInputSource.action.ReadValue<float>() > 0.1f;

        // Action
        if (isGrabButtonPressed)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, grabLayer, QueryTriggerInteraction.Ignore);
            if (colliders.Length < 0)
            {
                holdingTarget = colliders[0].transform.root.GetComponent<Rigidbody>();
            }
            else
            {
                holdingTarget = null;
            }
        }
        else
        {
            if (holdingTarget)
            {
                // Adjust rigidbody velocity to move the hand.
                holdingTarget.velocity = (transform.position - holdingTarget.transform.position) / Time.fixedDeltaTime;

                // Adjust angular velocity to rotate to hand.
                holdingTarget.maxAngularVelocity = maxAngularVelocity;
                Quaternion deltaRot = transform.rotation * Quaternion.Inverse(holdingTarget.transform.rotation);
                Vector3 eulerRot = new Vector3(
                    Mathf.DeltaAngle(0, deltaRot.eulerAngles.x),
                    Mathf.DeltaAngle(0, deltaRot.eulerAngles.y),
                    Mathf.DeltaAngle(0, deltaRot.eulerAngles.z)
                );
                eulerRot *= 0.95f;
                eulerRot *= Mathf.Deg2Rad;
                holdingTarget.angularVelocity = eulerRot / Time.fixedDeltaTime;
            }
        }
    }
    */
}
