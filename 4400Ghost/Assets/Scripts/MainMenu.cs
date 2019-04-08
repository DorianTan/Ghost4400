using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject MenuPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }

    public void BtnStart()
    {
        SceneManager.LoadScene("Scenes/Mecanics");
    }

    public void BtnCredit()
    {
        CreditsPanel.SetActive(true);
        MenuPanel.SetActive(false);
    }

    public void BtnMenu()
    {
        CreditsPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    public void BtnExit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
