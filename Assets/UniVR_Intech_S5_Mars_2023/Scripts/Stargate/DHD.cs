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
    private int glyphCount = 0;
    private bool isGateOccupied = false;

    // Start is called before the first frame update
    void Start()
    {
        glyphCount = 0;
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
        if (storedGlyphSequence == null) storedGlyphSequence = new Glyph[7];

        if (glyphCount <= 6)
        {
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

    public void AttemptDialling()
    {
        if (glyphCount != 7) return;

        bool isSuccesful = stargate.StartGateSequence(storedGlyphSequence);
        if (isSuccesful)
        {
            dialButton.image.color = new Color(1, 0.666f, 0, 1);
            isGateOccupied = true;

            Debug.Log("DHD has confirmed Gate is starting.");
        }
        else
        {
            stargate.StargateInterrupt();
            ResetDHD();

            Debug.Log("DHD has ordered Gate to shut down.");
        }
    }

    public void ResetDHD()
    {
        foreach(Glyph element in storedGlyphSequence)
        {
            glyphButtons[(int)element].image.color = new Color(1, 1, 1, 1);
        }
        foreach(Image element in glyphDisplays)
        {
            element.material = glyphsLibrary.GetBlankGlyphMaterial();
        }
        dialButton.image.color = new Color(0, 0.75f, 0, 1);
        storedGlyphSequence = null;
        glyphCount = 0;
        isGateOccupied = false;

        Debug.Log("DHD has been reset.");
    }
}
