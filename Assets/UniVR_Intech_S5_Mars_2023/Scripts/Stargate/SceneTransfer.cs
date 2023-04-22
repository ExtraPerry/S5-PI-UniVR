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
    private Collider entranceCollider;
    [SerializeField]
    private Collider2D eventHorizonCollider;
    [SerializeField]
    private Material[] colliderOffOnMaterials = new Material[2];

    [SerializeField]
    private int targetScene = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
            SceneManager.LoadScene(targetScene);
            Debug.Log("Player stepped through the gate.");
        }
    }

    public void UpdateTargetScene(int target)
    {
        targetScene = target;
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
        if (entranceMeshRenderer)
        {
            if (isEntranceOk)
            {
                entranceMeshRenderer.GetComponent<MeshRenderer>().material = colliderOffOnMaterials[0];
            }
            else
            {
                entranceMeshRenderer.GetComponent<MeshRenderer>().material = colliderOffOnMaterials[1];
            }
        }

        MeshRenderer eventHorizonMeshRenderer = eventHorizonCollider.gameObject.GetComponent<MeshRenderer>();
        if (eventHorizonMeshRenderer)
        {
            if (isEventHorizonOk)
            {
                eventHorizonMeshRenderer.GetComponent<MeshRenderer>().material = colliderOffOnMaterials[0];
            }
            else
            {
                eventHorizonMeshRenderer.GetComponent<MeshRenderer>().material = colliderOffOnMaterials[1];
            }
        }
    }
}
