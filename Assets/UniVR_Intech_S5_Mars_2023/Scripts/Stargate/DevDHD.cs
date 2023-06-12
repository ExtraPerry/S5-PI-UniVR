using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevDHD : MonoBehaviour
{
    [SerializeField]
    private SyncedGlyphSequence worldGateSequence;
    [SerializeField]
    private GameEvent stargateStart;
    [SerializeField]
    private GameEvent stargateInterrupt;
    [SerializeField]
    private Glyphs glyphsLibrary;
    [SerializeField]
    private Button dialButton;
    [SerializeField]
    private Button[] glyphButtons = new Button[39];
    [SerializeField]
    private Image[] glyphDisplays = new Image[7];
    [SerializeField]
    private TMPro.TMP_Text status;
    [SerializeField]
    private AudioSource[] sfx = new AudioSource[7];

    // Synced attributes.
    [SerializeField]
    private SyncedBool isGateOccupied;

    private Stack<Glyph> activeGlyphs = new Stack<Glyph>();

    // Start is called before the first frame update
    void Start()
    {
        worldGateSequence.GenerateNewSequence();

        dialButton.image.color = new Color(0, 0.75f, 0, 1);
        if (!IsGlyphDisplaysEmptyOrNull())
        {
            foreach (Image element in glyphDisplays)
            {
                element.color = new Color(1, 0.666f, 0, 1);
                element.material = glyphsLibrary.GetDevBlankGlyphMaterial();
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

    public void OnGateOverride(Component sender, object data)
    {
        if (!((sender is Stargate) && (data is Glyph[]))) return;
        Glyph[] glyphSequence = (Glyph[])data;

        ResetDHD();
        for (int i = 0; i < 7; i++)
        {
            Glyph glyph = glyphSequence[i];
            glyphButtons[(int)glyph].image.color = new Color(1, 0.666f, 0, 1);
            glyphDisplays[i].material = glyphsLibrary.GetDevGlyphMaterial(glyph);
            glyphDisplays[i].color = new Color(0, 0.75f, 0, 1);
            activeGlyphs.Push(glyph);
            Debug.Log("Glyph : " + glyph + " has been added to DHD sequence by the Gate override.");
        }
        UpdateStatus();
        dialButton.image.color = new Color(1, 0.666f, 0, 1);
    }

    public void OnSymbolePressed(Component sender, object data)
    {
        if (!(sender is DHDGlyphButton || sender is DevDHDGlyphEditorButton) || data is not Glyph)
        {
            Debug.LogWarning(sender.name + " or " + data.GetType() + " are not accepted sender or data.");
            return;
        }
        Glyph glyph = (Glyph)data;
        SymbolePressed(glyph);
    }

    public void SymbolePressed(int glyphNumber)
    {
        SymbolePressed((Glyph)glyphNumber);
    }

    public void SymbolePressed(Glyph glyph)
    {
        if (activeGlyphs.Contains(glyph))
        {
            Debug.Log("Glyph : " + glyph + " has already been selected or Gate already has an address.");
            return;
        }

        if (activeGlyphs.Count <= 6)
        {
            activeGlyphs.Push(glyph);
            glyphButtons[(int)glyph].image.color = new Color(1, 0.666f, 0, 1);
            glyphDisplays[activeGlyphs.Count - 1].material = glyphsLibrary.GetDevGlyphMaterial(glyph);
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

    public void OnDomePressed(Component sender, object data)
    {
        if (!(sender is DHDDomeButton || sender is DevDHDDomeEditorButton))
        {
            Debug.LogWarning(sender.name + " or " + data.GetType() + " are not accepted sender or data.");
            return;
        }
        DialPressed();
    }

    public void DialPressed()
    {
        // Check if gate is occupied.
        if (isGateOccupied.Get())
        {
            stargateInterrupt.Raise(this);
            Debug.Log("DHD has told the Gate to interupt.");
            return;
        }

        // Check if sequenc if full.
        if (activeGlyphs.Count != 7)
        {
            Debug.Log("DHD has attempted to dial but sequence was incomplete.");
            ResetDHD();
            return;
        }

        // Translate sequence into an array for later use.
        Glyph[] sequence = activeGlyphs.ToArray();
        System.Array.Reverse(sequence);

        // Check of sequence mateches world sequence.
        if (sequence != worldGateSequence.Get())
        {
#if UNITY_EDITOR    // Debugging Parts
            int count = 0;
            string worldSequenceString = "";
            foreach (Glyph glyph in worldGateSequence.Get())
            {
                worldSequenceString += glyph.ToString();
                if (count != worldGateSequence.Get().Length) worldSequenceString += ", ";
                count++;
            }
            count = 0;
            string dhdSequenceString = "";
            foreach (Glyph glyph in worldGateSequence.Get())
            {
                dhdSequenceString += glyph.ToString();
                if (count != sequence.Length) dhdSequenceString += ", ";
                count++;
            }
            Debug.Log("Input sequence on DHD does not match world sequence for next level.\nWorld : " + worldSequenceString + "\nDHD : " + dhdSequenceString);
#endif
            ResetDHD();
            return;
        }
        
        // Update Dev DHD UI.
        dialButton.image.color = new Color(1, 0.666f, 0, 1);
        if (!IsGlyphDisplaysEmptyOrNull())
        {
            foreach (Image element in glyphDisplays)
            {
                element.color = new Color(0, 0.75f, 0, 1);
            }
        }

        // Start the Stargate.
        stargateStart.Raise(this, sequence);

        Debug.Log("DHD has confirmed Gate is starting.");
    }

    public void OnResetDHD(Component sender, object data)
    {
        if (sender is not Stargate) return;
        ResetDHD();
    }

    private void ResetDHD()
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
