using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side
{
    Unspecified,
    Left,
    Right
} 

public class GrabbablePoint : MonoBehaviour
{
    [Header("Anchor Points :")]
    public Transform leftHand;
    public Transform rightHand;

    // Start is called before the first frame update
    void Start()
    {
        if (leftHand is null) leftHand = transform;
        if (rightHand is null) rightHand = transform;

        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (!rigidbody)
        {
            Debug.LogWarning("\"" + gameObject.name + "\" doesn't have a rigidbody when it should.");
        }
    }

   public void MatchGrabPoint(Side handSide, Transform handTransform)
    {
        if (handSide == Side.Unspecified) return;

        Vector3 offset = Vector3.zero;
        Quaternion rotation = transform.rotation;

        switch (handSide)
        {
            case Side.Left:
                offset = leftHand.position - transform.position;
                rotation = leftHand.rotation;
                break;

            case Side.Right:
                offset = rightHand.position - transform.position;
                rotation = rightHand.rotation;
                break;
        }

        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            Debug.Log("Updating \"" + gameObject.name + "\" position & rotation to match grab points.");
            rigidbody.position = handTransform.position + offset;
            rigidbody.rotation = rotation;
        }
    }
}
