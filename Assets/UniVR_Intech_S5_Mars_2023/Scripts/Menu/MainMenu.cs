using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text stargateStatus;
    [SerializeField]
    private Button gateButton;
    [SerializeField]
    private TMPro.TMP_Text gateButtonText;

    [SerializeField]
    private StargateAnimator stargate;
    [SerializeField]
    private ContinuousMovementPhysics playerController;
    [SerializeField]
    private Transform worldSpawn;

    public void Start()
    {
        gateButton.image.color = new Color(0, 0.5f, 0, 1);
        gateButtonText.text = "Start Gate";
        stargateStatus.text = "Gate Offline";
    }

    public void Update()
    {
        
    }

    // Dev utility to force the gate open for the next level.
    public void ForceToggleGate()
    {
        if (stargate.IsGateOccupied())
        {
            stargate.StargateInterrupt();

            gateButton.image.color = new Color(0, 0.5f, 0, 1);
            gateButtonText.text = "Start Gate";
            stargateStatus.text = "Gate Offline";
            Debug.Log("Force close the Stargate.");
        }
        else
        {
            // Abydos gate address (Desert World).
            stargate.StartGateSequence(new GlyphsList[]{
            GlyphsList.Taurus,
            GlyphsList.Serpens_Caput,
            GlyphsList.Capricornus,
            GlyphsList.Monoceros,
            GlyphsList.Sagittarius,
            GlyphsList.Orion,
            GlyphsList.Giza
            });

            gateButton.image.color = new Color(0.5f, 0, 0, 1);
            gateButtonText.text = "Close Gate";
            stargateStatus.text = "Gate Active";
            Debug.Log("Started Gate to Abydos !");
        }
    }

    // Dev utility to force the player to respawn (to be used in-case something goes wrong).
    public void forceRespawnPlayer()
    {
        playerController.PrepareTeleportTo(worldSpawn);
        Debug.Log("Force respawned the player back to World Spawn .");
    }

    // Button used to exit the game. Note : does not work in play test mode. It only works on a deployed application.
    public void exitApplication()
    {
        Application.Quit();
        Debug.Log("Exiting the Game.");
    }
}
