using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tir : MonoBehaviour
{
    public AudioClip SoundShoot, SoundReload, SoundEmpty;
    private Ray ray;
    private RaycastHit hit;
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private LayerMask hitLayer;

    public int cartouches, chargeurs, max_cartouches;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && cartouches> 0)
        {
            cartouches -=1;

            GetComponent<AudioSource>().PlayOneShot(SoundShoot);

            Vector2 ScreenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

            ray = Camera.main.ScreenPointToRay(ScreenCenterPoint);

            if(Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hit, 20f, hitLayer, QueryTriggerInteraction.Ignore))
            {
                if(hit.transform.gameObject.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<Dead>().ennemyDead();
                }
            }
        }

        //Recharge Fusil
        if (Input.GetKeyDown(KeyCode.R) && chargeurs>0)
        {
            GetComponent<AudioSource>().PlayOneShot(SoundReload);
            chargeurs -= 1;
            cartouches += (max_cartouches-cartouches);
        }

        //Plus de cartouches
        if (Input.GetButtonDown("Fire1") && cartouches == 0)
        {
            GetComponent<AudioSource>().PlayOneShot(SoundEmpty);
        }
    }
    
}
