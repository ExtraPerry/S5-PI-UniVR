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

    public static float GetRingGlyphRotation(GlyphsList glyphs, RingRotation ringRotation)
    {
        if (ringRotation == RingRotation.CounterClockwise)
        {
            return ringGlyphsRotationValue[(int)glyphs];
        }
        else
        {
            return ringGlyphsRotationValue[(int)glyphs] - 360;
        }
    }
}

public class StargateAnimator : MonoBehaviour
{
    // Rotational value in Degrees/Second.
    public float ringRotationSpeed = 55;
    public Transform gateRingBone;
    public Animator Stargate;

    private bool isDialling;
    private bool isActive;

    private GlyphsList[] glyphSequence = new GlyphsList[7];
    private RingRotation currentRotationalDirection;
    private float targetRotation;
    private int chevronsLocked;


    // Start is called before the first frame update
    void Start()
    {
        CloseGate();
        JumpRingToTarget(0);
    }

    // Update is called once per frame
    // Only update the rings position each time a frame is generated. Don't bother updating it if another frame isn't generated :D.
    void Update()
    {
        if (isActive)
        {
            return;
        }

        if (chevronsLocked == 7)
        {
            Stargate.SetBool("OpenClosed", true);
            isActive = true;
            isDialling = false;
            return;
        }

        if (isDialling && !isActive)
        {
            float nextRotation = ringRotationSpeed / Time.deltaTime;

            if (currentRotationalDirection == RingRotation.CounterClockwise)
            {
                nextRotation *= -1;
                if (gateRingBone.rotation.eulerAngles.x + nextRotation < targetRotation)
                {
                    JumpRingToTarget(targetRotation);
                    PrepareNextChevronSequence();
                    return;
                }
            }
            else
            {
                if (gateRingBone.rotation.eulerAngles.x + nextRotation > targetRotation)
                {
                    JumpRingToTarget(targetRotation);
                    PrepareNextChevronSequence();
                    return;
                }
            }

            JumpRingToTarget(gateRingBone.rotation.eulerAngles.x + nextRotation);
        }
    }

    public void StartDiallingSequence(GlyphsList[] sequence)
    {
        if (isDialling || isActive)
        {
            Debug.Log("Stargate is already dialling and/or activated. As such another dial sequence cannot be called.");
            return;
        }

        if (!(sequence.Length == 7))
        {
            Debug.Log("Stargate sequence code is incorrect. As such a dial sequence cannot be called.");
            return;
        }

        glyphSequence = sequence;
        chevronsLocked = 0;
        UpdateTargetRotation();
        isDialling = true;
    }

    public void CloseGate()
    {
        Stargate.SetBool("OpenClosed", false);
        Stargate.SetInteger("ChevronsLocked", 0);

        isDialling = false;
        isActive = false;

        currentRotationalDirection = RingRotation.CounterClockwise;
        chevronsLocked = 0;

    }

    private void JumpRingToTarget(float target)
    {
        gateRingBone.Rotate(new Vector3(target, 0, 0), Space.Self);
    }

    private void PrepareNextChevronSequence()
    {
        chevronsLocked++;
        Stargate.SetInteger("ChevronsLocked", chevronsLocked);

        if (currentRotationalDirection == RingRotation.CounterClockwise)
        {
            currentRotationalDirection = RingRotation.Clockwise;
        }
        else
        {
            currentRotationalDirection = RingRotation.CounterClockwise;
        }

        UpdateTargetRotation();
    }

    private void UpdateTargetRotation()
    {
        targetRotation = RingGlyths.GetRingGlyphRotation(glyphSequence[chevronsLocked], currentRotationalDirection);
    }
}
