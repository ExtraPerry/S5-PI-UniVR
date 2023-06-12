using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevDHDGlyphEditorButton : DHDGlyph
{
    public SyncedGlyphSequence worldSequence;
    public bool trigger = false;
    public bool fill = false;

    public void Update()
    {
        if (trigger)
        {
            GlyphPressed();
            trigger = false;
            Debug.Log("Manual Trigger : Glyph Button.");
        }

        if (fill)
        {
            foreach (Glyph glyph in worldSequence.Get())
            {
                glyphButtonType = glyph;
                GlyphPressed();
            }
            fill = false;
            Debug.Log("Manual Fill : Glyph Button.");
        }
    }
}
