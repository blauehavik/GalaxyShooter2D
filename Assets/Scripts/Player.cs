using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField]
    private float _speed = 5;

    void Start()
    {
        transform.position = new Vector3(0, -3.5f, 0);
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 0), 0);
        
        if (transform.position.x >= 9f)
        {
            transform.position = new Vector3(-9f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9f)
        {
            transform.position = new Vector3(9f, transform.position.y, 0);
        }
    }
}