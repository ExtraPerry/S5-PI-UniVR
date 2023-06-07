using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Synced GlyphSequence")]
public class SyncedGlyphSequence : ScriptableObject
{
    [SerializeField]
    public Glyph[] storedGlyphSequence;

    public void Set(Glyph[] glyphSequence)
    {
        storedGlyphSequence = glyphSequence;
    }

    public Glyph[] Get()
    {
        return storedGlyphSequence;
    }

    public void GenerateNewSequence()
    {
        storedGlyphSequence = new Glyph[7];
        // Fill new storedGlyphSequence with glyphs, making sure they are all unique.
        // Note that Giza cannot be the first glyph as the gate will always use that one as the default starting position.
        for (int i = 0; i < storedGlyphSequence.Length; i++)
        {
            bool again;
            Glyph randomGlyph;
            do
            {
                again = false;
                if (i == 0)
                {
                    randomGlyph = (Glyph)Random.Range(1, 38);
                }
                else
                {
                    randomGlyph = (Glyph)Random.Range(0, 38);
                }
                
                foreach (Glyph glyph in storedGlyphSequence)
                {
                    if (randomGlyph == glyph)
                    {
                        again = true;
                        break;
                    }
                }
            } while (again);
            storedGlyphSequence[i] = randomGlyph;
        }
    }
}
