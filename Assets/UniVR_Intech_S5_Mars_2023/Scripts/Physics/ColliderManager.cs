using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [SerializeField]
    private Collider[] colliders;

    private void Start()
    {
        colliders = gameObject.GetComponentsInChildren<Collider>();
    }

    public void EnableColliders()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    public void EnableCollidersWithDelay(float delay)
    {
        Invoke("EnableColliders", delay);
    }

    public void DisableColliders()
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
