using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int vie = 100;

    public void playerDamaged(int damage)
    {
        if(vie>0) vie -= damage;
        
    }
}
