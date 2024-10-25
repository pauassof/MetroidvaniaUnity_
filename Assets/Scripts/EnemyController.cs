using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float life;
    [SerializeField]
    private float speed;
    public bool playerDetected;
    [SerializeField]
    private float attackRate;
    [SerializeField]
    private float jumpForce;
    [SerializeField] 
    private float knockBackForce;
    [SerializeField]
    private Animator animator;
    private Transform player;
    [SerializeField]
    private float stopDistance;
    private Rigidbody2D rb;
    private float timePass;
    private bool isHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetected && GameManager.instance.life>0)
        {
            if (!isHit)
            {
                Vector3 distanciaEnVector = player.position - transform.position;
                Vector3 direccion = distanciaEnVector.normalized;
                float distance = distanciaEnVector.magnitude;
                if (distanciaEnVector.x > 0)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    transform.eulerAngles = Vector3.zero;
                }

                if (stopDistance < distance)
                {
                    rb.velocity = new Vector2(speed * direccion.x, rb.velocity.y);
                    animator.SetBool("Walk", true);
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    animator.SetBool("Walk", false);
                    Attack();
                }
            }
        }

        timePass += Time.fixedDeltaTime;
    }
    private void Attack()
    {
        if (timePass >= attackRate)
        {
            timePass = 0;
            animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.transform;
            playerDetected = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerDetected = false;
            rb.velocity = new Vector2 (0,rb.velocity.y);
            animator.SetBool("Walk", false);
        }
    }
    public void TakeDamage(float _damage)
    {
        life -= _damage;
        if (life <= 0)
        {
            Death();
        }
        else
        {
            isHit = true;
            animator.SetTrigger("Hit");
            rb.AddRelativeForce(transform.right * knockBackForce);
        }
    }
    void Death()
    {
        animator.SetTrigger("Death");
    }
    public void AnimationDeath()
    {
        Destroy(this.gameObject);
    }
    public void SetIsHitFalse()
    {
        isHit = false;
    }
}
