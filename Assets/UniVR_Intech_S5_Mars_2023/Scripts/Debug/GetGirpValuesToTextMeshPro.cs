using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GetGirpValuesToTextMeshPro : MonoBehaviour
{
    public TMPro.TMP_Text textOne;
    public TMPro.TMP_Text textTwo;

    public InputActionProperty leftGripSource;
    public InputActionProperty rightGripSource;

    // Update is called once per frame
    void Update()
    {
        float leftGripValue = leftGripSource.action.ReadValue<float>();
        float rightGripValue = rightGripSource.action.ReadValue<float>();

        textOne.text = "Left Hand Grab = " + leftGripValue;
        textTwo.text = "Right Hand Grab = " + rightGripValue;
    }
}
