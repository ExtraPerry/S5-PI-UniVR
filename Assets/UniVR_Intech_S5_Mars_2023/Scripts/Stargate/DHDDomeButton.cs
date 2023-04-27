using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHDDomeButton : MonoBehaviour
{
    [SerializeField]
    private GameEvent eventToTrigger;
    [SerializeField]
    private string[] interactionTags = new string[2];

    private void OnCollisionEnter(Collision collision)
    {
        foreach (string element in interactionTags)
        {
            if (!collision.collider.CompareTag(element)) return;
        }
        eventToTrigger.Raise(this, null);
    }
}
