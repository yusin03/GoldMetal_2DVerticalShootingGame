using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("BulletBorder"))
        {
            Destroy(gameObject);
        }
    }
}
