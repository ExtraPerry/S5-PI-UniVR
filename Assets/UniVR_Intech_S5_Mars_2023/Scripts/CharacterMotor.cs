using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{

    public Animator controller;


    // Start is called before the first frame update
    void Start()
    {
        controller.SetBool("isDead", false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
