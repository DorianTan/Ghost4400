using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementsTest : MonoBehaviour
{

    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput=new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        transform.up = rb.velocity;

    }

    void FixedUpdate()
    {
        rb.velocity=moveVelocity;
    }
}
