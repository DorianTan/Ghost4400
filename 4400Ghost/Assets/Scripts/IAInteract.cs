using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAInteract : MonoBehaviour
{
    [SerializeField] private Slider CarburantSlider;
    // [SerializeField] float range;
    [SerializeField] float IAFear=0;
    [SerializeField] GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        CarburantSlider.maxValue = 10;
        CarburantSlider.value = IAFear;
    }
    // Update is called once per frame
    void Update()
    {
        CarburantSlider.value = IAFear;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, range);
    //}
}
