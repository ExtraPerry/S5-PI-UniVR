using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHDGlyphButton : MonoBehaviour
{
    [SerializeField]
    private Collider[] permittedColliders;
    [SerializeField]
    private LayerMask permittedLayers;
    [SerializeField]
    private Glyph glyphButtonType;

    private Collider selfCollider;
    private DHD dhd;

    // Start is called before the first frame update
    void Start()
    {
        selfCollider = gameObject.GetComponent<Collider>();
        if (!selfCollider)
        {
            Debug.LogError("No collider found on self (" + gameObject.name + "), please add one.");
        }
        dhd = gameObject.GetComponentInParent<DHD>(false);
        if (!dhd)
        {
            Debug.LogError("No active DHD script found on parent of self (" + gameObject.name + "), please add one.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsButtonPressedPhysicaly())
        {
            dhd.SymbolePressed(glyphButtonType);
        }
    }

    public void RaycasterAction()
    {

    }

    private bool IsButtonPressedPhysicaly()
    {
        return false;
    }
}
