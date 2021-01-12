using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    GameManager gameManager;

    Rigidbody rb;

    float projectileMulti = 2f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0f, -gameManager.projectileSpeed - Time.deltaTime, 0f) * projectileMulti;
        rb.AddTorque(new Vector3(0f, -gameManager.projectileSpeed - Time.deltaTime, 0f) * projectileMulti);
    }
}
