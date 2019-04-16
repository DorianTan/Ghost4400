using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementsTest : MonoBehaviour
{

    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    private SpriteRenderer spriteRendererIDL;

    [SerializeField] private Sprite action;
    [SerializeField] private Sprite IDL;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRendererIDL = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput=new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        transform.up = rb.velocity;

        if (Input.GetKeyDown("c"))
        {
            StartCoroutine(SpriteAction());
        }
    }
    IEnumerator SpriteAction()
    {
        spriteRendererIDL.sprite = action;
        yield return new WaitForSeconds(1f);
        spriteRendererIDL.sprite = IDL;
    }

    void FixedUpdate()
    {
        rb.velocity=moveVelocity;
    }
}
