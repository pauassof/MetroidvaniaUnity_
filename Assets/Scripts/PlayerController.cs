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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        if (Input.GetButtonDown("Fire1") == true && rb.velocity.y == 0)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }
    private void FixedUpdate()
    {
            rb.velocity = new Vector2(speed * horizontal, rb.velocity.y);
        

        if(jumping == true && rb.velocity.y == 0)
        {
            rb.AddForce(Vector2.up * jumpForce);
            animator.SetBool("Jump", true);
        }
        else if (rb.velocity.y == 0)
        {
            animator.SetBool("Jump", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Le he dado");
        }
    }
}
