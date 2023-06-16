using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameobjectOnPlay : MonoBehaviour
{
    private void Start()
    {
        DestroyImmediate(gameObject);
    }
}
