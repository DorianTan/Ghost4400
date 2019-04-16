using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAInteract : MonoBehaviour
{
    [SerializeField] private Slider CarburantSlider;
    public static float IAFear=0;

    public AudioSource GirlScream;


    // Start is called before the first frame update
    void Start()
    {
        CarburantSlider.maxValue = 10;
        CarburantSlider.value = IAFear;

        GirlScream = GetComponent<AudioSource>();

    }
  
    void Update()
    {
        CarburantSlider.value = IAFear;
    }
}
