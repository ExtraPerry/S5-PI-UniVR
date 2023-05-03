using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpawnSetup : MonoBehaviour
{
    [SerializeField]
    private SyncedTransform worldSpawn;

    // Start is called before the first frame update
    void Start()
    {
        worldSpawn.Set(gameObject.transform);
        this.enabled = false;
    }
}
