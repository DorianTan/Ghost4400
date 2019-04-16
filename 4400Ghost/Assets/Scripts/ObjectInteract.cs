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

    private AudioSource ArmoireSound;
    private AudioSource PotSound;

    // Start is called before the first frame update
    void Start()
    {
        spriteRendererIDL = GetComponentInChildren<SpriteRenderer>();

        ArmoireSound = GetComponent<AudioSource>();
        PotSound = GetComponent<AudioSource>();
        
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

                if (gameObject.tag=="Armoire")
                {
                    ArmoireSound.Play();
                }
                else
                {
                    PotSound.Play();
                }
                if (IAIsNear)
                {
                    IAInteract.IAFear += 2;
                    StartCoroutine(fearMove());
                   // GameManager.Instance.IAInteract.GirlScream.Play();
                }
                
                ObjectIsBroken = true;
            }
        }
    }

    IEnumerator fearMove()
    {
        GameManager.Instance.speedIA *= 3;
        yield return new WaitForSeconds(2f);
        GameManager.Instance.speedIA /= 3;
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
