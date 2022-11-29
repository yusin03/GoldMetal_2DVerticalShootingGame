using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float speed;
    public int hp;
    public Sprite[] sprites;

    public float maxShotDelay;
    public float currentShotDelay;

    public GameObject bulletObj_A;
    public GameObject bulletObj_B;
    public GameObject player;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Fire();
        Reload();
    }
    
    void OnHit(int dmg)
    {
        hp -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void Fire()
    {
        if(currentShotDelay >= maxShotDelay)
        {
            if (enemyName == "S")
            {
                GameObject bullet = Instantiate(bulletObj_A, transform.position, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

                Vector3 dirVec = player.transform.position - transform.position;
                rigid.AddForce(dirVec.normalized * 8, ForceMode2D.Impulse);
                currentShotDelay = 0;
            }
            else if (enemyName == "L")
            {
                GameObject bulletR = Instantiate(bulletObj_A, transform.position + Vector3.right * 0.3f, transform.rotation);
                GameObject bulletL = Instantiate(bulletObj_A, transform.position + Vector3.left * 0.3f, transform.rotation);

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

                Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
                Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);

                rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
                rigidL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);
                currentShotDelay = 0;
            }
        }
    }

    void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("BulletBorder"))
        {
            Destroy(gameObject);
        }
        else if(collision.transform.CompareTag("PlayerBullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            Destroy(collision.gameObject); 
        }
    }
}
