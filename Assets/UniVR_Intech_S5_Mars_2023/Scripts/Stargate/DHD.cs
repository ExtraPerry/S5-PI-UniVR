using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DHD : MonoBehaviour
{
    [SerializeField]
    private Glyphs glyphsLibrary;
    [SerializeField]
    private Material dome;
    [SerializeField]
    private Material glyphsSymbole;
    [SerializeField]
    private Material glyphsEdge;
    [SerializeField]
    private StargateAnimator stargate;
    [SerializeField]
    private AudioSource[] sfx = new AudioSource[7];

    private Stack<Glyph> activeGlyphs = new Stack<Glyph>();

    // Start is called before the first frame update
    void Start()
    {
        
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
            activeGlyphs.Push(glyph);
            Debug.Log("Glyph : " + glyph + " has been added to DHD sequence by the Gate override.");
        }
        UpdateStatus();
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
    }
}
