using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D rb;
    private float horizontal;
    public bool isAttacking;
    [SerializeField] 
    private Animator animator;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float damage;
    private LevelManager levelManager;
    private bool isHit;
    private int jumpCount;

    [Header("FireBall")]
    [SerializeField]
    private GameObject FireBallPrefab;
    [SerializeField]
    private Transform spawnFireBall;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float fireBallSpeed;
    [SerializeField]
    private float manaCost;
    private float fireBallTimePass;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager=GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        fireBallTimePass += Time.deltaTime;

        if (!isAttacking)
        {

            horizontal = Input.GetAxis("Horizontal");
            if (horizontal > 0)
            {
                transform.eulerAngles = Vector3.zero;
                animator.SetBool("Run", true);
            }
            else if (horizontal < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Run", false);
            }

            if (Input.GetButtonDown("Jump") && jumpCount < GameManager.instance.gameData.AirRune)
            {
                rb.AddForce(Vector2.up * jumpForce);
                animator.SetBool("Jump", true);
                jumpCount++;
            }

            if (Input.GetButtonDown("Fire2") && GameManager.instance.gameData.FireRune == true)
            {
                FireShoot();
            }
        }
        else
        {
            horizontal = 0;
        }

        if (Input.GetButtonDown("Fire1") == true && rb.velocity.y < 1 && rb.velocity.y > -1)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }
    private void FixedUpdate()
    {
        if (isHit == false)
        {
            rb.velocity = new Vector2(speed * horizontal, rb.velocity.y);
        }
    }
    void WallJump(Vector2 wallNormal)
    {
        /*rb.AddForce((wallNormal+Vector2.up) * jumpForce);
        animator.SetBool("Jump", true);*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (collision.GetContact(collision.contactCount - 1).normal.y >= 0.5f)
            {
                animator.SetBool("Jump", false);
                jumpCount = 0;
                animator.SetBool("DeslizPared", false);
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (collision.GetContact(0).normal.y == 0)
            {
                animator.SetBool("DeslizPared", true);
                rb.velocity =new Vector2(rb.velocity.x, rb.velocity.y*0.5f);
                /*if (jumping == true)
                {
                    WallJump(collision.GetContact(0).normal);
                }*/
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("DeslizPared", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            try
            {
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            }
            catch 
            {
                collision.gameObject.GetComponent<FinalBossController>().TakeDamage(damage);
            }
        }
    }
    public void TakeDamage(float _damage)
    {
        GameManager.instance.gameData.Life -= _damage;
        levelManager.UpdateLife();
        if (GameManager.instance.gameData.Life <= 0)
        {
            animator.SetTrigger("Death");
            this.enabled = false;
        }
        else
        {
            animator.SetTrigger("Hit");
            isHit = true;
            Invoke("NotHit", 0.5f);
        }
    }

    void NotHit()
    {
        isHit = false;
    }

    private void FireShoot()
    {
        if (GameManager.instance.gameData.Mana >= manaCost && fireRate <= fireBallTimePass)
        {
            GameManager.instance.gameData.Mana -= manaCost;
            levelManager.UpdateMana();
            GameObject fireBallClone = Instantiate(FireBallPrefab, spawnFireBall.position, spawnFireBall.rotation);
            fireBallTimePass = 0;
            fireBallClone.GetComponent<Rigidbody2D>().velocity = spawnFireBall.right * fireBallSpeed; //new Vector2(fireBallSpeed, 0);
            Destroy(fireBallClone, 5);
        }
        
    }
}
