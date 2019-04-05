using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private GameObject endPanelWin;
    [SerializeField] GameObject endPanelLose;
    [SerializeField] float partyTime=25f;
    [SerializeField] private Rigidbody2D rgPlayer;

    // Update is called once per frame
    void Update()
    {

        if (Timer.time >= partyTime)// durée d'une partie en secondes si le joueur perds  
        {
            endPanelLose.SetActive(true);
            Timer.time=partyTime;
            Time.timeScale = 0;

        }

        if (IAInteract.IAFear==10)
        {
            endPanelWin.SetActive(true);
            Time.timeScale = 0;
        }

    }


    public void Retry()
    {
        SceneManager.LoadScene("Scenes/Mecanics");
        IAInteract.IAFear = 0;
        Time.timeScale = 1;
        Debug.Log(("fkjsh"));
        
    }
}
