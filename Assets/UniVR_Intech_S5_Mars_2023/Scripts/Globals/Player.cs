using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XrRig
{
    public Transform headset;
    public Transform leftController;
    public Transform rightController;
}

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

[System.Serializable]
public class ColliderRig
{
    public Collider head;
    public CapsuleCollider body;
    public Collider leftHand;
    public Collider rightHand;
}

[CreateAssetMenu(menuName = "Player")]
public class Player : ScriptableObject
{
    public XrRig xrRig;
    public RigidbodyRig rigidbodyRig;
    public ColliderRig colliderRig;
    public SyncedTransform respawnLocation;
}