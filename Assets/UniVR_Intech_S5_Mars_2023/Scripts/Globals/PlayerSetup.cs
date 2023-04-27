using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [Header("Player :")]
    [SerializeField]
    private Player player;

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
        player.xrRig.headset = headset;
        player.xrRig.leftController = leftController;
        player.xrRig.rightController = rightController;

        player.colliderRig.head = headCollider;
        player.colliderRig.body = bodyCollider;
        player.colliderRig.leftHand = leftHandCollider;
        player.colliderRig.rightHand = rightHandCollider;

        player.rigidbodyRig.head = headRigidbody;
        player.rigidbodyRig.body = bodyRigidbody;
        player.rigidbodyRig.leftHand = leftHandRigidbody;
        player.rigidbodyRig.rightHand = rightHandRigidbody;

        // Since this no longer needs to be run disable the behaviour.
        this.enabled = false;
    }
}
