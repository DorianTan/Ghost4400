using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private GameObject endPanelWin;
    [SerializeField] GameObject endPanelLose;
    [SerializeField] float partyTime=25f;

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Timer.time >= partyTime)// durée d'une partie en secondes si le joueur perds  
        {
            endPanelLose.SetActive(true);
            Time.timeScale = 0f;

        }

        if (IAInteract.IAFear==10)
        {
            endPanelWin.SetActive(true);
            Time.timeScale = 0f;
        }

    }


    public void Retry()
    {
        SceneManager.LoadScene("Scenes/Lvl");
        endPanelLose.SetActive(false);
        endPanelWin.SetActive(false);
        IAInteract.IAFear = 0;
        Time.timeScale = 1f;
        
    }
}
