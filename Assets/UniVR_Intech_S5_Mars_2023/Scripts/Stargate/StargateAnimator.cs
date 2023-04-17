using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RingRotation
{
    CounterClockwise,
    Clockwise
}

public class RingGlyths
{
    private static float[] ringGlyphsRotationValue = {
        0,       // 0
        9.3f,    // 1
        18.55f,  // 2
        27.79f,  // 3
        37.2f,   // 4
        46.1f,   // 5
        55.7f,   // 6
        64.5f,   // 7
        73.8f,   // 8
        82.9f,   // 9
        92.3f,   // 10
        101.2f,  // 11
        110.4f,  // 12
        119.8f,  // 13
        129.1f,  // 14
        138.3f,  // 15
        147.4f,  // 16
        156.6f,  // 17
        165.9f,  // 18
        175,     // 19
        184.5f,  // 20
        193.7f,  // 21
        202.9f,  // 22
        212.3f,  // 23
        221.8f,  // 24
        230.9f,  // 25
        239.9f,  // 26
        249,     // 27
        258.6f,  // 28
        268,     // 29
        277.1f,  // 30
        286,     // 31
        295.3f,  // 32
        305,     // 33
        314.1f,  // 34
        323.2f,  // 35
        332.4f,  // 36
        342,     // 37
        351.2f   // 38
        };

    public static float GetRingGlyphRotation(GlyphsList glyphs)
    {
        return ringGlyphsRotationValue[(int)glyphs];
    }
}

public class StargateAnimator : MonoBehaviour
{
    // Public Fields.
    [Range(0, 10)]
    public float speed = 1f;
    
    // Setup fields.
    public Transform Stargate;
    public Transform ring;

    // Program attributes.
    private GlyphsList[] storedGlyphsSequence = new GlyphsList[7];
    [SerializeField]
    private GlyphsList selectedGlyph;
    private GlyphsList oldGlyph;
    private RingRotation targetDirection = RingRotation.CounterClockwise;
    private float targetRotation = 0;

    Quaternion currentRotation;
    Quaternion targetQuaternion;

    // Event Triggers.
    private bool isDialling = false;
    private bool isActive = false;
    [Range(0, 7)]
    private int chevronLvl = 0;
    private bool isRingStart = false;


    // Start is called before the first frame update
    void Start()
    {
        StartGateSequence(new GlyphsList[]{
            GlyphsList.Taurus,
            GlyphsList.Serpens_Caput,
            GlyphsList.Capricornus,
            GlyphsList.Monoceros,
            GlyphsList.Sagittarius,
            GlyphsList.Orion,
            GlyphsList.Giza
        });
    }

    // Update is called once per frame
    // Only update the rings position each time a frame is generated. Don't bother updating it if another frame isn't generated :D.
    void Update()
    {
        if (selectedGlyph != oldGlyph)
        {
            targetRotation = RingGlyths.GetRingGlyphRotation(selectedGlyph);
            oldGlyph = selectedGlyph;
            isRingStart = true;
            Debug.Log("Current Selected Glypth is : " + selectedGlyph + " with a target rotation of : " + targetRotation + "°.");
        }

        UpdateRing(targetRotation);
    }

    public void StartGateSequence(GlyphsList[] sequence)
    {
        if (sequence.Length == 7 && !isDialling && !isActive)
        {
            storedGlyphsSequence = sequence;
            isDialling = true;
        }
    }

    private void UpdateRing(float targetRotation)
    {
        // Update the rotation of the Stargate's ring using targetRotation argument.
        // !!! Note it aligns to X & Y but rotating the Stargate on Z will break it :'D !!!
        currentRotation = ring.rotation.normalized;
        targetQuaternion = Quaternion.Euler(targetRotation, Stargate.eulerAngles.y, Stargate.eulerAngles.z).normalized;
        ring.rotation = Quaternion.Lerp(currentRotation, targetQuaternion, speed * Time.deltaTime);
    }
}
