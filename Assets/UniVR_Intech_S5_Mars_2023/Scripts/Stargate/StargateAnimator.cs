using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static float GetRingGlyphRotation(Glyph glyphs)
    {
        return ringGlyphsRotationValue[(int)glyphs];
    }
}

public enum RotationDirection
{
    CounterClockwise = 1,
    Clockwise = -1
}

public class StargateAnimator : MonoBehaviour
{
    // Settings.
    [SerializeField]
    private DHD dhd;
    public AnimationCurve speedCurve = new AnimationCurve(new Keyframe[] {
        new Keyframe(0, 0.1f),
        new Keyframe(0.1f, 0.35f, 1.81539f, 1.815319f),
        new Keyframe(0.33f, 0.5f),
        new Keyframe(0.66f, 0.5f),
        new Keyframe(1, 0.1f),
    });
    [Range(0, 360)]
    public float speedDegreesPerSecond = 15f;
    [Range(1,5)]
    public int spinMultiplier = 2;
    public AnimationCurve momentumCurve = new AnimationCurve(new Keyframe[] {
        new Keyframe(0, 1, -2.011582f, -2.011582f),
        new Keyframe(1, 0)
    });
    [Range(1, 2.5f)]
    public float remainingMomentumInSeconds = 1;

    // External elements.
    public Animator animator;
    public Transform Stargate;
    public Transform ring;

    // Address & Glyph information.
    private Glyph[] storedGlyphsSequence = null;
    [SerializeField] // Let's you manually change the value inside of the editor, but not in the true game.
    private Glyph selectedGlyph;
    private Glyph targetGlyth;

    // Ring rotation animation attributes.
    private RotationDirection direction = RotationDirection.CounterClockwise;
    private float targetXRotation = 0;
    private float currentXRotation = 0;
    private float startXRotation = 0;
    private bool isRingSpinning = false;
    private float ringProgress = 0;

    // Ring momentum attributes when the gate is interrupted mid dialling sequence.
    private bool isRingMaintainingMomentum = false;
    private float remainingMomentumSeconds = 1;
    private float momentumClamp = 1;

    // Event Triggers.
    private int chevronLvl = 0;
    private bool isGateActive = false;
    [SerializeField] // Let's you manually change the value inside of the editor, but not in the true game.
    private bool interruptGate = false;

    // Timeout parameters.
    private bool isAnimationTimeout = false;
    private float animationTimeoutAmount = 1;
    private float animationTimeout = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 0 - Ring should always update even if interupt has been called. It needs to maintain visible momentum.
        UpdateRing();

        // 1 - Check if gate is animating with Anomator Controller. Skip frame if so.
        UpdateAnimationTimeout();
        if (isAnimationTimeout) return;

        // 2 - Check if startgate has an address. If not then nothing to dial, though still reset interrupt if needed.
        if (storedGlyphsSequence == null)
        {
            if (interruptGate) interruptGate = false;
            return;
        }
            
        // 2 - Check for interrupt. If the event horizon is active close it first then next frame interrupt is called again to turn off the gate completly.
        if (interruptGate)
        {
            if (isGateActive)
            {
                // Turn off the event horizon.
                CloseStargate();
                return;
            }
            else
            {
                // Turn off the stargate completly & reset interrupt.
                ResetStargate();
                return;
            }
        }

        // 3 - Check for the various phases the gate can have.
        if (!isGateActive)
        {
            if (selectedGlyph != targetGlyth) SetupRingForRotation();
            if (!isRingSpinning) UpdateChevrons();
        }
        else // If the Gate is activated check if the animator is up to date.
        {
            // If the animator is not up to date then update it.
            if (!animator.GetBool("EventHorizon"))
            {
                animator.SetBool("EventHorizon", isGateActive);
                SetupAnimationTimeout(0.2f);
            }
        }
    }

    // Public Methodes.
    public bool StartGateSequence(Glyph[] sequence)
    {
        if ((sequence.Length == 7) && !IsGateOccupied() && (sequence != null))
        {
            storedGlyphsSequence = sequence;
            selectedGlyph = storedGlyphsSequence[0];

            Debug.Log("Dialling : [" + storedGlyphsSequence[0] + ", " + storedGlyphsSequence[1] + ", " + storedGlyphsSequence[2] + ", " + storedGlyphsSequence[3] + ", " + storedGlyphsSequence[4] + ", " + storedGlyphsSequence[5] + ", " + storedGlyphsSequence[6] + "] address.");

            return true;
        }
        return false;
    }

    public bool IsGateOccupied()
    {
        return storedGlyphsSequence != null;
    }

    public void StargateInterrupt()
    {
        interruptGate = true;
    }

    // Private Methodes.
    private void CloseStargate()
    {
        isGateActive = false;
        animator.SetBool("EventHorizon", isGateActive);
        SetupAnimationTimeout(0.2f);

        Debug.Log("Closing event horizon.");
    }

    private void ResetStargate()
    {
        // Event Triggers.
        chevronLvl = 0;
        interruptGate = false;

        // Rando stuff.
        animator.SetInteger("ChevronsLocked", chevronLvl);
        remainingMomentumSeconds = remainingMomentumInSeconds;
        momentumClamp = speedCurve.Evaluate(ringProgress);
        isRingMaintainingMomentum = true;
        

        // Address & Glyph information.
        storedGlyphsSequence = null;

        // Animation timeout.
        SetupAnimationTimeout(0.15f);

        Debug.Log("Stargate shuting down.");
    }

    private void UpdateChevrons()
    {
        if (chevronLvl <= 7)
        {
            // First check if chevron is 0-6 and set it to the next one.
            if (chevronLvl <= 6) chevronLvl++;

            // Then check again after updating the active chevron if it's contained from 0-6 to be used for selecting the next glyph.
            if (chevronLvl <= 6) selectedGlyph = storedGlyphsSequence[chevronLvl];

            // Animate the active chevron locking.
            animator.SetInteger("ChevronsLocked", chevronLvl);
            SetupAnimationTimeout(1);

            //Check if gate has reached the 7th chevron.
            if (chevronLvl == 7) isGateActive = true;
        }
    }

    private void SetupRingForRotation()
    {
        // Remeber what rotation the ring is starting at.
        startXRotation = currentXRotation;

        // Change the direction it should spin based on previous direction.
        if (direction == RotationDirection.CounterClockwise)
        {
            direction = RotationDirection.Clockwise;
        }
        else
        {
            direction = RotationDirection.CounterClockwise;
        }

        // Set continue spinning so that the ring knows it'll need to spin a certain amount of time before being able to lock.
        isRingSpinning = true;

        // Get the rotation of the glyph on the ring.
        targetXRotation = RingGlyths.GetRingGlyphRotation(selectedGlyph) + (360 * spinMultiplier * (int)direction);
        targetGlyth = selectedGlyph;

        // Debug :D
        Debug.Log("Current target Glypth is : " + targetGlyth + " with a target rotation of : " + targetXRotation + "°.");
    }

    private void UpdateRing()
    {
        // Check if ring is spinning.
        if (isRingSpinning)
        {
            // Calculate the current progress and the amount to rotate by.
            float amoutToRotate = speedDegreesPerSecond * (int)direction * Time.deltaTime;
            if (!isRingMaintainingMomentum)
            {
                ringProgress = Mathf.InverseLerp(startXRotation, targetXRotation, currentXRotation);
                amoutToRotate *= speedCurve.Evaluate(ringProgress);
            }
            else
            {
                remainingMomentumSeconds -= Time.deltaTime;
                ringProgress = Mathf.InverseLerp(1, 0, remainingMomentumSeconds);
                amoutToRotate *= Mathf.Clamp(momentumCurve.Evaluate(ringProgress), 0, momentumClamp);
                if (remainingMomentumSeconds <= 0) isRingSpinning = false;
            }
            
            // Update the ring's X rotation.
            currentXRotation += amoutToRotate;

            // Check if ring has reached target. If so jump to final target & stop spinning.
            if (direction == RotationDirection.CounterClockwise)
            {
                if (currentXRotation >= targetXRotation)
                {
                    StopSpinningGate();
                    return;
                }
            }
            else
            {
                if (currentXRotation <= targetXRotation)
                {
                    StopSpinningGate();
                    return;
                }
            }

            // If ring hasn't reached target update it's rotation.
            ring.rotation = Quaternion.Euler(currentXRotation, Stargate.eulerAngles.y, Stargate.eulerAngles.z).normalized;

            // Debug
            // Debug.Log("Turned : " + amoutToRotate + "°, Current : " + currentXRotation + "°, Target : " + targetXRotation + "°, Progress : " + (ringProgress * 100) + "%.");
        }
    }

    private void StopSpinningGate()
    {
        // Tell the gate to stop spinning and set it to target position.
        ring.rotation = Quaternion.Euler(targetXRotation, Stargate.eulerAngles.y, Stargate.eulerAngles.z).normalized;
        isRingSpinning = false;

        // Debug
        Debug.Log("Ring has reached : " + targetGlyth + " at : " + targetXRotation + "°.");
    }

    private void SetupAnimationTimeout(float seconds)
    {
        if (isAnimationTimeout)
        {
            if (animationTimeoutAmount - animationTimeout < seconds) animationTimeout = seconds;
        }
        else
        {
            animationTimeoutAmount = seconds;
            isAnimationTimeout = true;
        }
    }

    private void UpdateAnimationTimeout()
    {
        if (isAnimationTimeout)
        {
            animationTimeout += Time.deltaTime;

            if (animationTimeout >= animationTimeoutAmount)
            {
                animationTimeout = 0;
                isAnimationTimeout = false;
            }
        }
    }
}
