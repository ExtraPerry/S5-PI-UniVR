using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevDHDDomeEditorButton : DHDDome
{
    public bool trigger;

    public void Update()
    {
        if (trigger)
        {
            DomePressed();
            trigger = false;
            Debug.Log("Manual Trigger : Dome Button.");
        }
    }
}
