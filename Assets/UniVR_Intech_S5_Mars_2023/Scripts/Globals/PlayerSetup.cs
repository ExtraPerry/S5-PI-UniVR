using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [Header("Player :")]
    [SerializeField]
    private PlayerRig playerRig;

    [Header("XR Rig :")]
    [SerializeField]
    private Transform headset;
    [SerializeField]
    private Transform leftController;
    [SerializeField]
    private Transform rightController;

    [Header("Collider Rig :")]
    [SerializeField]
    private Collider headCollider;
    [SerializeField]
    private CapsuleCollider bodyCollider;
    [SerializeField]
    private Collider leftHandCollider;
    [SerializeField]
    private Collider rightHandCollider;

    [Header("Rigidbody Rig :")]
    [SerializeField]
    private Rigidbody headRigidbody;
    [SerializeField]
    private Rigidbody bodyRigidbody;
    [SerializeField]
    private Rigidbody leftHandRigidbody;
    [SerializeField]
    private Rigidbody rightHandRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        playerRig.xrRig.headset = headset;
        playerRig.xrRig.leftController = leftController;
        playerRig.xrRig.rightController = rightController;

        playerRig.colliderRig.head = headCollider;
        playerRig.colliderRig.body = bodyCollider;
        playerRig.colliderRig.leftHand = leftHandCollider;
        playerRig.colliderRig.rightHand = rightHandCollider;

        playerRig.rigidbodyRig.head = headRigidbody;
        playerRig.rigidbodyRig.body = bodyRigidbody;
        playerRig.rigidbodyRig.leftHand = leftHandRigidbody;
        playerRig.rigidbodyRig.rightHand = rightHandRigidbody;

        // Since this no longer needs to be run disable the behaviour.
        this.enabled = false;
    }
}
