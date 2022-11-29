using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;

    public float speed;
    public float power;
    public float maxShotDelay;
    public float currentShotDelay;

    public GameObject bulletObj_A;
    public GameObject bulletObj_B;

    public GameManager manager;
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
    }

    void Move()
    {
        //Movement Logic
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))//Left, Right Movement Limit
        {
            h = 0;
        }
        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))//Top, Bottom Movement Limit
        {
            v = 0;
        }

        Vector3 currentPosition = transform.position;
        Vector3 nextPosition = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = currentPosition + nextPosition;

        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            animator.SetInteger("Input", (int)h);
        }
    }

    void Fire()
    {
        if (Input.GetKey(KeyCode.Space) && currentShotDelay >= maxShotDelay)
        {
            switch (power)
            {
                case 1:
                    GameObject bullet = Instantiate(bulletObj_A, transform.position, transform.rotation);

                    Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                    rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    currentShotDelay = 0;
                    break;
                case 2:
                    GameObject bulletL = Instantiate(bulletObj_A, transform.position + Vector3.left * 0.1f, transform.rotation);
                    GameObject bulletR = Instantiate(bulletObj_A, transform.position + Vector3.right * 0.1f, transform.rotation);
                    Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                    rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    currentShotDelay = 0;
                    break;
                case 3:
                    GameObject bulletLL = Instantiate(bulletObj_A, transform.position + Vector3.left * 0.25f, transform.rotation);
                    GameObject bulletRR = Instantiate(bulletObj_B, transform.position, transform.rotation);
                    GameObject bulletCC = Instantiate(bulletObj_A, transform.position + Vector3.right * 0.25f, transform.rotation);
                    Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                    rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    currentShotDelay = 0;
                    break;
            }
        }
        //Power One
        {
            
        }
    }

    void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Wall Recognition
        if (collision.gameObject.CompareTag("Border"))
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
            }
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            manager.RespawnPlayer(); 
            gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        //Wall Recognition
        if (collision.gameObject.CompareTag("Border"))
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
            }
        }
    }
}
