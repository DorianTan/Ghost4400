using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanel : MonoBehaviour
{

    [SerializeField] GameObject endPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Toggle();
        }

        if (Timer.time >= 5f)
        {
            Toggle();
            endPanel.SetActive(true);
        }
    }
    public void Toggle() // marche pour le bouton "continue"
    {
        endPanel.SetActive(!endPanel.activeSelf); //plus simple pour basculer d'un état à l'autre

        if (endPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("Scenes/Mecanics");
        IAInteract.IAFear = 0;
    }
}
