using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    public InputActionProperty leftCancel;
    public InputActionProperty rightCancel;

    public XRRayInteractor leftRay;
    public XRRayInteractor rgihtRay;

    // Update is called once per frame
    void Update()
    {
        bool isLeftTriggerPulled = leftActivate.action.ReadValue<float>() > 0.1f;
        bool isRightTriggerPulled = rightActivate.action.ReadValue<float>() > 0.1f;

        bool isLeftGripReleased = leftCancel.action.ReadValue<float>() < 0.15f;
        bool isRightGripReleased = rightCancel.action.ReadValue<float>() < 0.15f;

        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);
        bool isRightRayHovering = leftRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool lrightValid);

        leftTeleportation.SetActive((isLeftTriggerPulled && isLeftGripReleased) && (!isLeftRayHovering));
        rightTeleportation.SetActive((isRightTriggerPulled && isRightGripReleased) && (!isRightRayHovering));
    }
}
