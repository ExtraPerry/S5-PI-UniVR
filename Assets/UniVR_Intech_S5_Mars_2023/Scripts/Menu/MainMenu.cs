using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject stargate;
    public ContinuousMovementPhysics playerController;
    public Transform worldSpawn;

    // Dev utility to force the gate open for the next level.
    public void forceStartGate()
    {
        Debug.Log("Force started the Stargate for next level.");
    }

    // Dev utility to force the player to respawn (to be used in-case something goes wrong).
    public void forceRespawnPlayer()
    {
        playerController.teleportTo(worldSpawn);
        Debug.Log("Force respawned the player back to World Spawn .");
    }

    // Button used to exit the game. Note : does not work in play test mode. It only works on a deployed application.
    public void exitApplication()
    {
        Application.Quit();
        Debug.Log("Exiting the Game.");
    }
}
