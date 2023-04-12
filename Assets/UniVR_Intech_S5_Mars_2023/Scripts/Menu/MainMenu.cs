using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMPro.TMP_Text stargateStatus;
    public Button gateButton;
    public TMPro.TMP_Text gateButtonText;
    public Animator stargate;
    public ContinuousMovementPhysics playerController;
    public Transform worldSpawn;

    public void Update()
    {
        stargateStatus.text = stargate.GetCurrentAnimatorStateInfo(0).ToString();
    }

    // Dev utility to force the gate open for the next level.
    public void ForceToggleGate()
    {
        if (!stargate.GetBool("StartGate"))
        {
            stargate.SetBool("StartGate", true);
            gateButton.image.color = new Color(0.5f, 0, 0, 1);
            gateButtonText.text = "Close Gate";
            Debug.Log("Force started the Stargate for next level.");
        }
        else
        {
            stargate.SetBool("StartGate", false);
            gateButton.image.color = new Color(0, 0.5f, 0, 1);
            gateButtonText.text = "Start Gate";
            Debug.Log("Force close the Stargate.");
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
