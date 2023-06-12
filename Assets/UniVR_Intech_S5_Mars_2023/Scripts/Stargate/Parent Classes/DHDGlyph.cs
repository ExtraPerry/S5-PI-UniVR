using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHDGlyph : MonoBehaviour
{
    public GameEvent eventToTrigger;
    public Glyph glyphButtonType;

    public void GlyphPressed()
    {
        eventToTrigger.Raise(this, glyphButtonType);
    }
}
