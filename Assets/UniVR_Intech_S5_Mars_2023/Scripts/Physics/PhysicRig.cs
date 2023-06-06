using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicRig : MonoBehaviour
{
    [Header("Player :")]
    [SerializeField]
    private PlayerRig playerRig;

    [Header("Settings :")]
    //Settings for the main body physics.
    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2;

    [Header("Joints :")]
    // Physics elements representing the playerRig's head and hands.
    [SerializeField]
    private ConfigurableJoint headJoint;
    [SerializeField]
    private ConfigurableJoint leftHandJoint;
    [SerializeField]
    private ConfigurableJoint rightHandJoint;

    // Update is called once per fixed interval.
    void FixedUpdate()
    {   
        // Update the body physics to adjust it's height based on the playerRig's VR headset position.
        playerRig.colliderRig.body.height = Mathf.Clamp(playerRig.xrRig.headset.localPosition.y, bodyHeightMin, bodyHeightMax);
        playerRig.colliderRig.body.center = new Vector3(playerRig.xrRig.headset.localPosition.x, playerRig.colliderRig.body.height / 2, playerRig.xrRig.headset.transform.localPosition.z);

        // Update the left hand physics to the left controller's position & rotation.
        leftHandJoint.targetPosition = playerRig.xrRig.leftController.localPosition;
        leftHandJoint.targetRotation = playerRig.xrRig.leftController.localRotation;

        // Update the right hand physics to the right controller's position & rotation.
        rightHandJoint.targetPosition = playerRig.xrRig.rightController.localPosition;
        rightHandJoint.targetRotation = playerRig.xrRig.rightController.localRotation;

        // Update the head physics to the playerRig's VR headset position & rotation.
        headJoint.targetPosition = playerRig.xrRig.headset.localPosition;
        headJoint.targetRotation = playerRig.xrRig.headset.localRotation;
    }
}
