using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Synced Bool")]
public class SyncedBool : ScriptableObject
{
    private bool syncedBool;

    public void Set(bool value)
    {
        syncedBool = value;
    }

    public bool Get()
    {
        return syncedBool;
    }
}
