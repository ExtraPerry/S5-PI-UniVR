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
    [SerializeField]
    private TMPro.TMP_Text status;
    [SerializeField]
    private AudioSource[] sfx = new AudioSource[7];

    private Stack<Glyph> activeGlyphs = new Stack<Glyph>();

    // Start is called before the first frame update
    void Start()
    {
        dialButton.image.color = new Color(0, 0.75f, 0, 1);
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
        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GateOverride(Glyph[] glyphSequence)
    {
        ResetDHD();
        for (int i = 0; i < 7; i++)
        {
            Glyph glyph = glyphSequence[i];
            glyphButtons[(int)glyph].image.color = new Color(1, 0.666f, 0, 1);
            glyphDisplays[i].material = glyphsLibrary.GetGlyphMaterial(glyph);
            glyphDisplays[i].color = new Color(0, 0.75f, 0, 1);
            activeGlyphs.Push(glyph);
            Debug.Log("Glyph : " + glyph + " has been added to DHD sequence by the Gate override.");
        }
        UpdateStatus();
        dialButton.image.color = new Color(1, 0.666f, 0, 1);
    }

    public void SymbolePressed(int glyphNumber)
    {
        Debug.Log("Glyph : " + glyphNumber + " => " + (Glyph)glyphNumber + ".");
        SymbolePressed((Glyph)glyphNumber);
    }

    public void SymbolePressed(Glyph glyph)
    {
        if (activeGlyphs.Contains(glyph) || stargate.IsGateOccupied())
        {
            Debug.Log("Glyph : " + glyph + " has already been selected or Gate already has an address.");
            return;
        }

        if (activeGlyphs.Count <= 6)
        {
            activeGlyphs.Push(glyph);
            glyphButtons[(int)glyph].image.color = new Color(1, 0.666f, 0, 1);
            glyphDisplays[activeGlyphs.Count - 1].material = glyphsLibrary.GetGlyphMaterial(glyph);
            sfx[activeGlyphs.Count - 1].Play();
            UpdateStatus();

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
        if (activeGlyphs.Count != 7)
        {
            Debug.Log("DHD has attempted to dial but sequence was incomplete.");
            ResetDHD();
            return;
        }

        // (sequence.Length == 7) && !IsGateOccupied())
        Glyph[] sequence = activeGlyphs.ToArray();
        System.Array.Reverse(sequence);
        bool isSuccesful = stargate.StartGateSequence(sequence);
        if (isSuccesful)
        {
            dialButton.image.color = new Color(1, 0.666f, 0, 1);
            if (!IsGlyphDisplaysEmptyOrNull())
            {
                foreach (Image element in glyphDisplays)
                {
                    element.color = new Color(0, 0.75f, 0, 1);
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
        activeGlyphs.Clear();
        Start();
        
        Debug.Log("DHD has been reset.");
    }

    private void UpdateStatus()
    {
        int count = 0;
        string[] stringGlyphs = new string[7];
        if (activeGlyphs.Count != 0)
        {
            Glyph[] glyphs = activeGlyphs.ToArray();
            System.Array.Reverse(glyphs);
            foreach (Glyph element in glyphs)
            {
                stringGlyphs[count] = element.ToString();
                count++;
            }
        }
        for (int i = count; i < stringGlyphs.Length; i++)
        {
            stringGlyphs[i] = "None";
        }
        status.text = "1 - [" + stringGlyphs[0] + "]\n2 - [" + stringGlyphs[1] + "]\n3 - [" + stringGlyphs[2] + "]\n4 - [" + stringGlyphs[3] + "]\n5 - [" + stringGlyphs[4] + "]\n6 - [" + stringGlyphs[5] + "]\n7 - [" + stringGlyphs[6] + "]";
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
