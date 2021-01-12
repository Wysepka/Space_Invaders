using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void GameStartDelegate();
    public event GameStartDelegate gameStartEvent;
    public event GameStartDelegate gameRestartEvent;

    AsyncOperation async;

    //Ui GameObjects and texts
    GameObject menu, mainMenu, backgroundSelect, gameUI , gameOver;

    Text livesText, pointsText, totalScore, gameOverText;

    [SerializeField]enum GameState { MainMenu , BackgroundSettings , InGame , GameOver};
    [SerializeField]GameState gameState;

    string gameOverScene = "GameOver";

    private int livesCount = 3;
    [SerializeField]private int pointsCount;
    private int shipsDestroyed;
    private int shipsTotal;

    int strongShipPoints = 40;
    int lightShipPoints = 15;

    //Time in seconds to pass till ships go down
    float timeToGoDown = 15f;

    //How Many units Ships will go Down
    float unitsToMoveDown = 1.5f;

    public float projectileSpeed = 10f;

    private void Awake()
    {
        gameState = GameState.MainMenu;

        menu = gameObject.transform.GetChild(0).gameObject;
        gameUI = gameObject.transform.GetChild(1).gameObject;
        gameOver = gameObject.transform.GetChild(2).gameObject;
        mainMenu = menu.transform.GetChild(1).gameObject;
        backgroundSelect = menu.transform.GetChild(2).gameObject;

        livesText = gameUI.transform.GetChild(0).gameObject.GetComponent<Text>();
        pointsText = gameUI.transform.GetChild(1).gameObject.GetComponent<Text>();
        gameOverText = gameOver.transform.GetChild(1).gameObject.GetComponent<Text>();
        totalScore = gameOver.transform.GetChild(2).gameObject.GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(true);
        mainMenu.SetActive(true);

        pointsCount = 0;

        shipsTotal = GameObject.FindGameObjectsWithTag("LightShip").Length;
        shipsTotal += GameObject.FindGameObjectsWithTag("StrongShip").Length;
        Debug.Log(shipsTotal);

        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == GameState.MainMenu)
        {
            if (Input.anyKeyDown)
            {
                SwitchToBackgroundSelection();
            }
        }
    }

    //-----------------------UI Settings----------------------//

    void SwitchToBackgroundSelection()
    {
        gameState = GameState.BackgroundSettings;
        mainMenu.SetActive(false);
        backgroundSelect.SetActive(true);
        gameOver.SetActive(false);
    }

    public void LoadSkybox(string skybox)
    {
        //Loading Skybox Dependent On Image Gameobject Name
        string fullPathName = "Skyboxes/DeepSpace" + skybox + "DS" + skybox[0];
        Debug.Log(fullPathName);
        RenderSettings.skybox = (Material)Resources.Load("Skyboxes/DeepSpace" + skybox + "/DS" + skybox[0]);
        GameStart();
    }

    public void GameStart()
    {
        menu.SetActive(false);
        gameUI.SetActive(true);
        gameState = GameState.InGame;

        if (gameStartEvent != null)
        {
            gameStartEvent.Invoke();
        }
        else
        {
            EnemyShip[] enemyShip = FindObjectsOfType<EnemyShip>();
            foreach(EnemyShip ship in enemyShip)
            {
                ship.OnGameStart();
            }
            ShipControl shipControl = FindObjectOfType<ShipControl>();
            shipControl.OnGameStart();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        SwitchToBackgroundSelection();
        pointsCount = 0;
        livesCount = 3;
    }

    /*
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene);
        if(scene.name == "GameOver")
        {
            Debug.Log("Game over scene loaded");
            totalScore.text ="Total Score: " + pointsCount.ToString();
            totalScore.rectTransform.ForceUpdateRectTransforms();
        }
        else if(scene.name == "Game")
        {

        }
    }
    */

    //------------------game Settings-------------------//

    public void AddPoints(string shipName)
    {
        if (shipName.Contains("Strong"))
        {
            pointsCount += strongShipPoints;
        }
        else if (shipName.Contains("Light"))
        {
            pointsCount += lightShipPoints;
        }

        shipsDestroyed++;

        CheckForEndGameSettings();
        DisplayPointsAndLives();
    }

    void DisplayPointsAndLives()
    {
        pointsText.text = "Points: " + pointsCount;
        livesText.text = "Lives: " + livesCount;
    }

    public void DecreaseLive()
    {
        livesCount--;

        CheckForEndGameSettings();
        DisplayPointsAndLives();
    }

    void CheckForEndGameSettings()
    {
        if(shipsDestroyed >= shipsTotal)
        {
            GameOver(false);
        }
        if(livesCount <= 0)
        {
            GameOver(true);
        }
    }

    //---------------Game Over Settings-------------------//

    public void GameOver(bool ifLost)
    {
        StaticStats.pointsTotal = pointsCount;
        if (ifLost)
        {
            StaticStats.gameResult = "You Lost !";
        }
        else
        {
            StaticStats.gameResult = "You Won !";
        }

        if (gameState == GameState.InGame)
        {
            Time.timeScale = 0f;
            gameState = GameState.GameOver;
            //gameOver.SetActive(true);
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(gameOverScene);
        async.allowSceneActivation = false;
        while (async.progress <= 0.89f)
        {
            Debug.Log(async.progress.ToString());
            yield return null;
        }
        Time.timeScale = 1f;
        async.allowSceneActivation = true;
    }

    //-------------------Return Functions------------------//

    public float ReturnTimeToShipsGoDown()
    {
        return timeToGoDown;
    }

    public float ReturnUnitsToGoDown()
    {
        return unitsToMoveDown;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
