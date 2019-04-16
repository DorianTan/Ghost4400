using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private GameObject intercateText;

    private SpriteRenderer spriteRendererIDL;

    [SerializeField] private Sprite Broken;

    //[SerializeField] float range;
    bool ObjectIsBroken = false;
    bool IAIsNear = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRendererIDL = GetComponentInChildren<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            intercateText.SetActive(true);          
        }
        if (coll.gameObject.CompareTag("IA"))
        {
            IAIsNear = true;
        }
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if (ObjectIsBroken == true) return;

        if (coll.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown("c"))
            {
                               
                spriteRendererIDL.sprite = Broken;
                if (IAIsNear)
                {
                    IAInteract.IAFear += 10;
                }
                
                ObjectIsBroken = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        
        if (coll.gameObject.CompareTag("Player"))
        {
            intercateText.SetActive(false);
        }
        if (coll.gameObject.CompareTag("IA"))
        {
            IAIsNear = false;
        }
    }
}
