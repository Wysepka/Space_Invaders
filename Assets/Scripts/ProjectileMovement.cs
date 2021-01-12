using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    GameManager gameManager;

    Rigidbody rb;

    float projectileSpeed;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0f, gameManager.projectileSpeed + Time.deltaTime, 0f);
        rb.AddTorque(new Vector3(0f, gameManager.projectileSpeed + Time.deltaTime, 0f));
        //gameObject.transform.Translate(new Vector3(0f, gameManager.projectileSpeed + Time.deltaTime, 0f));
    }

    
    /*
    private void OnTriggerEnter(Collider other)
    {
        string colliderTag = other.gameObject.transform.parent.tag;
        if (colliderTag.Contains("Ship"))
        {
            if (colliderTag.Contains("Light"))
            {
                Destroy(other.gameObject.transform.parent);
                DestroyProjectile();
            }
            else if (colliderTag.Contains("Strong"))
            {
                EnemyShip enemyShip = other.gameObject.GetComponent<EnemyShip>();
                if(enemyShip.ReturnCurrentLivesCount() <= 0)
                {
                    Destroy(other.gameObject.transform.parent.gameObject);
                    DestroyProjectile();
                }
                else
                {
                    enemyShip.DecreaseLiveCount();
                    DestroyProjectile();
                }
            }
        }

        else if (other.CompareTag("ProjectileTrigger"))
        {
            DestroyProjectile();
        }
    }
    */
    

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
