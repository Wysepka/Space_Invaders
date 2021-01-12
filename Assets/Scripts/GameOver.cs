using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    Text stats, points;

    private void Awake()
    {
        stats = gameObject.transform.GetChild(1).GetComponent<Text>();
        points = gameObject.transform.GetChild(2).GetComponent<Text>();

        stats.text = StaticStats.gameResult;
        points.text = "Total Score: " + StaticStats.pointsTotal;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
