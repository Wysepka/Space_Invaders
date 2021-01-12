using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    GameManager gameManager;

    GameObject projectileMissile;

    float shipSpeed = 0.15f;
    private float shootCooldown = 0;

    [SerializeField]bool ifGameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.gameStartEvent += OnGameStart;

        projectileMissile = gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (ifGameStarted)
        {
            shootCooldown += Time.deltaTime;
            ShipControls();
        }
    }

    void ShipControls()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (gameObject.transform.position.x > -48f)
            {
                gameObject.transform.Translate(new Vector3(-shipSpeed - Time.deltaTime, 0, 0));
            }
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (gameObject.transform.position.x < -12f)
            {
                gameObject.transform.Translate(new Vector3(shipSpeed + Time.deltaTime, 0, 0));
            }
        }

        //Shoot Cooldown to pretend spamming
        if (shootCooldown > 0.4f && Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectile = Instantiate(Resources.Load("Prefabs/Projectile"), projectileMissile.transform.position, projectileMissile.transform.rotation) as GameObject;
            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("Projectiles").transform);
            shootCooldown = 0;
        }
            
    }

    public void OnGameStart()
    {
        ifGameStarted = true;
        gameManager.gameStartEvent -= OnGameStart;
    }

    private void OnDisable()
    {
        gameManager.gameStartEvent -= OnGameStart;
    }
}
