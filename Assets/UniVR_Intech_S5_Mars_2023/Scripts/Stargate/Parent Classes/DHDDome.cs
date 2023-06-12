using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHDDome : MonoBehaviour
{
    public GameEvent eventToTrigger;

    public void DomePressed()
    {
        eventToTrigger.Raise(this);
    }
}
