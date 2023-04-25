using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public float speed = 1f;

    public Transform p1;
    public Transform p2;
    public Rigidbody rb;

    private Vector3 targetPostion;

    // Start is called before the first frame update
    void Start()
    {
        targetPostion = p1.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (targetPostion - rb.position).normalized;
        rb.MovePosition(rb.position + ((speed * direction) * Time.fixedDeltaTime));

        if (Vector3.Distance(rb.position, targetPostion) < 0.5f)
        {
            if (targetPostion == p1.position)
                targetPostion = p2.position;
            else
                targetPostion = p1.position;
        }
    }
}
