using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAmouvements : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        target=FindObjectOfType<MovementsTest>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.velocity = target.position - transform.position; //plus on est loin plus on va vite / plus en proche plus on ralentit

        rb.velocity = rb.velocity.normalized*speed;
    }
}
