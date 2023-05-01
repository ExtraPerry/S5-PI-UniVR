using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DHD : MonoBehaviour
{
    [SerializeField]
    private GameEvent stagateCall;
    [SerializeField]
    private Material dome;
    [SerializeField]
    private Material glyphsSymbole;
    [SerializeField]
    private Material glyphsEdge;
    [SerializeField]
    private AudioSource[] sfx = new AudioSource[7];
    [SerializeField]    // Debug
    private Stack<Glyph> activeGlyphs = new Stack<Glyph>();

    // Start is called before the first frame update
    void Start()
    {
        dome.SetColor("_EmissionColor", new Color(0.75f, 0, 0, 0));
        glyphsSymbole.SetFloat("_Chevron_1", -1);
        glyphsSymbole.SetFloat("_Chevron_2", -1);
        glyphsSymbole.SetFloat("_Chevron_3", -1);
        glyphsSymbole.SetFloat("_Chevron_4", -1);
        glyphsSymbole.SetFloat("_Chevron_5", -1);
        glyphsSymbole.SetFloat("_Chevron_6", -1);
        glyphsSymbole.SetFloat("_Chevron_7", -1);
        glyphsEdge.SetFloat("_Chevron_1", -1);
        glyphsEdge.SetFloat("_Chevron_2", -1);
        glyphsEdge.SetFloat("_Chevron_3", -1);
        glyphsEdge.SetFloat("_Chevron_4", -1);
        glyphsEdge.SetFloat("_Chevron_5", -1);
        glyphsEdge.SetFloat("_Chevron_6", -1);
        glyphsEdge.SetFloat("_Chevron_7", -1);
        activeGlyphs.Clear();
    }

    // To be used with a GameEventListener monoscript.
    public void OnGateOverride(Component sender, object data)
    {
        if (!((sender is Stargate) && (data is Glyph[]))) return;
        Glyph[] glyphSequence = (Glyph[])data;

        // Check if the entered sequence is of proper size.
        if (!(glyphSequence.Length == 7)) return;

        // Check if the entered sequence contains a dupplicate glyph.
        for (int i = 0; i < glyphSequence.Length; i++)
        {
            for (int j = 0; j < glyphSequence.Length; j++)
            {
                if (i != j) // Ignore the same index checks.
                {
                    if (glyphSequence[i] == glyphSequence[j]) return;
                }
            }
        }

        ResetDHD();
        // Assign new overide glyph sequence to the DHD.
        for (int i = 0; i < 7; i++)
        {
            SymbolePressed(glyphSequence[i]);
        }
    }

    // To be used with a GameEventListener monoscript.
    public void OnTouchePressed(Component sender, object data)
    {
        if (!((sender is DHDGlyphButton) && (data is Glyph))) return;
        Glyph glyph = (Glyph)data;

        SymbolePressed(glyph);
    }

    // To be used with a GameEventListener monoscript.
    public void OnDomePressed(Component sender, object data)
    {
        if (!(sender is DHDDomeButton)) return;

        if (activeGlyphs.Count != 7)
        {
            Debug.Log("DHD has attempted to dial but sequence was incomplete.");
            ResetDHD();
            return;
        }

        // Send the sequence to the Gate.
        Glyph[] sequence = activeGlyphs.ToArray();
        System.Array.Reverse(sequence);
        stagateCall.Raise(this, sequence);
    }

    public void OnDHDReset(Component sender, object data)
    {
        if (!(sender is Stargate)) return;

        ResetDHD();
    }

    private void SymbolePressed(Glyph glyph)
    {
        if (activeGlyphs.Contains(glyph))
        {
            Debug.Log("Glyph : " + glyph + " has already been selected.");
            return;
        }

        if (activeGlyphs.Count > 6) return;

        // Add the glyph to the sequence.
        activeGlyphs.Push(glyph);
        sfx[activeGlyphs.Count - 1].Play();

        // Update the DHD visuals.
        UpdateTouchesVisual();

        Debug.Log("Glyph : " + glyph + " has been added to DHD sequence.");
    }

    private void UpdateTouchesVisual()
    {
        // Retrieve the current glyph sequence.
        Glyph[] sequence = activeGlyphs.ToArray();
        System.Array.Reverse(sequence);

        // Update the materials.
        int selected = (int)activeGlyphs.Peek();
        glyphsSymbole.SetFloat("_Chevron_" + activeGlyphs.Count, selected);
        glyphsEdge.SetFloat("_Chevron_" + activeGlyphs.Count, selected);
    }

    private void ResetDHD()
    {
        Start();

        Debug.Log("DHD has been reset.");
    }
}
