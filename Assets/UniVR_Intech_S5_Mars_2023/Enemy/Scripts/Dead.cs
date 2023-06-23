using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dead : MonoBehaviour
{
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    public void ennemyDead()
    {
        anim.SetTrigger("dead");
        GetComponent<AIEnemy>().disabledATK();
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 5f);
    }
}
