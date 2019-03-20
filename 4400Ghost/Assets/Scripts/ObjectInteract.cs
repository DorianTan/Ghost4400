using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private GameObject intercateText;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            intercateText.SetActive(true);
            if (Input.GetKeyDown("m"))
            {
                Debug.Log("color");
                spriteRenderer.color = Color.green;
            }          
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown("m"))
            {
                Debug.Log("color");
                spriteRenderer.color = Color.green;
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            intercateText.SetActive(false);
        }
    }
}
