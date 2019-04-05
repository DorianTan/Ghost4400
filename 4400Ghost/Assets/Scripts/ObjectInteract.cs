using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private GameObject intercateText;

    private SpriteRenderer spriteRenderer;
    //[SerializeField] float range;
    bool ObjectIsGreen = false;
    bool IAIsNear = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        if (ObjectIsGreen == true) return;
        if (coll.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown("c"))
            {
                spriteRenderer.color = Color.green;
                if (IAIsNear)
                {
                    Debug.Log("IAIsNear");
                    IAInteract.IAFear += 10;
                }
                
                ObjectIsGreen = true;
                Debug.Log("ObjectIsGreen=true");
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
    

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, range);
    //}
}
