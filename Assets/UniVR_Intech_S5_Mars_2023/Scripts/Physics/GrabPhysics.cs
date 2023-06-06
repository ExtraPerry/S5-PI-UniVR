using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabPhysics : MonoBehaviour
{
    // Input Maps.
    public InputActionProperty grabInputSource;
    // Settings.
    public float radius = 0.1f;
    public LayerMask grabLayer;
    // Active Joint (the link between the hand and the grabbed object).
    private FixedJoint fixedJoint;
    private XRGrabInteractable xRGrabInteraction;
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

                // Check if grab is using XR toolkit Grab
                xRGrabInteraction = nearbyRigidbody.gameObject.GetComponent<XRGrabInteractable>();
                if (xRGrabInteraction)
                {
                    isGrabbing = true;
                    return;
                }

                fixedJoint = gameObject.AddComponent<FixedJoint>();
                fixedJoint.autoConfigureConnectedAnchor = false;

                // Check if it has a rigibody or not to define the type of interaction.
                // (Can you hold it in your hand ? / Is it a climbing element to attach yourself to ?)
                if (nearbyRigidbody)
                {
                    // Lock the object to the hand.
                    fixedJoint.connectedBody = nearbyRigidbody;
                    fixedJoint.connectedAnchor = nearbyRigidbody.transform.InverseTransformPoint(transform.position);
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
            // Check if the hand is a grab interaction, the grab interaction references to the object being grabbed.
            if (xRGrabInteraction)
            {
                // If so and it is not released then ignore.
                if (xRGrabInteraction.isSelected == true) return;
            }

            // CHeck if the hand has an active joint.
            if (fixedJoint)
            {
                // Delete the joint.
                Destroy(fixedJoint);
            }

            // Tell the system this hand is currently no longer grabbing something.
            isGrabbing = false;
        }
    }
}