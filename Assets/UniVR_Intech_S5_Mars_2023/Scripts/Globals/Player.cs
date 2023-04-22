using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Collider body;
    public Collider leftHand;
    public Collider rightHand;
}


public class Player : MonoBehaviour
{
    public static string playerName;
    public RigidbodyRig rigidbodyRig;
    public ColliderRig colliderRig;
}