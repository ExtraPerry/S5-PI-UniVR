using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Synced Transform")]
public class SyncedTransform : ScriptableObject
{
    private Transform syncedTransform;

    public void Set(Transform value)
    {
        syncedTransform = value;
    }

    public Transform Get()
    {
        return syncedTransform;
    }
}
