using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicRig : MonoBehaviour
{
    [Header("Player :")]
    [SerializeField]
    private Player player;

    [Header("Settings :")]
    //Settings for the main body physics.
    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2;

    [Header("Joints :")]
    // Physics elements representing the player's head and hands.
    [SerializeField]
    private ConfigurableJoint headJoint;
    [SerializeField]
    private ConfigurableJoint leftHandJoint;
    [SerializeField]
    private ConfigurableJoint rightHandJoint;

    // Update is called once per fixed interval.
    void FixedUpdate()
    {   
        // Update the body physics to adjust it's height based on the player's VR headset position.
        player.colliderRig.body.height = Mathf.Clamp(player.xrRig.headset.localPosition.y, bodyHeightMin, bodyHeightMax);
        player.colliderRig.body.center = new Vector3(player.xrRig.headset.localPosition.x, player.colliderRig.body.height / 2, player.xrRig.headset.transform.localPosition.z);

        // Update the left hand physics to the left controller's position & rotation.
        leftHandJoint.targetPosition = player.xrRig.leftController.localPosition;
        leftHandJoint.targetRotation = player.xrRig.leftController.localRotation;

        // Update the right hand physics to the right controller's position & rotation.
        rightHandJoint.targetPosition = player.xrRig.rightController.localPosition;
        rightHandJoint.targetRotation = player.xrRig.rightController.localRotation;

        // Update the head physics to the player's VR headset position & rotation.
        headJoint.targetPosition = player.xrRig.headset.localPosition;
        headJoint.targetRotation = player.xrRig.headset.localRotation;
    }
}
