using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlyphTablet : MonoBehaviour
{
    [SerializeField]
    private SyncedGlyphSequence worldGlyphSequence;
    [SerializeField]
    [Range(1, 7)]
    private int selectedGlyph = 1;
    [SerializeField]
    private Glyphs glyphLibrary;
    [SerializeField]
    private Image glyphImage;
    [SerializeField]
    private TMPro.TMP_Text glyphName;
    [SerializeField]
    private TMPro.TMP_Text glyphNumber;

    void Start()
    {
        glyphNumber.text = selectedGlyph.ToString();
        glyphName.text = worldGlyphSequence.Get()[selectedGlyph - 1].ToString();
        glyphImage.material = glyphLibrary.GetDevGlyphMaterial(worldGlyphSequence.Get()[selectedGlyph - 1]);
    }
}
