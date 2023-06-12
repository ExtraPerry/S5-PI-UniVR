using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHDGlyphButton :  DHDGlyph
{
    public string[] interactionTags = new string[2];

    public void OnCollisionEnter(Collision collision)
    {
        foreach (string element in interactionTags)
        {
            if (!collision.collider.CompareTag(element)) return;
        }
        GlyphPressed();
    }
}
