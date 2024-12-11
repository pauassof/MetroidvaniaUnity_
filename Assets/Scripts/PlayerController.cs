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

    [Header("Dash")]
    [SerializeField]
    private float manaDash;
    [SerializeField]
    private float dashForce;
    private bool dash;


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

    [Header("SFX")]
    [SerializeField]
    private AudioClip espadazoSFX, hitSFX, deathSFX, fireballSFX, jumpSFX, pasosSFX;


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

#if UNITY_ANDROID == false
            horizontal = Input.GetAxis("Horizontal");
#endif
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
                AudioManager.instance.PlaySFX(jumpSFX, 1);
            }

            if (Input.GetButtonDown("Fire2") && GameManager.instance.gameData.FireRune == true)
            {
                FireShoot();
            }
            if (Input.GetButtonDown("Fire3") && GameManager.instance.gameData.DashRune == true)
            {
                
                Dash();
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
            AudioManager.instance.PlaySFX(espadazoSFX, 1);
        }
    }

    public void JumpButton()
    {
        if (jumpCount < GameManager.instance.gameData.AirRune)
        {
            rb.AddForce(Vector2.up * jumpForce);
            animator.SetBool("Jump", true);
            jumpCount++;
            AudioManager.instance.PlaySFX(jumpSFX, 1);
        }
    }

    public void MoveButtonDown( int _horizontal)
    {
        horizontal = _horizontal;
    }
    public void MoveButtonUp()
    {
        horizontal = 0;
    }

    public void AttackButton()
    {
        if (rb.velocity.y < 1 && rb.velocity.y > -1)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
            AudioManager.instance.PlaySFX(espadazoSFX, 1);
        }
    }
    public void FireButton()
    {
        if (GameManager.instance.gameData.FireRune == true)
        {
            FireShoot();
        }
    }

    private void FixedUpdate()
    {
        if (isHit == false && dash == false)
        {
            rb.velocity = new Vector2(speed * horizontal, rb.velocity.y);
        }
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
            AudioManager.instance.PlaySFX(deathSFX, 1);
            levelManager.DeathPanel();
        }
        else
        {
            animator.SetTrigger("Hit");
            isHit = true;
            Invoke("NotHit", 0.5f);
            AudioManager.instance.PlaySFX(hitSFX, 1);
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
            AudioManager.instance.PlaySFX(fireballSFX, 1);
        }
        
    }

    private void Dash()
    {
        if (jumpCount == 0 && GameManager.instance.gameData.Mana >= manaCost)
        {
        
        dash = true;
        GameManager.instance.gameData.Mana -= manaDash;
        levelManager.UpdateMana();
        rb.AddForce(transform.right * dashForce);
        animator.SetTrigger("Dash");
        gameObject.layer = 10;
        }
    }
    public void ChangeLayer()
    {
        gameObject.layer = 0;
        dash = false;
    }

    public void PlayStepSound()
    {
        AudioManager.instance.PlaySFX(pasosSFX, 1);
    }
}
