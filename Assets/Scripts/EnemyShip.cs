
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    GameObject missilePlace;

    GameManager gameManager;

    Vector3 shipStartingPos;

    int liveCount;
    float shootTimer;
    float shootCooldown;
    float movingDownTimer;

    bool ifGameStarted;
    bool ifShipsGoDown;

    // Start is called before the first frame update
    void Start()
    {
        missilePlace = gameObject.transform.parent.GetChild(1).gameObject;

        gameManager = FindObjectOfType<GameManager>();

        gameManager.gameStartEvent += OnGameStart;

        shipStartingPos = gameObject.transform.parent.position;

        shootTimer = 0;
        shootCooldown = Random.Range(4f, 12f);
        movingDownTimer = 0f;

        CalculateLivesCount();
    }


    // Update is called once per frame
    void Update()
    {
        if (ifGameStarted)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer > shootCooldown)
            {
                ShootMissile();
            }

            MoveEnemyShip();
            MovingShipsDown();
        }
    }

    void MovingShipsDown()
    {
        movingDownTimer += Time.deltaTime;
        if(movingDownTimer > gameManager.ReturnTimeToShipsGoDown())
        {
                ifShipsGoDown = true;
                Vector3 unitsToGoDown = new Vector3(0f, -gameManager.ReturnUnitsToGoDown(), 0f);
                shipStartingPos += unitsToGoDown;

                if (shipStartingPos.y > -5f)
                {
                    gameObject.transform.root.position += unitsToGoDown;
                    movingDownTimer = 0;
                }
                else
                {
                    gameManager.GameOver(true);
                }
        }
        else
        {
            ifShipsGoDown = false;
        }
    }

    private void MoveEnemyShip()
    {
        if (!ifShipsGoDown)
        {
            Transform parentShip = gameObject.transform.parent;
            //Vector3 currentPosition = parentShip.position;
            float shipSinMovement = Mathf.Sin(Time.time) * 3;
            parentShip.position = new Vector3(shipStartingPos.x + shipSinMovement, shipStartingPos.y, shipStartingPos.z);
        }
    }

    void ShootMissile()
    {
        GameObject missile = Instantiate(Resources.Load("Prefabs/EnemyProjectile"), missilePlace.transform.position, missilePlace.transform.rotation) as GameObject;

        shootTimer = 0;
        shootCooldown = Random.Range(4f, 12f);


        //Change Missile Scale
        if (gameObject.CompareTag("StrongShip"))
        {
            missile.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
    }

    public int ReturnCurrentLivesCount()
    {
        return liveCount;
    }

    public void DecreaseLiveCount()
    {
        liveCount--;
    }

    private void CalculateLivesCount()
    {
        if (gameObject.transform.parent.name.Contains("Strong"))
        {
            liveCount = 3;
        }
        else if (gameObject.transform.parent.name.Contains("Light"))
        {
            liveCount = 1;
        }
    }

    public void OnGameStart()
    {
        ifGameStarted = true;
        FindObjectOfType<GameManager>().gameStartEvent -= OnGameStart;
    }

    private void OnDisable()
    {
        gameManager.gameStartEvent -= OnGameStart;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Collisioon");

            if(liveCount == 1)
            {
                gameManager.AddPoints(gameObject.transform.parent.name);
                Destroy(gameObject.transform.parent.gameObject);
            }
            else
            {
                liveCount--;
            }
            Destroy(other.gameObject);
        }
    }
    
}
