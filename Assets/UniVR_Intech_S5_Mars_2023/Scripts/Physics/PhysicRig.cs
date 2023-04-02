using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicRig : MonoBehaviour
{
    // Real world Headset and Controllers.
    public Transform playerHead;
    public Transform leftController;
    public Transform rightController;

    // Physics elements representing the player's head and hands.
    public ConfigurableJoint headJoint;
    public ConfigurableJoint leftHandJoint;
    public ConfigurableJoint rightHandJoint;

    // The main body physics for the player.
    public CapsuleCollider bodyCollider;

    //Settings for the main body physics.
    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2;

    // Update is called once per fixed interval.
    void Update()
    {   
        // Update the body physics to adjust it's height based on the player's VR headset position.
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2, playerHead.localPosition.z);

        // Update the left hand physics to the left controller's position & rotation.
        leftHandJoint.targetPosition = leftController.localPosition;
        leftHandJoint.targetRotation = leftController.localRotation;

        // Update the right hand physics to the right controller's position & rotation.
        rightHandJoint.targetPosition = rightController.localPosition;
        rightHandJoint.targetRotation = rightController.localRotation;

        // Update the head physics to the player's VR headset position & rotation.
        headJoint.targetPosition = playerHead.localPosition;
        headJoint.targetRotation = playerHead.localRotation;
    }
}
