using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    public void UpdateLife(int vie)
    {
        vie = Mathf.Clamp(vie, 0, 100);
    }
}
