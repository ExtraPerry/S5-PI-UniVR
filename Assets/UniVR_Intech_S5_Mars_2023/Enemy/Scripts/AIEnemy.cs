using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent agent;
    private Animator anim;
    public float walkDistance = 10f;
    public float attackDistance = 2f;
    [SerializeField]
    private float distance;

    public int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance < walkDistance)
        {
            anim.SetBool("walk", true);
            anim.SetBool("attack", false);
            agent.SetDestination(target.transform.position);

            if (distance < attackDistance)
            {
                anim.SetBool("attack", true);
                agent.SetDestination(transform.position);
                
            }
        }
        else
        {
            anim.SetBool("attack", false);
            anim.SetBool("walk", false);
            agent.SetDestination(transform.position);

        }
    }
    public void disabledATK()
    {
        anim.SetBool("attack", false);
        this.enabled = false;
    }

    public void damageToPlayer()
    {
        GameObject.Find("Player").GetComponent<HealthScript>().playerDamaged(damage);
    }
}
