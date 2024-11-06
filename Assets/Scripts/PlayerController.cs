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
    private bool jumping;
    public bool isAttacking;
    [SerializeField] 
    private Animator animator;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float damage;
    private LevelManager levelManager;
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager=GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
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

            jumping = Input.GetButton("Jump");
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
            rb.velocity = new Vector2(speed * horizontal, rb.velocity.y);


        if (jumping == true && (rb.velocity.y < 1 && rb.velocity.y > -1) && isJumping == false)
        {
            rb.AddForce(Vector2.up * jumpForce);
            animator.SetBool("Jump", true);
            isJumping = true;
        }
        else if (rb.velocity.y == 0)
        {
            animator.SetBool("Jump", false);
        }
    }
    void WallJump(Vector2 wallNormal)
    {
        rb.AddForce((wallNormal+Vector2.up) * jumpForce);
        animator.SetBool("Jump", true);
        isJumping = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (collision.GetContact(collision.contactCount-1).normal.y >= 0.5f)
            {
                isJumping = false;
                animator.SetBool("DeslizPared", false);
            }
            else if (collision.GetContact(0).normal.y == 0)
            {
                animator.SetBool("DeslizPared", true);
                rb.velocity =new Vector2(rb.velocity.x, rb.velocity.y*0.5f);
                if (jumping == true)
                {
                    WallJump(collision.GetContact(0).normal);
                }
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
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
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
        }
    }
}
