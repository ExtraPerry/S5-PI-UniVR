using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHDDomeButton : DHDDome
{
    public string[] interactionTags = new string[2];

    private void OnCollisionEnter(Collision collision)
    {
        foreach (string element in interactionTags)
        {
            if (!collision.collider.CompareTag(element)) return;
        }
        DomePressed();
    }
}
