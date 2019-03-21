using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class moveDina : MonoBehaviour
{
    private Rigidbody2D rb;

    private Transform target;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<MovementsTest>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 force = target.position - transform.position;

        force = force.normalized * acceleration;

        rb.AddForce(force);

        if (rb.velocity.magnitude>maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
     
    }
}
