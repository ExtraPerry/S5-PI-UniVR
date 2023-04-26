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
        dome.SetColor("_Emission Color", new Color(0.75f, 0, 0, 0));
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
    }

    public void SymbolePressed(int glyphNumber)
    {
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
        dome.SetColor("_Emission Color", new Color(0.75f, 0, 0, 0));
        activeGlyphs.Clear();
        Start();

        Debug.Log("DHD has been reset.");
    }
}
