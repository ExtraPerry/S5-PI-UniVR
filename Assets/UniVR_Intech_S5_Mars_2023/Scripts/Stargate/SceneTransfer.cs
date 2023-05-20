using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    // Inputs.
    [SerializeField]
    private Player player;
    [SerializeField]
    private SyncedInt targetScene;
    [SerializeField]
    private Collider entranceCollider;
    [SerializeField]
    private Collider eventHorizonCollider;
    [SerializeField]
    private Material[] colliderOffOnMaterials = new Material[2];

    [SerializeField]
    private bool debug = false;
    
    // Update is called once per frame
    void Update()
    {
        bool isEntranceOk = false;
        if (IsRigIntersectsEntranceZone())
        {
            isEntranceOk = true;
        }

        bool isEventHorizonOk = false;
        if (IsRigHeadIntersectEventHorizon())
        {
            isEventHorizonOk = true;
        }
        
        UpdateDebugColliderVisual(isEntranceOk, isEventHorizonOk);
        
        if (isEntranceOk && isEventHorizonOk)
        {
            SceneManager.LoadScene(targetScene.Get());
            Debug.Log("Player stepped through the gate.");
        }
    }

    private bool IsRigIntersectsEntranceZone()
    {
        return entranceCollider.bounds.Intersects(player.colliderRig.head.bounds) && entranceCollider.bounds.Intersects(player.colliderRig.body.bounds) && entranceCollider.bounds.Intersects(player.colliderRig.leftHand.bounds) && entranceCollider.bounds.Intersects(player.colliderRig.rightHand.bounds);
    }

    private bool IsRigHeadIntersectEventHorizon()
    {
        return eventHorizonCollider.bounds.Intersects(player.colliderRig.head.bounds);
    }

    // Debug Stuff.
    private void UpdateDebugColliderVisual(bool isEntranceOk, bool isEventHorizonOk)
    {
        MeshRenderer entranceMeshRenderer = entranceCollider.gameObject.GetComponent<MeshRenderer>();
        MeshRenderer eventHorizonMeshRenderer = eventHorizonCollider.gameObject.GetComponent<MeshRenderer>();
        if (debug) {
            entranceMeshRenderer.enabled = true;
            eventHorizonMeshRenderer.enabled = true;

            if (entranceMeshRenderer)
            {
                if (isEntranceOk)
                {
                    entranceMeshRenderer.material = colliderOffOnMaterials[1];
                }
                else
                {
                    entranceMeshRenderer.material = colliderOffOnMaterials[0];
                }
            }

            if (eventHorizonMeshRenderer)
            {
                if (isEventHorizonOk)
                {
                    eventHorizonMeshRenderer.material = colliderOffOnMaterials[1];
                }
                else
                {
                    eventHorizonMeshRenderer.material = colliderOffOnMaterials[0];
                }
            }
        }
        else
        {
            entranceMeshRenderer.enabled = false;
            eventHorizonMeshRenderer.enabled = false;
        }
    }
}
