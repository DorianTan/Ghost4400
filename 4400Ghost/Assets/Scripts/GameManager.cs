using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    public bool gameEnded = false;

    private Bsp bspScript;
    public Bsp BspScript
    {
        get { return bspScript; }
        set { bspScript = value; }
    }

    private Disjskra dijkstra;

    public Disjskra Dijkstra
    {
        get { return dijkstra; }
        set { dijkstra = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SetupScene();

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoadingScene;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoadingScene;
    }

    //this function is activated every time a scene is loaded
    private void OnLevelFinishedLoadingScene(Scene scene, LoadSceneMode mode)
    {
        SetupScene();
        Debug.Log("Scene Loaded");
    }

    void SetupScene() // initialise le niveau
    {
        dijkstra = FindObjectOfType<Disjskra>();
        bspScript = FindObjectOfType<Bsp>();
    }


}
