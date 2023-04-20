using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DHD : MonoBehaviour
{
    [SerializeField]
    private DHD self;
    [SerializeField]
    private Glyphs glyphsLibrary;
    [SerializeField]
    private Button dialButton;
    [SerializeField]
    private Button[] glyphButtons = new Button[39];
    [SerializeField]
    private Image[] glyphDisplays = new Image[7];
    [SerializeField]
    private StargateAnimator stargate;

    private TMPro.TMP_Text status;
    private Glyph[] storedGlyphSequence = null;
    private Stack<Glyph> activeGlyphs = new Stack<Glyph>();
    private int glyphCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        glyphCount = 0; 
        if (!IsGlyphDisplaysEmptyOrNull())
        {
            foreach (Image element in glyphDisplays)
            {
                element.color = new Color(1, 0.666f, 0, 1);
                element.material = glyphsLibrary.GetBlankGlyphMaterial();
            }
        }
        if (!IsGlyphButtonsEmptyOrNull())
        {
            foreach (Button element in glyphButtons)
            {
                element.image.color = new Color(1, 1, 1, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SymbolePressed(int glyphNumber)
    {
        SymbolePressed((Glyph)glyphNumber);
    }

    public void SymbolePressed(Glyph glyph)
    {
        if (IsStoredGlyphSequenceEmptyOrNull())
        {
            storedGlyphSequence = new Glyph[7];
        }

        if (activeGlyphs.Contains(glyph) || stargate.IsGateOccupied())
        {
            Debug.Log("Glyph : " + glyph + " has already been selected or Gate already has an address.");
            return;
        }

        if (glyphCount <= 6)
        {
            activeGlyphs.Push(glyph);
            storedGlyphSequence[glyphCount] = glyph;
            glyphButtons[(int)glyph].image.color = new Color(1, 0.666f, 0, 1);
            glyphDisplays[glyphCount].material = glyphsLibrary.GetGlyphMaterial(glyph);
            glyphCount++;

            Debug.Log("Glyph : " + glyph + " has been added to DHD sequence.");
        }
        else
        {
            ResetDHD();

            Debug.Log("More than 7 glyphs have been input. Reseting DHD.");
        }
    }

    public void DialPressed()
    {
        if (glyphCount != 7)
        {
            Debug.Log("DHD has attempted to dial but sequence was incomplete.");
            ResetDHD();
            return;
        }

        // (sequence.Length == 7) && !IsGateOccupied())
        bool isSuccesful = stargate.StartGateSequence(storedGlyphSequence);
        if (isSuccesful)
        {
            dialButton.image.color = new Color(1, 0.666f, 0, 1);
            if (!IsGlyphDisplaysEmptyOrNull())
            {
                foreach (Image element in glyphDisplays)
                {
                    element.color = new Color(1, 0.666f, 0, 1);
                }
            }

            Debug.Log("DHD has confirmed Gate is starting.");
        }
        else
        {
            stargate.StargateInterrupt();
            ResetDHD();

            Debug.Log("Gate has refused input.");
        }
    }

    public void ResetDHD()
    {
        if (!IsStoredGlyphSequenceEmptyOrNull())
        {
            foreach (Glyph element in storedGlyphSequence)
            {
                glyphButtons[(int)element].image.color = new Color(1, 1, 1, 1);
            }
        }
        if (!IsGlyphDisplaysEmptyOrNull())
        {
            foreach (Image element in glyphDisplays)
            {
                element.material = glyphsLibrary.GetBlankGlyphMaterial();
            }
        }
        dialButton.image.color = new Color(0, 0.75f, 0, 1);
        storedGlyphSequence = null;
        activeGlyphs.Clear();
        glyphCount = 0;

        Debug.Log("DHD has been reset.");
    }

    private bool IsStoredGlyphSequenceEmptyOrNull()
    {
        return storedGlyphSequence == null || storedGlyphSequence.Length == 0;
    }

    private bool IsGlyphButtonsEmptyOrNull()
    {
        return glyphDisplays == null || glyphDisplays.Length == 0;
    }

    private bool IsGlyphDisplaysEmptyOrNull()
    {
        return glyphDisplays == null || glyphDisplays.Length == 0;
    }
}
