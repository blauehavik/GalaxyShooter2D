using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 12f;
    [SerializeField]
    private bool _isEnemyLaser = false;

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        };

    }
    void MoveDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -8f)
        {   
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyLaser == true)
        {
            if (other.tag == "Player" )
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                    Destroy(GetComponent<Collider2D>());
                }
            }
        }
    }
}
