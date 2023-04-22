using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private StargateAnimator stargate;
    [SerializeField]
    private ContinuousMovementPhysics playerController;
    [SerializeField]
    private Transform worldSpawn;

    public void Start()
    {
        
    }

    public void Update()
    {
        
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
