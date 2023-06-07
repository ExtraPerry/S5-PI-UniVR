using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tir : MonoBehaviour
{
    public AudioClip SoundShoot, SoundReload, SoundEmpty;
    private Ray ray;
    private RaycastHit hit;

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

            if(Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
            {
                if(hit.transform.gameObject.tag == "Enemy")
                {
                    Destroy(hit.transform.gameObject);
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
