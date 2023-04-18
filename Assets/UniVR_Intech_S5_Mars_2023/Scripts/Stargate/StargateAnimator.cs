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

    public static float GetRingGlyphRotation(GlyphsList glyphs)
    {
        return ringGlyphsRotationValue[(int)glyphs];
    }
}

public class StargateAnimator : MonoBehaviour
{
    // Public Fields.
    [Range(0, 360)]
    public float speed = 120f;
    public AnimationCurve speedCurve = new AnimationCurve(new Keyframe[] {
        new Keyframe(0, 0.1f),
        new Keyframe(0.1f, 0.35f, 1.81539f, 1.815319f),
        new Keyframe(0.33f, 0.5f),
        new Keyframe(0.66f, 0.5f),
        new Keyframe(1, 0.1f),
    });

    // Setup fields.
    public Animator animator;
    public Transform Stargate;
    public Transform ring;

    // Program attributes.
    private GlyphsList[] storedGlyphsSequence = null;
    [SerializeField] // Let's you manually change the value inside of the editor, but not in the true game.
    private GlyphsList selectedGlyph;
    [SerializeField]
    private GlyphsList oldGlyph;

    // Ring rotation animation attributes.
    private Quaternion originalRotation;
    private Quaternion currentRotation;
    private Quaternion targetQuaternion;
    private float originalToTargetAngle = 0;

    // Event Triggers.
    private int chevronLvl = 0;
    private bool isGateActive = false;
    [SerializeField] // Let's you manually change the value inside of the editor, but not in the true game.
    private bool interruptGate = false;

    // Timeout parameters.
    private bool isAnimationTimeout = false;
    private float animationTimeoutAmount = 1;
    private float animationTimeout = 0;

    // Gate status.
    private bool isGateOccupied = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // Only update the rings position each time a frame is generated. Don't bother updating it if another frame isn't generated :D.
    void Update()
    {
        // Needs to be checked every frame for consistency.
        UpdateAnimationTimeout();
        if (isAnimationTimeout) return; // Check if animator is still animating a chevron.

        // If no address is in the register nothing to dial.
        if (storedGlyphsSequence == null)
        {
            if (interruptGate) interruptGate = false;
            return;
        }
            
        // If a Gate interrupt is called.
        if (interruptGate)
        {
            if (isGateActive)
            {
                CloseStargate();
            }
            else
            {
                ResetStargate();
            }
        }

        // If the Gate isn't activated check the Update the Ring.
        if (!isGateActive)
        {
            UpdateRingDiallingSequence();
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
    public void StartGateSequence(GlyphsList[] sequence)
    {
        if (sequence.Length == 7 && !isGateOccupied)
        {
            storedGlyphsSequence = sequence;
            selectedGlyph = storedGlyphsSequence[0];
            isGateOccupied = true;
        }
    }

    public bool IsGateOccupied()
    {
        return isGateOccupied;
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
    }

    private void ResetStargate()
    {
        storedGlyphsSequence = null;
        chevronLvl = 0;
        animator.SetInteger("ChevronsLocked", chevronLvl);
        interruptGate = false;
        isGateOccupied = false;
        SetupAnimationTimeout(0.15f);
    }

    private void UpdateRingDiallingSequence()
    {
        if (selectedGlyph != oldGlyph)
        {
            SetupRingForRotation();
        }

        if (currentRotation != targetQuaternion)
        {
            UpdateRing();
        }
        else if (chevronLvl <= 7)
        {
            // Both lines need to be checked independantly withing the instant between each line execution.
            if (chevronLvl <= 6) chevronLvl++;
            if (chevronLvl <= 6) selectedGlyph = storedGlyphsSequence[chevronLvl];

            // Update the animator's parameter too.
            animator.SetInteger("ChevronsLocked", chevronLvl);
            // Prep the script to wait for the animation to end.
            SetupAnimationTimeout(1);
            if (chevronLvl == 7)
            {
                chevronLvl++; // Make this higher than the number of chevrons on the gate to not trigger this event again unless gate is reset.
                isGateActive = true;
            }
        }
    }

    private void SetupRingForRotation()
    {
        // Get the rotation of the glyph on the ring.
        float targetRotation = RingGlyths.GetRingGlyphRotation(selectedGlyph);
        targetQuaternion = Quaternion.Euler(targetRotation, Stargate.eulerAngles.y, Stargate.eulerAngles.z).normalized;
        oldGlyph = selectedGlyph;

        // Remember what the starting rotation is at.
        originalRotation = ring.rotation.normalized;

        // Calculate the total angle that will be traversed.
        originalToTargetAngle = Quaternion.Angle(originalRotation, targetQuaternion);

        // Debug :D
        Debug.Log("Current Selected Glypth is : " + selectedGlyph + " with a target rotation of : " + targetRotation + "° and a planned traverse angle of : " + originalToTargetAngle + "°.");
    }

    private void UpdateRing()
    {
        // Update the rotation of the Stargate's ring, requires that SetupRingForRotation() be used before if you want to change the target rotation.
        // !!! Note it aligns to X & Y but rotating the Stargate on Z will break it :'D !!!

        // Calculate progress % of how close it's getting to the target.
        currentRotation = ring.rotation.normalized;
        float currentAngleProgress = Quaternion.Angle(originalRotation, currentRotation);
        float progress = Mathf.InverseLerp(0f, originalToTargetAngle, currentAngleProgress);

        // Rotate the ring using the time curve over time & base speed value.
        ring.rotation = Quaternion.RotateTowards(currentRotation, targetQuaternion, speedCurve.Evaluate(progress) * speed * Time.deltaTime);

        // Debug :D
        // Debug.Log("Progress : " + progress + ", Traverse planned : " + originalToTargetAngle + "°, Current : " + currentAngleProgress + "°.");
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
