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
            Debug.LogWarning("\"" + gameObject.name + " (" + gameObject.GetInstanceID() + ")" + "\" doesn't have a rigidbody when it should, disabling " + this.name + " component.");
            this.enabled = false;
        }
    }

   public void MatchGrabPoint(Side handSide, ConfigurableJoint joint)
    {
        

        /*
        if (handSide == Side.Unspecified) return;

        Vector3 offset = Vector3.zero;
        Quaternion rotation = joint.transform.rotation;

        switch (handSide)
        {
            case Side.Left:
                offset = leftHand.position;
                break;

            case Side.Right:
                offset = rightHand.position;
                break;
        }

        Debug.Log("Updating \"" + gameObject.name + " (" + gameObject.GetInstanceID() + ")" + "\" position & rotation to match grab points.");
        joint.targetPosition += offset;
        joint.targetRotation = rotation;
        */
    }
}
