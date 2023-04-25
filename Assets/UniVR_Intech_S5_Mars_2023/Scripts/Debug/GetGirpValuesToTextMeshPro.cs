using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GetGirpValuesToTextMeshPro : MonoBehaviour
{
    // The text mesh pro text zones.
    public TMPro.TMP_Text textOne;
    public TMPro.TMP_Text textTwo;
    // Input Maps.
    public InputActionProperty leftGripSource;
    public InputActionProperty rightGripSource;

    // Update is called once per frame
    void Update()
    {
        // Read the inputs.
        float leftGripValue = leftGripSource.action.ReadValue<float>();
        float rightGripValue = rightGripSource.action.ReadValue<float>();

        // Update the text to show the value of the inputs.
        textOne.text = "Left Hand Grab = " + leftGripValue;
        textTwo.text = "Right Hand Grab = " + rightGripValue;
    }
}
