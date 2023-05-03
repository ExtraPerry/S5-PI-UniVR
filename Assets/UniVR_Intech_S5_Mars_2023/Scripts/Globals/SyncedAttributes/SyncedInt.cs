using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Synced Int")]
public class SyncedInt : ScriptableObject
{
    private int syncedInt;

    public void Set(int value)
    {
        syncedInt = value;
    }

    public int Get()
    {
        return syncedInt;
    }
}
